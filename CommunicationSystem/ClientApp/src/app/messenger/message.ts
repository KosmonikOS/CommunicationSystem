export class Message {
  constructor(
    public id: number = 0,
    public isMine: boolean = false,
    public toGroup: boolean = false,
    public date?: Date,
    public content: string = "",
    public type?: number,
    public nickName: string = "",
    public accountImage: string = "",    
  ) { }
}
