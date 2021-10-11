import { Component, OnDestroy } from '@angular/core';
import { AuthDataService } from "./auth/auth.data.service";
import { ToastService } from "./toast.service"
import { Account } from "./account/account"
import { AccountDataService } from "./account/account.data.service"
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [AuthDataService, ToastService, AccountDataService]
})
export class AppComponent implements OnDestroy {
  title = 'ClientApp';
  isNavOpen: boolean = false;
//  currentAccount: Account = this.accountDataService.currenAccount;
  constructor(private authDataService: AuthDataService, public toastService: ToastService, public accountDataService: AccountDataService) { }
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
  ngOnDestroy() {
    this.logOut()
  }
}
