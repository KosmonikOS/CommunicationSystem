export class MessageBetweenUsers {
  constructor(
    public from: number,
    public to: number = 0,
    public togroup: number = 0,
    public content: string = "",
    public toEmail: string = "",
    public type?: number,
    public nickName?: string,
    public accountImage?: string,
    public id?: number,
    public date?: Date,
    public auto: boolean = false
  ) { }
}
