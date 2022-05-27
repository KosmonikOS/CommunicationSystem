import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { AccountDataService } from "../account/account.data.service"

@Injectable()
export class AuthDataService {
  url: string = "/api/auth/";
  currentUserEmail = "";
  refreshTimer: any = null;
  constructor(public http: HttpClient, public jwtHelper: JwtHelperService, public router: Router, private accountDataService: AccountDataService) { }
  getToken(email: string, password: string) {
    return this.http.post<any>(this.url, { email, password }).pipe(
      tap(token => {
        console.log(token);
        localStorage.setItem("COMMUNICATION_ACCESS_TOKEN_KEY", token.accessToken);
        localStorage.setItem("COMMUNICATION_REFRESH_TOKEN", token.refreshToken);
        localStorage.setItem("CURRENT_COMMUNICATION_EMAIL", email);
        localStorage.setItem("CURRENT_COMMUNICATION_ID", token.currentAccountId);
        this.currentUserEmail = email;
      })
    );
  }
  setRefreshTimer() {
    var time = JSON.parse(atob(localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY")!.split('.')[1])).exp * 1000 - new Date().getTime() - 60000;
    if (time > 0) {
      this.refreshTimer = setTimeout(() => {
        this.http.post<any>(this.url + "refresh", {
          "AccessToken": localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY"),
          "RefreshToken": localStorage.getItem("COMMUNICATION_REFRESH_TOKEN")
        }).subscribe(
          token => {
            localStorage.setItem("COMMUNICATION_ACCESS_TOKEN_KEY", token.accessToken);
            localStorage.setItem("COMMUNICATION_REFRESH_TOKEN", token.refreshToken);
            this.setRefreshTimer();
          })
      }, time);
    }
  }
  setTime(id: number, action: number) {
    if (this.isAuthenticated()) {
      return this.http.put(this.url + "settime", {
        "Id": id,
        "Action": action
      }).subscribe(() => { });
    }
  }
  isAuthenticated() {
    var token = localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY") || "";
    var lifeTime = this.jwtHelper.isTokenExpired(token);
    if (token && lifeTime) {
      this.router.navigate([""]);
    }
    return token && !lifeTime;

  }
  logIn(email: string) {
    this.setRefreshTimer();
    this.setTime(Number(localStorage.getItem("CURRENT_COMMUNICATION_ID")), 0);
    this.accountDataService.getAccount(email);
  }
  logOut() {
    if (this.isAuthenticated()) {
      this.setTime(Number(localStorage.getItem("CURRENT_COMMUNICATION_ID")), 1);
      clearTimeout(this.refreshTimer);
    }
    localStorage.removeItem("COMMUNICATION_ACCESS_TOKEN_KEY");
    localStorage.removeItem("CURRENT_COMMUNICATION_EMAIL");
    localStorage.removeItem("CURRENT_COMMUNICATION_ID");
    localStorage.removeItem("COMMUNICATION_REFRESH_TOKEN");
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
