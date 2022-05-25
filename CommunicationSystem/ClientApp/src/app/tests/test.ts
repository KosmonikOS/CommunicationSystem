import { TestMember } from "../createtests/testmember";
import { Question } from "./question"

export class Test {
  constructor(
    public id: string = '',
    public subject?: number,
    public subjectName?: string,
    public name?: string,
    public grade?: number,
    public questions?: number,
    public time: number = 40,
    public date?: Date,
    public creator?: number,
    public creatorName?: string,
    public students: TestMember[] = [],
    public questionsList: Question[] = [],
  ) { };
}
