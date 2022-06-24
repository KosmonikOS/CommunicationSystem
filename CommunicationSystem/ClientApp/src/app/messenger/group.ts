import { Guid } from "../infrastructure/guid";
import { Member } from "./member";

export class Group {
  constructor(
    public members: Member[],
    public id: string = Guid.Empty,
    public name?: string,
    public groupImage?: string,
  ) { }
}
