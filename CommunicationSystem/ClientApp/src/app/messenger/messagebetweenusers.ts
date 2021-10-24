
export class MessageBetweenUsers {
  constructor(
    public from: number,
    public to: number,
    public togroup: number,
    public content: string,
    public toEmail: string,
    public type?: number,
    public nickName?: string,
    public accountImage?: string,
    public id?: number,
    public date?: Date,
  ) { }
}
