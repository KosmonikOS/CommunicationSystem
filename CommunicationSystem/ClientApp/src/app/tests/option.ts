import { Guid } from "../infrastructure/guid";

export class Option {
  constructor(
    public id: string = Guid.Empty,
    public text?: string,
    public isRightOption: boolean = false,
    public isSelected: boolean = false,
  ) { }
}
