import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";

@Injectable()
export class AuthDataService {
  url: string = "/api/auth/";
  currentUserEmail = "";
  refreshTimer: any = null;
  constructor(public http: HttpClient, public jwtHelper: JwtHelperService, public router: Router) { }
  getToken(email: string, password: string) {
    return this.http.post<any>(this.url, { email, password }).pipe(
      tap(token => {
        localStorage.setItem("COMMUNICATION_ACCESS_TOKEN_KEY", token.access_token);
        localStorage.setItem("COMMUNICATION_REFRESH_TOKEN", token.refresh_token);
        localStorage.setItem("CURRENT_COMMUNICATION_EMAIL", email);
        localStorage.setItem("CURRENT_COMMUNICATION_ID", token.current_account_id);
        this.currentUserEmail = email;
        this.setRefreshTimer();
      })
    );
  }
  setRefreshTimer() {
    var time = JSON.parse(atob(localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY")!.split('.')[1])).exp * 1000 - new Date().getTime() - 60000;
    this.refreshTimer = setTimeout(() => {
      this.http.post<any>(this.url + "refresh", { "JWT": localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY"), "RT": localStorage.getItem("COMMUNICATION_REFRESH_TOKEN") }).subscribe((token: any) => {
        localStorage.setItem("COMMUNICATION_ACCESS_TOKEN_KEY", token.access_token);
        localStorage.setItem("COMMUNICATION_REFRESH_TOKEN", token.refresh_token);
        this.setRefreshTimer();
      })
    }, time);
  }
  setTime(id: number, action: string) {
    return this.http.get(this.url + "settime/" + id + "/" + action).subscribe(() => { });
  }
  isAuthenticated() {
    var token = localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY") || "";
    var lifeTime = this.jwtHelper.isTokenExpired(token);
    if (token && lifeTime) {
      this.router.navigate([""]);
    }
    return token && !lifeTime;
    
  }
  logOut() {
    this.setTime(Number(localStorage.getItem("CURRENT_COMMUNICATION_ID")), "leave");
    clearTimeout(this.refreshTimer);
    localStorage.removeItem("COMMUNICATION_ACCESS_TOKEN_KEY");
    localStorage.removeItem("CURRENT_COMMUNICATION_EMAIL");
    localStorage.removeItem("CURRENT_COMMUNICATION_ID");
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
