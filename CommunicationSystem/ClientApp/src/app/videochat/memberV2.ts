import { MediaConnection } from "peerjs";

export class Member {
  constructor(
    private _connection?: MediaConnection,
    private _myself: boolean = false,
    private _stream: MediaStream = new MediaStream(),
    private _audioState: boolean = false,
    private _videoState: boolean = false,
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
  set stream(value: MediaStream) {
    this._stream = value;
    this.state = !this.state;
  }
  get stream() {
    return this._stream;
  }
  set connection(value: MediaConnection) {
    this._connection = value;
    this.state = !this.state;
  }
  get connection() {
    return this._connection!;
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
