import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
@Injectable()
export class RecoveryDataService {
  url = "/api/auth/recover";
  constructor(private http: HttpClient) { };
  putRecoverPassword(email: string) {
    return this.http.put(this.url, { "Email": email });
  }
  
}
