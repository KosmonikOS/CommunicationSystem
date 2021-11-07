import { ElementRef, ViewChild } from "@angular/core";

export class Member {
  constructor(
    private _myself: boolean = false,
    private _localPeer: any = null,
    private _remotePeer: any = null,
    private _localStream: any = null,
    private _remoteStream: any = null,
    private _audioState: boolean = true,
    private _videoState: boolean = true,
    private _accountImage: string = "",
    private _nickName: string = "",
    public state: boolean = false,
  ) { }
  set myself(value: boolean) {
    this._myself = value;
    this.state = !this.state;
  }
  get myself() {
    return this._myself;
  }
  set localPeer(value: any) {
    this._localPeer = value;
    this.state = !this.state;
  }
  get localPeer() {
    return this._localPeer;
  }
  set remotePeer(value: any) {
    this._remotePeer = value;
    this.state = !this.state;
  }
  get remotePeer() {
    return this._remotePeer;
  }
  set localStream(value: any) {
    this._localStream = value;
    this.state = !this.state;
  }
  get localStream() {
    return this._localStream;
  }
  set remoteStream(value: any) {
    this._remoteStream = value;
    this.state = !this.state;
  }
  get remoteStream() {
    return this._remoteStream;
  }
  set audioState(value: boolean) {
    this._audioState = value;
    this.state = !this.state;
  }
  get audioState() {
    return this._audioState;
  }
  set videoState(value: boolean) {
    this._videoState = value;
    this.state = !this.state;
  }
  get videoState() {
    return this._videoState;
  }
  set accountImage(value: string) {
    this._accountImage = value;
    this.state = !this.state;
  }
  get accountImage() {
    return this._accountImage;
  }
  set nickName(value: string) {
    this._nickName = value;
    this.state = !this.state;
  }
  get nickName() {
    return this._nickName;
  }

}
