import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthDataService } from "./auth/auth.data.service";
import { ToastService } from "./toast.service"
import { Account } from "./account/account"
import { AccountDataService } from "./account/account.data.service"
import { Router } from '@angular/router';
import { VideochatDataService } from './videochat/videochat.data.service';
import { AudioService } from "./audio.service"
import { ViewChild } from '@angular/core';
import { ElementRef } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [AuthDataService, ToastService, VideochatDataService, AudioService]
})
export class AppComponent implements OnInit {
  title = 'ClientApp';
  isNavOpen: boolean = false;
  dismissType: boolean = true;
  @ViewChild("callModal") callModal: ElementRef = new ElementRef("");
  constructor(private authDataService: AuthDataService, public toastService: ToastService, public accountDataService: AccountDataService, private router: Router, public videochatDataService: VideochatDataService, private audioService: AudioService, private modalService: NgbModal) { }
  openNav() {
    this.isNavOpen = !this.isNavOpen;
  }
  checkAuth() {
    return this.authDataService.isAuthenticated();
  }
  logOut() {
    this.authDataService.logOut();
  }
  acceptCall() {
    this.router.navigate(["/videochat"]).then(() => {
      this.audioService.stopAudio();
      setTimeout(() => {
        this.videochatDataService.hubConnection.invoke("React", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), !this.videochatDataService.caller.groupId?"AcceptCall":"AddMember", { Email: this.videochatDataService.caller.email});
      }, 1000);
    })
  }
  denyCall() {
    this.videochatDataService.hubConnection.invoke("React", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), "DenyCall", { Email: this.videochatDataService.caller.email});
  }
  ngOnInit(): void {
    this.accountDataService.getAccount(localStorage.getItem("CURRENT_COMMUNICATION_EMAIL") || "");
    if (localStorage.getItem("CURRENT_COMMUNICATION_EMAIL") && window.location.pathname == "/") {
      this.router.navigate(["/messenger"]);
    }
    this.videochatDataService.addConnectionListener("CallRequest", (caller: any,members:any) => {
      this.dismissType = true;
      this.videochatDataService.members = members;
      console.log(members);
      this.videochatDataService.caller = caller;
      this.audioService.startAudio("assets/calling.mp3");
      this.modalService.open(this.callModal, { size: "md" }).result.then(() => { }, () => {
        this.audioService.stopAudio();
        if (this.dismissType) {
          this.denyCall();
        }
      });
    })
    this.videochatDataService.addConnectionListener("AcceptCall", () => {
      this.videochatDataService.callState = false;
      this.audioService.stopAudio();
      this.router.navigate(["/videochat"]);
    })
    this.videochatDataService.addConnectionListener("DenyCall", () => {
      if (this.videochatDataService.calling.email != "Group") {
        this.toastService.showAlert("Ваш звонок отклонен");
        this.videochatDataService.callState = false;
        this.audioService.stopAudio();
      }
    })
    this.videochatDataService.addConnectionListener("ResetCall", () => {
      this.dismissType = false;
      this.toastService.showAlert("Звонок завершен");
      this.audioService.stopAudio();
      this.modalService.dismissAll();
    })
    if (this.checkAuth()) {
      this.videochatDataService.checkConnection();
    }
  }

}
