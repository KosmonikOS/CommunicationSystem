
export class MessageBetweenUsers {
  constructor(
    public from: number,
    public to: number,
    public togroup: boolean,
    public content: string,
    public nickName?: string,
    public accountImage?: string,
    public id?: number,
    public date?: Date,
  ) { }
}
