import { Question } from "../tests/question";

export class TestMember {
  constructor(
    public userId: number = 0,
    public testId?: string,
    public name?: string,
    public grade?: string,
    public isSelected?: boolean,
    public isCompleted: boolean = false,
    public mark: number = 0,
    public state: number = 3,
    public answers: Question[] = []
  ) { };
}
