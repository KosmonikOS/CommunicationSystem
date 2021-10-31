import { OnDestroy, QueryList } from '@angular/core';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import * as SimplePeer from 'simple-peer';
import { VideochatDataService } from './videochat.data.service';
import { AccountDataService } from "../account/account.data.service"
import { Router } from '@angular/router';
import { ToastService } from '../toast.service';
@Component({
  selector: 'app-videochat',
  templateUrl: './videochat.component.html',
  styleUrls: ['./videochat.component.css'],
})
export class VideochatComponent implements OnInit, OnDestroy {
  @ViewChild("callerVideo") callerVideo: ElementRef = new ElementRef("");
  @ViewChild("myVideo") myVideo: ElementRef = new ElementRef("");
  localPeer: any = null;
  remotePeer: any = null;
  localStream: any = null;
  remoteStream: any = null;
  mediaConfig: any = { video: true, audio: true };
  myAudioState: boolean = true;
  myVideoState: boolean = true;
  myScreenState: boolean = false;
  callerAudioState: boolean = true;
  callerVideoState: boolean = true;
  callerScreenState: boolean = false;
  constructor(private dataService: VideochatDataService, public accountDataService: AccountDataService, private router: Router, private toastService: ToastService) { }
  toggleAudio() {
    this.myAudioState = !this.myAudioState;
    if (this.dataService.calling != null) {
      this.dataService.hubConnection.invoke("ToggleState", this.dataService.calling, this.myAudioState, "Audio");
    }
  }
  toggleVideo() {
    var video = this.myVideo.nativeElement;
    if (this.myVideoState) {
      this.loadVideo(video, null);
      this.mediaConfig.video = false;
    } else {
      this.loadVideo(video, this.localStream);
      this.mediaConfig.video = true;
    }
    this.myVideoState = !this.myVideoState
    if (this.dataService.calling != null) {
      this.dataService.hubConnection.invoke("ToggleState", this.dataService.calling, this.myVideoState, "Video");
    }
  }
  toggleScreen() {
    this.myScreenState = !this.myScreenState;
  }
  loadVideo(video: any, stream: any) {
    if ('srcObject' in video) {
      video.srcObject = stream
    } else {
      video.src = window.URL.createObjectURL(stream)
    }
  }
  checkMedia(): Promise<any> {
    return new Promise((resolve, reject) => {
      navigator.mediaDevices.enumerateDevices().then((devices) => {
        var video = false;
        var audio = false;
        devices.forEach(function (device) {
          video = video || device.kind.indexOf("video") > -1 ? true : false;
          audio = audio || device.kind.indexOf("audio") > -1 ? true : false;
        });
        resolve({ video: video, audio: audio });
      });
    })
  }
  async turnOnVideo() {
    this.mediaConfig = await this.checkMedia();
    return new Promise((resolve, reject) => {
      navigator.mediaDevices.getUserMedia(this.mediaConfig).then((stream) => {
        var video = this.myVideo.nativeElement;
        video.muted = true;
        this.loadVideo(video, stream);
        this.localStream = stream;
        resolve("");
      }).catch((err: any) => { console.log(err) });
    });
  }
  createInitiatorPeer() {
    this.localPeer = new SimplePeer({ "initiator": true });
    this.localPeer.addStream(this.localStream);
    this.localPeer.on("error", (error: any) => { });
    if (this.dataService.calling != null) {
      this.localPeer.on("signal", (data: any) => {
        this.dataService.hubConnection.invoke("StartCall", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), this.dataService.calling, { Data: data, Dst: "RemotePeer" });
      });
    }
  }
  createRemotePeer() {
    this.remotePeer = new SimplePeer({ initiator: false });
    this.remotePeer.on("error", (error: any) => { });
    this.remotePeer.on("signal", (data: any) => {
      this.dataService.hubConnection.invoke("StartCall", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), this.dataService.calling, { Data: data, Dst: "LocalPeer" });
    });
    this.remotePeer.on("connect", () => {
      if (this.dataService.calling["Caller"] == false) {
        this.createInitiatorPeer();
      }
    });
    this.remotePeer.on("stream", (stream: any) => {
      var video = this.callerVideo.nativeElement;
      this.loadVideo(video, stream);
      this.remoteStream = stream;
    });
  }
  destroyPeers() {
    this.localPeer?.destroy();
    this.remotePeer?.destroy();
    if (this.localPeer != null) {
      this.dataService.hubConnection.invoke("DestroyConnection", this.dataService.calling).then(() => {
        this.dataService.closeConnection();
      })
    } else {
      this.dataService.closeConnection();
    }
    this.localStream?.getTracks()?.forEach(function (track: any) {
      track?.stop();
    });
    this.remoteStream?.getTracks()?.forEach(function (track: any) {
      track?.stop();
    });
    this.dataService.calling = null;
    this.localPeer = null;
  };
  leaveVideoChat() {
    this.router.navigate(["/messenger"]);
  }
  subscribeHubEvents() {
    this.dataService.addConnectionListener("Accept", (caller: string, data: any) => {
      if (this.dataService.calling == null) {
        this.dataService.calling = { Email: caller, Caller: false };
      }
      if (data.Dst == "RemotePeer") {
        this.remotePeer.signal(data.Data);
      } else {
        this.localPeer.signal(data.Data);
      }
    });
    this.dataService.addConnectionListener("ToggleVideo", (state: boolean) => {
      var video = this.callerVideo.nativeElement;
      if (state) {
        this.loadVideo(video, this.remoteStream);
      } else {
        this.loadVideo(video, null);
      }
      this.callerVideoState = state;
    });
    this.dataService.addConnectionListener("ToggleAudio", (state: boolean) => {
      var video = this.callerVideo.nativeElement;
      video.muted = !state;
      this.callerAudioState = state;
    });
    this.dataService.addConnectionListener("DestroyConnection", () => {
      this.leaveVideoChat();
      this.toastService.showSuccess("Звонок завершен");
    });
  };
  ngOnInit(): void {
    this.subscribeHubEvents();
    this.turnOnVideo().then(() => {
      this.dataService.startConnection().then(() => {
        this.createRemotePeer();
        if (this.dataService.calling != null) {
          this.createInitiatorPeer();
        }
      });
    });
  }
  ngOnDestroy(): void {
    this.destroyPeers();
  }
}
