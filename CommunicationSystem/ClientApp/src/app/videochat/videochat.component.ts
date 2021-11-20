import { HostListener, OnDestroy, QueryList } from '@angular/core';
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
  @ViewChild("screenVideo") screenVideo: ElementRef = new ElementRef("");
  screenStream: MediaStream = new MediaStream();
  mediaConfig: any = { video: true, audio: true };
  screenState: boolean = false;
  chatRoom: { [key: string]: Member } = {
    "myself": new Member(true, null, null, null, null, true, true, this.accountDataService.currentAccount.accountImage, this.accountDataService.currentAccount.nickName)
  };
  currentSize: any = {
    height: 0,
    width: 0
  };
  currentQuantity: number = 1;
  constructor(private dataService: VideochatDataService, public accountDataService: AccountDataService, public router: Router, private toastService: ToastService, private audioService: AudioService) { }
  getConnectedMembers() {
    var members = Object.keys(this.chatRoom);
    return members.filter((key) => key != "myself" && this.chatRoom[key]["remoteStream"] != null);
  }
  toggleState(type: string) {
    return new Promise(async (resolve, reject) => {
      switch (type) {
        case "audioState":
          this.chatRoom["myself"]["audioState"] = !this.chatRoom["myself"]["audioState"];
          break;
        case "videoState":
          this.chatRoom["myself"]["videoState"] = !this.chatRoom["myself"]["videoState"];
          break;
        case "screenState":
          await this.toggleScreen();
          this.calculateSize();
          break;
      }
      this.getConnectedMembers().forEach((member) => {
        this.dataService.hubConnection.invoke("ToggleState", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), { "Email": member }, type);
      })
      resolve("");
    })
  }
  async toggleScreen() {
    return new Promise((resolve, reject) => {
      var video = this.screenVideo.nativeElement;
      if (this.screenState) {
        video.srcObject = null
        this.screenStream?.getTracks()?.forEach((track: any) => {
          this.getConnectedMembers().forEach((member: string) => {
            this.chatRoom[member]["localPeer"]?.removeTrack(track, this.chatRoom["myself"]?.remoteStream);
          })
          track?.stop();
          this.screenStream = new MediaStream();
        });
        this.screenState = false
        resolve("");
      } else {
        video.muted = true;
        //@ts-ignore
        navigator.mediaDevices.getDisplayMedia({ video: true, audio: true }).then((stream: any) => {
          this.screenStream = stream;
          this.getConnectedMembers().forEach((member: string) => {
            stream.getTracks()?.forEach((track: any) => {
              this.chatRoom[member]["localPeer"]?.addTrack(track, this.chatRoom["myself"]?.remoteStream);
            })
          });
          video.srcObject = stream;
          this.screenState = true;
          resolve("");
        });
      };
    })
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
      if (this.chatRoom["myself"]?.remoteStream == null) {
        this.mediaConfig = await this.checkMedia();
        navigator.mediaDevices.getUserMedia(this.mediaConfig).then((stream) => {
          this.chatRoom["myself"]["remoteStream"] = stream
          resolve("");
        }).catch(() => {});
      } else {
        resolve("");
      }
    });
  }
  createInitiatorPeer(email: string) {
    var peer = new SimplePeer({ "initiator": true });
    this.CheckVideo().then(() => {
      peer.addStream(this.chatRoom["myself"].remoteStream);
    })
    peer.on("error", (error: any) => { });
    peer.on("connect", () => {
      peer.send(this.accountDataService.currentAccount.accountImage + "@img@" + this.accountDataService.currentAccount.nickName);
    })
    peer.on("signal", (data: any) => {
      this.dataService.hubConnection.invoke("StartCall", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), { Email: email }, { Data: data, Dst: "RemotePeer" });
    });
    return peer;
  }
  createRemotePeer(email: string) {
    var peer = new SimplePeer({ initiator: false });
    peer.on("error", (error: any) => { });
    peer.on("signal", (data: any) => {
      this.dataService.hubConnection.invoke("StartCall", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), { Email: email }, { Data: data, Dst: "LocalPeer" });
    });
    peer.on("connect", () => {
      this.currentQuantity++;
      this.calculateSize();
      if (this.dataService.calling["Caller"] == false) {
        this.CheckVideo().then(() => {
          this.chatRoom[email]["localPeer"] = this.createInitiatorPeer(email);
        })
      }
    });
    peer.on("stream", (stream: any) => {
      this.chatRoom[email]["remoteStream"] = stream
    });
    peer.on("data", (data: any) => {
      var decoded = new TextDecoder().decode(data).split("@img@");
      this.chatRoom[email]["accountImage"] = decoded[0];
      this.chatRoom[email]["nickName"] = decoded[1];
    })
    return peer;
  }
  @HostListener('window:resize', ['$event'])
  calculateSize() {
    if (!this.screenState) {
      var length = this.currentQuantity;
      if (window.innerWidth >= 576) {
        this.currentSize = {
          width: length <= 4 ? 6 : length <= 9 ? 4 : length <= 16 ? 3 : 2,
          height: length <= 2 ? 80 : length <= 6 ? 40 : length <= 9 ? 27 : length <= 16 ? 20 : 16
        };
      } else {
        this.currentSize = {
          width: length <= 2 ? 12 : 6,
          height: 40
        };
      }
    } else {
      this.currentSize.height = 10;
      this.currentSize.width = window.innerWidth >= 576 ? 12 : 4;
    }
  }
  destroyPeers() {
    return new Promise((resolve, reject) => {
      Object.keys(this.chatRoom).forEach((key) => {
        this.chatRoom[key]?.localPeer?.destroy();
        this.chatRoom[key]?.remotePeer?.destroy();
        this.chatRoom[key]?.remoteStream?.getTracks()?.forEach(function (track: any) {
          track?.stop();
        });
        this.chatRoom[key]?.localStream?.getTracks()?.forEach(function (track: any) {
          track?.stop();
        });
      })
      this.dataService.closeConnection();
      this.screenStream?.getTracks()?.forEach(function (track: any) {
        track?.stop();
      });
      this.dataService.calling = null;
      this.dataService.callState = false;
      this.chatRoom = {};
      this.dataService.members = [];
      resolve("");
    });
  };
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
    this.dataService.addConnectionListener("ToggleState", async (caller: string, type: string) => {
      switch (type) {
        case "audioState":
          this.chatRoom[caller]["audioState"] = !this.chatRoom[caller]["audioState"];
          break;
        case "videoState":
          this.chatRoom[caller]["videoState"] = !this.chatRoom[caller]["videoState"];
          break;
        case "screenState":
          var video = this.screenVideo.nativeElement;
          video.muted = false;
          this.screenState = !this.screenState;
          if (this.screenState) {
            this.currentSize.height = 10;
            this.currentSize.width = window.innerWidth >= 576 ? 12 : 4;
          } else {
            this.calculateSize();
          }
          if (this.screenState) {
            setTimeout(async () => {
              var tracks = await this.chatRoom[caller]?.remoteStream?.getTracks();
              if (tracks != undefined && tracks[3] != undefined) {
                if (tracks[1].kind == "audio") {
                  this.screenStream.addTrack(tracks[1]);
                } else {
                  this.screenStream.addTrack(tracks[2]);
                }
                this.screenStream.addTrack(tracks[3]);
              } else {
                this.screenStream.addTrack(tracks[2]);
              }
              video.srcObject = this.screenStream;
            }, 1000);
          } else {
            video.srcObject = null;
            this.screenStream = new MediaStream();
          }
          break;
        case "disconnect":
          if (this.chatRoom[caller].remoteStream.getTracks().length > 2) {
            this.screenState = false;
            this.screenStream = new MediaStream();
          }
          this.chatRoom[caller] = new Member;
          this.currentQuantity--;
          this.calculateSize();
          break;
        case "disconnectAll":
          this.destroyPeers().then(() => {
            this.router.navigate(["/messenger"]);
            this.toastService.showAlert("Звонок завершен");
          });
          break;
      }
    });
    this.dataService.addConnectionListener("AddMember", (email: string) => {
      var members = Object.keys(this.chatRoom);
      members.shift();
      this.dataService.hubConnection.invoke("NeedToConnect", email, members).then(() => {
        this.createChatMember(email);
      })
    });
    this.dataService.addConnectionListener("OfferToConnect", (members: string[]) => {
      setTimeout(() => {
        members.forEach((email) => {
          if (this.chatRoom[email]?.localPeer == null) {
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
    this.chatRoom[email] = new Member(false, localPeer, remotePeer, this.chatRoom["myself"]?.remoteStream);
  }
  createRemotePeers() {
    this.dataService.members.filter((member) => member.email != localStorage.getItem("CURRENT_COMMUNICATION_EMAIL")).forEach((member) => {
      var email = member.email;
      var remotePeer = this.createRemotePeer(email);
      this.chatRoom[email] = new Member(false, null, remotePeer);
    })
  }
  ngOnInit(): void {
    this.subscribeHubEvents();
    if (this.dataService.calling == null) {
      this.createRemotePeers();
    }
    this.CheckVideo().then(() => {
      this.dataService.checkConnection()
      if (this.dataService.calling != null && this.dataService.calling.email != "Group") {
        this.createChatMember(this.dataService.calling.email);
      }
    });
    this.calculateSize();
  }
  ngOnDestroy(): void {
    var dcst = Object.keys(this.chatRoom).length > 2 || this.dataService?.calling?.email == "Group" ? "disconnect" : "disconnectAll";
    this.toggleState(dcst).then(() => {
      this.destroyPeers();
    });
  }
}
