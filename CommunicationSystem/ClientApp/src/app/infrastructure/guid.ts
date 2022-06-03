export abstract class Guid {
  public static Empty: string = "00000000-0000-0000-0000-000000000000";
  public static IsEmpty(guid: string): boolean {
    return guid == this.Empty;
  }
}
