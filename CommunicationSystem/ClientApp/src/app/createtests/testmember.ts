export class TestMember {
  constructor(
    public testId: number,
    public userId: number,
    public name: string,
    public grade: string,
    public isSelected: boolean,
    public isCompleted: boolean = false,
    public mark?: number,
    public id: number = 0,
  ) { };
}
