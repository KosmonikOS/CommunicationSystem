import { Question } from "./question"

export class Test {
  constructor(
    public id: number = 0,
    public subject?: number,
    public subjectName?: string,
    public name?: string,
    public grade?: number,
    public questions?: number,
    public time?: number,
    public notFormatedTime?:any,
    public date?: Date,
    public creator?: number,
    public creatorName?: string,
    public students?: number[],
    public questionsList: Question[] = [],
  ) { };
}
