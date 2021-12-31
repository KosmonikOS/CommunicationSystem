import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthDataService } from "./auth.data.service"
import { Login } from "./login"
import { ToastService } from "../toast.service"
import { AccountDataService } from '../account/account.data.service';
import { VideochatDataService } from '../videochat/videochat.data.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
  providers: [AuthDataService]
})
export class AuthComponent implements OnInit {
  login: Login = new Login(localStorage.getItem("COMMUNICATION_EMAIL") || "", localStorage.getItem("COMMUNICATION_PASSWORD") || "");
  saveToLocalStorage: boolean = false;
  errors: any = {};

  constructor(private dataService: AuthDataService, private router: Router, private toastService: ToastService, private accountDataService: AccountDataService, private videochatDataService: VideochatDataService) { }
  enter() {
    this.dataService.getToken(this.login.email, this.login.password).subscribe(result => {
      if (this.saveToLocalStorage) {
        this.dataService.saveUserData(this.login.email, this.login.password);
      } else {
        this.dataService.deleteUserData();
      }
      this.videochatDataService.checkConnection();
      this.router.navigate(["/messenger"]);
      this.dataService.logIn(this.login.email);
    },
      error => {
        if (error.error.status == 401) {
          this.toastService.showError("Не верные данные");
        }
        this.errors = error.error.errors;
      }
    );
  }
 
  redirectToRegistration() {
    this.router.navigate(["/registration"]);
  }
  ngOnInit(): void {
    this.dataService.logOut();
  }
}
