import { Component, ElementRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { environment } from '../../environments/environment.prod';
import { ActivatedRoute, Router } from '@angular/router';
import Peer, { DataConnection, MediaConnection } from 'peerjs';
import { DevicesService } from '../devices.service';
import { ToastService } from '../toast.service';
import { VideochatDataService } from './videochat.data.service';
import { AccountDataService } from "../account/account.data.service"
import { Member } from './memberV2';
import { State } from './state';
@Component({
  selector: 'app-videochat',
  templateUrl: './videochat.component.html',
  styleUrls: ['./videochat.component.css'],
})
export class VideochatComponent implements OnInit, OnDestroy {
  constructor(private activateRoute: ActivatedRoute, private dataService: VideochatDataService,
    private deviceService: DevicesService, private toastService: ToastService,
    private router: Router, private accountDataService: AccountDataService) { }
  @ViewChild("videoArea") videoArea: ElementRef = new ElementRef("");
  @ViewChild("screenVideo") screenVideo: ElementRef = new ElementRef("");
  roomId: string = "";
  myVideo = document.createElement("video");
  peers: { [key: string]: Member } = {};
  myPeer: Peer = new Peer({
    host: environment.peerServer,
    port: environment.peerServerPort,
    secure: true
  });
  currentSize: any = {
    height: 0,
    width: 0
  };
  showControls: boolean = true;
  screenState: boolean = false;
  screenInitiator: boolean = false;
  screenStream: MediaStream = new MediaStream();
  GetPeers() {
    return Object.values(this.peers);
  }
  ConnectToRoom(roomId: string, peerId: string) {
    this.dataService.connectToRoom(roomId, peerId);
  }
  async GetUserVideo() {
    var mediaConfig = await this.deviceService.checkMedia();
    navigator.mediaDevices.getUserMedia(mediaConfig).then(stream => {
      var myVideo = new Member();
      myVideo.myself = true;
      myVideo.accountImage = this.accountDataService.currentAccount.accountImage!;
      myVideo.nickName = this.accountDataService.currentAccount.nickName!;
      myVideo.audioState = mediaConfig.audio;
      myVideo.videoState = mediaConfig.video;
      myVideo.stream = stream;
      this.peers["myself"] = myVideo;
      this.CalculateSize();
      this.AddStreamListeners(stream);
    });
  }
  AddStreamListeners(stream: MediaStream) {
    this.myPeer.on("call", call => {
      if (!(call.peer in this.peers)) {
        call.answer(stream);
        this.SubscribeVideoCall(call);
      } else {
        call.answer();
        this.SubscribeScreenStream(call);
      }
    })
    this.dataService.addConnectionListener("UserConnected", (peerId: string) => {
      this.ConnectToNewUser(peerId, stream);
    })
  }
  SubscribeScreenStream(call: MediaConnection) {
    call.on("stream", screenStream => {
      if (!this.screenState) {
        this.screenStream = screenStream;
        this.screenVideo.nativeElement.srcObject = screenStream;
        this.screenState = true;
        this.screenInitiator = false;
        this.CalculateSize();
      }
    });
  }
  CopyAccessCode() {
    navigator.clipboard.writeText(this.roomId);
    this.toastService.showSuccess("Код доступа скопирован");
  }
  SubscribeVideoCall(call: MediaConnection) {
    call.on("stream", userVideoStream => {
      this.peers[call.peer].stream = userVideoStream;
    });
    call.on("close", () => {
      delete this.peers[call.peer];
      this.CalculateSize();
    })
    this.peers[call.peer] = new Member(call);
    this.CalculateSize();
  }
  AddScreenInitiatorListeners(call: MediaConnection) {
    call.on("stream", () => {
      this.myPeer.call(call.peer, this.screenStream);
    })
  }
  AddListeners() {
    this.myPeer.on("connection", conn => {
      this.AddDataListeners(conn);
    })
    this.screenVideo.nativeElement.addEventListener("loadedmetadata", () => {
      this.screenVideo.nativeElement.play()
    })
    this.dataService.addConnectionListener("UserDisconnected", (peerId: string) => {
      if (this.peers[peerId]) {
        this.peers[peerId].connection.close();
        delete this.peers[peerId];
      }
    })
    this.dataService.addConnectionListener("StateToggled", (peerId: string,
      type: State, value: boolean) => {
      switch (type) {
        case State.Video:
          this.peers[peerId].videoState = value;
          break;
        case State.Audio:
          this.peers[peerId].audioState = value;
          break;
        case State.Screen:
          this.screenStream.getTracks().forEach(track => {
            track.stop();
          });
          this.screenStream = new MediaStream();
          this.screenVideo.nativeElement.srcObject = null;
          this.screenState = false;
          this.screenInitiator = false;
          this.CalculateSize();
          break;
      }
    })
  }
  AddDataListeners(connection: DataConnection) {
    connection.on("data", (data: any) => {
      try {
        this.peers[connection.peer].accountImage = data.accountImage;
        this.peers[connection.peer].nickName = data.nickName;
        this.peers[connection.peer].audioState = data.audioState;
        this.peers[connection.peer].videoState = data.videoState;
      } catch { }
    })
    connection.on("open", () => {
      var data = {
        "nickName": this.accountDataService.currentAccount.nickName,
        "accountImage": this.accountDataService.currentAccount.accountImage,
        "videoState": this.peers["myself"].videoState,
        "audioState": this.peers["myself"].audioState,
      };
      connection.send(data);
    })
  }
  ConnectToNewUser(peerId: string, stream: any) {
    var call = this.myPeer.call(peerId, stream);
    var conn = this.myPeer.connect(peerId);
    this.AddDataListeners(conn);
    this.SubscribeVideoCall(call);
    if (this.screenInitiator && this.screenState)
      this.AddScreenInitiatorListeners(call);
  }
  Leave() {
    this.router.navigate(['/messenger']);
  }
  @HostListener('window:resize', ['$event'])
  CalculateSize() {
    if (!this.screenState) {
      var length = Object.keys(this.peers).length;
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
  ToggleVideo() {
    this.peers["myself"].videoState = !this.peers["myself"].videoState;
    this.peers["myself"].stream.getVideoTracks()[0].enabled = this.peers["myself"].videoState;
    this.dataService.toggelState(this.roomId, this.myPeer.id, State.Video,
      this.peers["myself"].videoState);
  }
  ToggleAudio() {
    this.peers["myself"].audioState = !this.peers["myself"].audioState;
    this.peers["myself"].stream.getAudioTracks()[0].enabled = this.peers["myself"].audioState;
    this.dataService.toggelState(this.roomId, this.myPeer.id, State.Audio,
      this.peers["myself"].audioState);
  }
  ToggleScreen() {
    return new Promise((resolve, reject) => {
      var video = this.screenVideo.nativeElement;
      if (this.screenState) {
        this.screenStream.getTracks().forEach(track => {
          track.stop();
        });
        video.srcObject = null;
        this.screenState = false;
        this.screenInitiator = false;
        this.dataService.toggelState(this.roomId, this.myPeer.id, State.Screen, false).then(() => {
          this.CalculateSize();
          resolve("");
        });
      } else {
        video.muted = true;
        //@ts-ignore
        navigator.mediaDevices.getDisplayMedia({ video: true, audio: true }).then(stream => {
          this.screenStream = stream;
          this.GetPeers().filter(x => !x.myself).forEach(x => {
            this.myPeer.call(x.connection.peer, stream);
          })
          video.srcObject = stream;
          this.screenState = true;
          this.screenInitiator = true;
          this.CalculateSize();
          resolve("");
        });
      }
    })
  }
  TurnOnScreen() {

  }
  TurnOffScreen() {

  }
  ngOnInit(): void {
    this.dataService.startConnection().then(async () => {
      await this.GetUserVideo();
      this.myPeer.on("open", peerId => {
        this.AddListeners();
        this.roomId = this.activateRoute.snapshot.params["roomId"];
        this.ConnectToRoom(this.roomId, peerId);
      })
    }).catch(error => {
      this.toastService.showError("Что-то пошло не так");
      this.router.navigate(["/messenger"]);
    })
  }
  ngOnDestroy(): void {
    this.dataService.disconnectFromRoom(this.roomId, this.myPeer.id).then(async () => {
      try {
        if (this.screenInitiator && this.screenState)
          await this.ToggleScreen();
        this.screenStream.getTracks().forEach(track => {
          track.stop();
        });
        this.peers["myself"].stream.getTracks().forEach(track => {
          track.stop();
        })
        this.myPeer.disconnect();
        this.dataService.closeConnection();
      } catch { document.location.reload(); }
    });
  }
}
