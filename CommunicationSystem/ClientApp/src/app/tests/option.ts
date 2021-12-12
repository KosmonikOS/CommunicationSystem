export class Option {
  constructor(
    public id: number = 0,
    public text?: string,
    public isRightOption: boolean = false,
    public isSelected: boolean = false,
  ) { }
}
