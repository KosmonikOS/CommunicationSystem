import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
@Injectable()
export class ConfirmationDataService {
  url = "/api/registration";
  constructor(private http: HttpClient) { };
  getConfirmAccount(token: string) {
    return this.http.get(this.url + "/" + token);
  }
  
}
