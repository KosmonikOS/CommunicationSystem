import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { AuthDataService } from "../auth/auth.data.service"
@Injectable()
export class RegistrationDataService {
  url = "/api/registration";
  constructor(private http: HttpClient, private authDataSerive: AuthDataService) { };
  postRegistration(registration: any) {
    return this.http.post(this.url, registration);
  }
}
