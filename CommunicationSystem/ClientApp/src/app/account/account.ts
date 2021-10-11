export class Account {
  constructor(
    public id?: number,
    public password?: string,
    public email?: string,
    public nickName?: string,
    public firstName?: string,
    public middleName?: string,
    public lastName?: string,
    public role?: number,
    public accountImage?: string,
    public phone?: string
  ) { }
}
