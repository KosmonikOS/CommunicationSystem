import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { Account } from "./account";
import { tap } from "rxjs/operators";
import { Timestamp } from "rxjs/internal/operators/timestamp";

@Injectable({ providedIn: 'root' })
export class AccountDataService {
  url = "api/account/";
  currentAccount: Account = new Account();
  constructor(private http: HttpClient) { }
  getAccount(email: string) {
    return this.http.get(this.url + email).subscribe((data: any) => this.currentAccount = data);
  }
  putAccount() {
    return this.http.put(this.url, this.currentAccount);
  }
}
