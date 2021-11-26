export class Account {
  constructor(
    public id: number = 0,
    public password?: string,
    public email?: string,
    public nickName?: string,
    public firstName?: string,
    public middleName?: string,
    public lastName?: string,
    public grade?: number,
    public gradeLetter?:string,
    public role: number = 1,
    public roleName?: string,
    public accountImage: string = "assets/user.png",
    public phone?: string
  ) { }
}
