import { QueryList } from '@angular/core';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import * as SimplePeer from 'simple-peer';
import { VideochatDataService } from './videochat.data.service';
@Component({
  selector: 'app-videochat',
  templateUrl: './videochat.component.html',
  styleUrls: ['./videochat.component.css'],
})
export class VideochatComponent implements OnInit {
  @ViewChild("callerVideo") callerVideo: ElementRef = new ElementRef("");
  @ViewChild("myVideo") myVideo: ElementRef = new ElementRef("");
  localPeer: any = null;
  remotePeer: any = null
  localStream: any = null;
  constructor(private dataService: VideochatDataService) { }

  turnOnVideo() {
    navigator.mediaDevices.getUserMedia({
      video: true,
      audio: true
    }).then((stream) => {
      var video = this.myVideo.nativeElement;
      video.muted = true;
      if ('srcObject' in video) {
        video.srcObject = stream
      } else {
        video.src = window.URL.createObjectURL(stream)
      }
      this.localStream = stream;
      //this.localPeer.addStream(stream);
    }).catch((err: any) => { console.log(err) });
  }
  click() {
    this.localPeer = new SimplePeer({ "initiator": true });
    this.localPeer.addStream(this.localStream);
    if (this.dataService.calling != null) {
      //this.dataService.sendRequestToStartChat(this.localPeer, this.dataService.calling);
      this.localPeer.on("signal", (data: any) => {
        this.dataService.hubConnection.invoke("StartCall", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), this.dataService.calling , { Data: data, Dst: "RemotePeer" });
      });
    }
    this.localPeer.on("connect", () => {
      console.log("P2P Connect");
    })
  }
  ngOnInit(): void {
    this.dataService.addConnectionListener("Accept", (caller: string, data: any) => {
      this.remotePeer.on("signal", (data: any) => {
        this.dataService.hubConnection.invoke("StartCall", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), { Email: caller }, { Data: data, Dst: "LocalPeer" });
      });
      this.dataService.calling = { Email: caller };
      //if (this.dataService.calling == null) {
      //  this.dataService.sendRequestToStartChat(this.localPeer, { Email: caller });
      //}
      if (data.Dst == "RemotePeer") {
        this.remotePeer.signal(data.Data);
      } else {
        this.localPeer.signal(data.Data);
      }
    });
    this.dataService.startConnection().then(() => {
      //this.localPeer = new SimplePeer({ "initiator": true });
      this.remotePeer = new SimplePeer();
      //if (this.dataService.calling != null) {
      //  this.dataService.sendRequestToStartChat(this.localPeer, this.dataService.calling);
      //}
        this.remotePeer.on("stream", (stream: any) => {
          console.log(stream);
          var video = this.callerVideo.nativeElement;
          console.log(video);
          if ('srcObject' in video) {
            video.srcObject = stream
          } else {
            video.src = window.URL.createObjectURL(stream)
          }
        });
        this.turnOnVideo();
    });
  }
}
