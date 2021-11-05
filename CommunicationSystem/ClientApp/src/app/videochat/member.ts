export class Member {
  constructor(
    public localPeer: any = null,
    public remotePeer: any = null,
    public localStream: any = null,
    public remoteStream: any = null,
    public audioState: boolean = true,
    public videoState: boolean = true,
    public accountImage: string = "",
    public nickName:string = ""
  ) { }
}
