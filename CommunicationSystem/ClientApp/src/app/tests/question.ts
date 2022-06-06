import { Guid } from "../infrastructure/guid"
import { Option } from "./option"
export const QuestionType = [
  { "value": 0, "name": "Выбор 1 правильного варианта" },
  { "value": 1, "name": "Выбор нескольких правильных вариантов" },
  { "value": 2, "name": "Ввод ответа с проверкой" },
  { "value": 3, "name": "С открытым ответом" }
]

export class Question {
  constructor(
    public id: string = Guid.Empty,
    public options: Option[] = [],
    public text?: string,
    public questionType: number = 0,
    public image?: string,
    public points: number = 1,
    public answers: string[] = [],
    public openAnswer: string = "",
  ) { }
}
