export class TestMember {
  constructor(
    public testId?: string,
    public userId: number = 0,
    public name?: string,
    public grade?: string,
    public isSelected?: boolean,
    public isCompleted: boolean = false,
    public mark: number = 0,
    public id: number = 0,
  ) { };
}
