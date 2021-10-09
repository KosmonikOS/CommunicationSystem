import { Component, OnDestroy } from '@angular/core';
import { AuthDataService } from "./auth/auth.data.service";
import { ToastService } from "./toast.service"
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [AuthDataService, ToastService]
})
export class AppComponent implements OnDestroy {
  title = 'ClientApp';
  isNavOpen: boolean = false;
  constructor(private authDataService: AuthDataService, public toastService: ToastService) { }
  openNav() {
    this.isNavOpen = !this.isNavOpen;
  }
  checkAuth() {
    return this.authDataService.isAuthenticated();
  }
  logOut() {
    this.authDataService.logOut();
  }
  ngOnDestroy() {
    this.logOut()
  }
}
