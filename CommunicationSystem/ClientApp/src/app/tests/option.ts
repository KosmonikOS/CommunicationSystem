export class Option {
  constructor(
    public id: string = '',
    public text?: string,
    public isRightOption: boolean = false,
    public isSelected: boolean = false,
  ) { }
}
