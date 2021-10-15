import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthDataService } from "./auth/auth.data.service";
import { ToastService } from "./toast.service"
import { Account } from "./account/account"
import { AccountDataService } from "./account/account.data.service"
import { Router } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [AuthDataService, ToastService, AccountDataService]
})
export class AppComponent implements OnInit {
  title = 'ClientApp';
  isNavOpen: boolean = false;
  //  currentAccount: Account = this.accountDataService.currenAccount;
  constructor(private authDataService: AuthDataService, public toastService: ToastService, public accountDataService: AccountDataService, private router: Router) { }
  openNav() {
    this.isNavOpen = !this.isNavOpen;
  }
  checkAuth() {
    return this.authDataService.isAuthenticated();
  }
  logOut() {
    this.authDataService.logOut();
    //console.log(this.currentAccount);
  }
  ngOnInit(): void {
    this.accountDataService.getAccount(localStorage.getItem("CURRENT_COMMUNICATION_EMAIL") || "");
    if (localStorage.getItem("CURRENT_COMMUNICATION_EMAIL") && window.location.pathname == "/") {
      this.router.navigate(["/messenger"]);
    }

  }

}
