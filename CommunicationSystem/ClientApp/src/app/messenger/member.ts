export class Member {
  constructor(
    public userId: number,
    public state: number = 3,
    public nickName?: string,
    public accountImage?: string,
    public isSelected?: boolean,
    public isLinked:boolean = false
  ) { }
}
