import { TestMember } from "../createtests/testmember";
import { Guid } from "../infrastructure/guid";
import { Question } from "./question"

export class Test {
  constructor(
    public id: string = Guid.Empty,
    public subject: number = 1,
    public subjectName?: string,
    public name?: string,
    public grade?: number,
    public questionsQuantity?: number,
    public time: number = 40,
    public date?: Date,
    public creator?: number,
    public creatorName?: string,
    public students: TestMember[] = [],
    public questions: Question[] = [],
  ) { };
}
