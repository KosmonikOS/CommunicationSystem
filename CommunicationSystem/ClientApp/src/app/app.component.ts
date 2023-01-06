import { Component,OnInit } from '@angular/core';
import { AuthDataService } from "./auth/auth.data.service";
import { ToastService } from "./toast.service"
import { AccountDataService } from "./account/account.data.service"
import { Router } from '@angular/router';
import { VideochatDataService } from './videochat/videochat.data.service';
import { DevicesService } from "./devices.service"
import { ViewChild } from '@angular/core';
import { ElementRef } from '@angular/core';
import { NgbModal} from '@ng-bootstrap/ng-bootstrap';
import { UtilitesService } from "./utilites.service"
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [AuthDataService, ToastService, VideochatDataService, UtilitesService, DevicesService]
})
export class AppComponent {
  title = 'ClientApp';
  isNavOpen: boolean = false;
  dismissType: boolean = true;
  @ViewChild("callModal") callModal: ElementRef = new ElementRef("");
  constructor(private authDataService: AuthDataService, public toastService: ToastService,
    public accountDataService: AccountDataService, private router: Router,
    public videochatDataService: VideochatDataService) { }

  openNav() {
    this.isNavOpen = !this.isNavOpen;
  }
  checkAuth() {
    return this.authDataService.isAuthenticated();
  }
  logOut() {
    this.isNavOpen = false;
    this.authDataService.logOut();
  }
  ngOnInit(): void {
    if (localStorage.getItem("CURRENT_COMMUNICATION_EMAIL") && window.location.pathname == "/") {
      this.router.navigate(["/messenger"]);
    }
    window.addEventListener("beforeunload", () => {
      this.authDataService.setTime(Number(localStorage.getItem("CURRENT_COMMUNICATION_ID")), 1);
    });
    if (this.checkAuth()) {
      this.authDataService.logIn(localStorage.getItem("CURRENT_COMMUNICATION_EMAIL") || "");
    }
  }
}
