export class UserLastMessage {
  constructor(
    public id: number,
    public nickName: string,
    public accountImage: string,
    public messageId: number,
    public from: number,
    public to: number,
    public content: string,
    public date: Date
  ) { }
}
