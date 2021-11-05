import { OnDestroy, QueryList } from '@angular/core';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import * as SimplePeer from 'simple-peer';
import { VideochatDataService } from './videochat.data.service';
import { AccountDataService } from "../account/account.data.service"
import { Router } from '@angular/router';
import { ToastService } from '../toast.service';
import { decode } from 'punycode';
import { Member } from './member';
import { AudioService } from '../audio.service';
@Component({
  selector: 'app-videochat',
  templateUrl: './videochat.component.html',
  styleUrls: ['./videochat.component.css'],
})
export class VideochatComponent implements OnInit, OnDestroy {
  @ViewChild("callerVideo") callerVideo: ElementRef = new ElementRef("");
  @ViewChild("myVideo") myVideo: ElementRef = new ElementRef("");
  @ViewChild("screenVideo") screenVideo: ElementRef = new ElementRef("");
  localPeer: any = null;
  remotePeer: any = null;
  localStream: any = null;
  remoteStream: any = null;
  screenStream: MediaStream = new MediaStream();
  mediaConfig: any = { video: true, audio: true };
  myAudioState: boolean = true;
  myVideoState: boolean = true;
  screenState: boolean = false;
  callerAudioState: boolean = true;
  callerVideoState: boolean = true;
  //callerScreenState: boolean = false;
  callerData: any = {};
  chatRoom: any = {};
  constructor(private dataService: VideochatDataService, public accountDataService: AccountDataService, private router: Router, private toastService: ToastService, private audioService: AudioService) { }
  toggleAudio() {
    this.myAudioState = !this.myAudioState;
    if (this.dataService.calling != null) {
      this.dataService.hubConnection.invoke("ToggleState", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), this.dataService.calling, this.myAudioState, "Audio");
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
      this.dataService.hubConnection.invoke("ToggleState", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), this.dataService.calling, this.myVideoState, "Video");
    }
  }
  toggleScreen() {
    this.screenState = !this.screenState;
    this._changeScreenState().then(() => {
      if (this.dataService.calling != null) {
        this.dataService.hubConnection.invoke("ToggleState", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), this.dataService.calling, this.screenState, "Screen");
      }
    })
  }
  _changeScreenState() {
    return new Promise((resolve, reject) => {
      var video = this.screenVideo.nativeElement;
      if (!this.screenState) {
        this.loadVideo(video, null);
        this.screenStream?.getTracks()?.forEach((track: any) => {
          this.localPeer.removeTrack(track, this.localStream);
          track?.stop();
          this.screenStream = new MediaStream();
        });
        resolve("");
      } else {
        video.muted = true;
        //@ts-ignore
        navigator.mediaDevices.getDisplayMedia({ video: true, audio: true }).then((stream: any) => {
          this.loadVideo(video, stream);
          this.screenStream = stream
          stream.getTracks()?.forEach((track: any) => {
            this.localPeer.addTrack(track, this.localStream);
          });
          resolve("");
        });
      };
    })
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
  CheckVideo() {
    return new Promise(async (resolve, reject) => {
      if (this.localStream == null) {
        this.mediaConfig = await this.checkMedia();
        navigator.mediaDevices.getUserMedia(this.mediaConfig).then((stream) => {
          var video = this.myVideo.nativeElement;
          video.muted = true;
          this.loadVideo(video, stream);
          this.localStream = stream;
          resolve("");
        }).catch((err: any) => { console.log(err) });
      } else {
        resolve("");
      }
    });
  }
  createInitiatorPeer(email: string) {
    var peer = new SimplePeer({ "initiator": true });
    peer.addStream(this.localStream);
    peer.on("error", (error: any) => { });
    peer.on("connect", () => {
      peer.send(this.accountDataService.currentAccount.accountImage + "@img@" + this.accountDataService.currentAccount.nickName);
    })
    //if (this.dataService.calling != null) {
    peer.on("signal", (data: any) => {
      this.dataService.hubConnection.invoke("StartCall", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), { Email: email }, { Data: data, Dst: "RemotePeer" });
    });
    //}
    return peer;
  }
  createRemotePeer(email: string) {
    var peer = new SimplePeer({ initiator: false });
    peer.on("error", (error: any) => { });
    peer.on("signal", (data: any) => {
      this.dataService.hubConnection.invoke("StartCall", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), { Email: email }, { Data: data, Dst: "LocalPeer" });
    });
    peer.on("connect", () => {
      if (this.dataService.calling["Caller"] == false) {
        this.CheckVideo().then(() => {
          this.chatRoom[email]["localPeer"] = this.createInitiatorPeer(email);
        })
      }
    });
    peer.on("stream", (stream: any) => {
      var video = this.callerVideo.nativeElement;
      this.loadVideo(video, stream);
      this.chatRoom[email]["remoteStream"] = stream;
    });
    peer.on("data", (data: any) => {
      var decoded = new TextDecoder().decode(data).split("@img@");
      this.chatRoom[email]["accountImage"] = decoded[0];
      this.chatRoom[email]["nickName"] = decoded[1];
    })
    return peer;
  }
  destroyPeers() {
    this.localPeer?.destroy();
    this.remotePeer?.destroy();
    if (this.localPeer != null) {
      this.dataService.hubConnection.invoke("DestroyConnection", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), this.dataService.calling).then(() => {
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
    this.screenStream?.getTracks()?.forEach(function (track: any) {
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
        this.chatRoom[caller]["remotePeer"].signal(data.Data);
      } else {
        this.chatRoom[caller]["localPeer"].signal(data.Data);
      }
    });
    this.dataService.addConnectionListener("ToggleVideo", async (state: boolean) => {
      this.callerVideoState = state;
    });
    this.dataService.addConnectionListener("ToggleAudio", (state: boolean) => {
      var video = this.callerVideo.nativeElement;
      video.muted = !state;
      this.callerAudioState = state;
    });
    this.dataService.addConnectionListener("ToggleScreen", (state: boolean) => {
      var video = this.screenVideo.nativeElement;
      video.muted = false;
      this.screenState = !this.screenState;
      if (state) {
        setTimeout(async () => {
          var tracks = await this.remoteStream?.getTracks();
          if (tracks != undefined && tracks[3] != undefined) {
            this.screenStream.addTrack(tracks[1]);
            this.screenStream.addTrack(tracks[3]);
          } else {
            this.screenStream.addTrack(tracks[2]);
          }
          this.loadVideo(video, this.screenStream);
        }, 1000);
      } else {
        this.loadVideo(video, null);
        this.screenStream = new MediaStream();
      }
    });
    this.dataService.addConnectionListener("DestroyConnection", () => {
      this.leaveVideoChat();
      this.toastService.showAlert("Звонок завершен");
    });
    this.dataService.addConnectionListener("AcceptCall", (email: string) => {
      this.dataService.hubConnection.invoke("NeedToConnect", email, Object.keys(this.chatRoom)).then(() => {
        this.createChatMember(email);
      });
    });
    this.dataService.addConnectionListener("OfferToConnect", (members: string[]) => {
      setTimeout(() => {
        members.forEach((email) => {
          if (this.chatRoom[email]["localPeer"] == null) {
            var peer = this.createInitiatorPeer(email);
            this.chatRoom[email]["localPeer"] = peer;
          }
        })
      }, 1000)
    });
  };

  createChatMember(email: string) {
    var remotePeer = this.createRemotePeer(email);
    var localPeer = this.createInitiatorPeer(email);
    this.chatRoom[email] = new Member(localPeer, remotePeer, this.localStream, null);
  }
  createRemotePeers() {
    this.dataService.members.filter((member) => member.email != localStorage.getItem("CURRENT_COMMUNICATION_EMAIL")).forEach((member) => {
      var email = member.email
      var remotePeer = this.createRemotePeer(email);
      this.chatRoom[email] = new Member(null, remotePeer);
    })
  }
  createLocalPeers() {
    //this.dataService.members.filter((member) => member.email != localStorage.getItem("CURRENT_COMMUNICATION_EMAIL")).forEach((member) => {
    //  var email = member.email
    //  var localPeer = this.createInitiatorPeer(email);
    //  this.chatRoom[email]["localPeer"] = localPeer;
    //  this.chatRoom[email]["localStream"] = this.localStream;
    //})
  } //DELETED
  ngOnInit(): void {
    this.subscribeHubEvents();
    if (this.dataService.calling == null) {
      this.createRemotePeers();
    }
    this.CheckVideo().then(() => {
      this.dataService.checkConnection()
      //if (this.dataService.calling != null) {
      //  this.createLocalPeers();
      //}
    });
  }
  ngOnDestroy(): void {
    this.destroyPeers();
  }
}
