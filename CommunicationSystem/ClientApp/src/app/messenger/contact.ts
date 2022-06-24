import { Guid } from "../infrastructure/guid";

export class Contact {
  constructor(
    public toId?: number,
    public toGroup: string = Guid.Empty,
    public isGroup? :boolean,
    public nickName?: string,
    public accountImage?: string,
    public lastMessage?: string,
    public lastMessageDate?: Date | null,
    public lastMessageType?:number,
    public notViewedMessages: number = 0,
    public newMessages:number = 0,
    public lastActivity?:string
  ) { }
}
