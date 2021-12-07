import { Option } from "./option"

export class Question {
  constructor(
    public id: number = 0,
    public options: Option[] = [],
    public text?: string,
  ) { }
}
