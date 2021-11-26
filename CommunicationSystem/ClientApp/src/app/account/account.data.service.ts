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
  postAccount() {
    return this.http.post(this.url, this.currentAccount);
  }
  putImage(image: File, id: any) {
    var formData = new FormData();
    formData.append("ImageToSave", image);
    return this.http.put(this.url + id, formData);
  }
}
