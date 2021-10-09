import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthDataService } from "./auth.data.service"
import { Login } from "./login"
import { ToastService } from "../toast.service"

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
  providers: [AuthDataService]
})
export class AuthComponent {
  login: Login = new Login(localStorage.getItem("COMMUNICATION_EMAIL") || "", localStorage.getItem("COMMUNICATION_PASSWORD") || "");
  saveToLocalStorage: boolean = false;
  errors: any = {};

  constructor(private dataService: AuthDataService, private router: Router, private toastService: ToastService) { }
  enter() {
    this.dataService.getToken(this.login.email, this.login.password).subscribe(result => {
      if (this.saveToLocalStorage) {
        this.dataService.saveUserData(this.login.email, this.login.password);
      } else {
        this.dataService.deleteUserData();
      }
      this.router.navigate([""]);
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
}
