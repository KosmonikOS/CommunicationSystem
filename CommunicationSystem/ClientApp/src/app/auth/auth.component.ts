import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthDataService } from "./auth.data.service"
import { Login } from "./login"
import { ErrorHandler } from "../infrastructure/error.handler"
import { AccountDataService } from '../account/account.data.service';
import { VideochatDataService } from '../videochat/videochat.data.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
  providers: [AuthDataService, ErrorHandler]
})
export class AuthComponent implements OnInit {
  login: Login = new Login(localStorage.getItem("COMMUNICATION_EMAIL") || "", localStorage.getItem("COMMUNICATION_PASSWORD") || "");
  saveToLocalStorage: boolean = false;
  errors: any = {};

  constructor(private dataService: AuthDataService, private router: Router,
    private accountDataService: AccountDataService, private videochatDataService: VideochatDataService, private errorHandler: ErrorHandler) { }
  Enter() {
    this.dataService.getToken(this.login.email, this.login.password).subscribe(result => {
      if (this.saveToLocalStorage) {
        this.dataService.saveUserData(this.login.email, this.login.password);
      } else {
        this.dataService.deleteUserData();
      }
      this.router.navigate(["/messenger"]);
      this.dataService.logIn(this.login.email);
    },
      error => {
        this.errors = this.errorHandler.Handle(error);
      }
    );
  }

  RedirectToRegistration() {
    this.router.navigate(["/registration"]);
  }
  RedirectToRecovery() {
    this.router.navigate(["/recovery"]);
  }
  ngOnInit(): void {
    this.dataService.logOut();
  }
}
