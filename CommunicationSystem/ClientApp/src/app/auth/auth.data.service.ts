import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { tap } from 'rxjs/operators';
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";


@Injectable()
export class AuthDataService {
  url: string = "/api/auth";
  constructor(public http: HttpClient, public jwtHelper: JwtHelperService, public router: Router) { }
  getToken(email: string, password: string) {
    return this.http.post<any>(this.url, { email, password }).pipe(
      tap(token => {
        localStorage.setItem("COMMUNICATION_ACCESS_TOKEN_KEY", token.access_token)
      })
    );
  }
  isAuthenticated() {
    var token = localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY");
    return token && !this.jwtHelper.isTokenExpired(token);
    
  }
  logOut() {
    localStorage.removeItem("COMMUNICATION_ACCESS_TOKEN_KEY");
    this.router.navigate(['']);
  }
  saveUserData(email: string, password: string) {
    localStorage.setItem("COMMUNICATION_EMAIL", email);
    localStorage.setItem("COMMUNICATION_PASSWORD", password);
  }
  deleteUserData() {
    localStorage.removeItem("COMMUNICATION_EMAIL");
    localStorage.removeItem("COMMUNICATION_PASSWORD");
  }
}
