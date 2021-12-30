export class UserLastMessage {
  constructor(
    public id: number,
    public nickName: string,
    public accountImage: string,
    public email: string,
    public messageId: number,
    public from: number,
    public to: number,
    public content: string,
    public notViewed: number,
    public date?: Date,
    public userActivity?:string
  ) { }
}
