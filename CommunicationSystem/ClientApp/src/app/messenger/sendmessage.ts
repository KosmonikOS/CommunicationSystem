export class SendMessage {
  constructor(
    public from: number,
    public content: string = "",
    public type: number = 0,    
    public to?: number,
    public toGroup: string = "",
    public isGroup?: boolean,
    public id: number = 0,
  ) { }
}
