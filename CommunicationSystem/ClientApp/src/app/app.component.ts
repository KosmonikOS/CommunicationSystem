import { Component,OnInit } from '@angular/core';
import { AuthDataService } from "./auth/auth.data.service";
import { ToastService } from "./toast.service"
import { AccountDataService } from "./account/account.data.service"
import { Router } from '@angular/router';
import { VideochatDataService } from './videochat/videochat.data.service';
import { AudioService } from "./audio.service"
import { DevicesService } from "./devices.service"
import { ViewChild } from '@angular/core';
import { ElementRef } from '@angular/core';
import { NgbModal} from '@ng-bootstrap/ng-bootstrap';
import { UtilitesService } from "./utilites.service"
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [AuthDataService, ToastService, VideochatDataService, AudioService, UtilitesService, DevicesService]
})
export class AppComponent implements OnInit {
  title = 'ClientApp';
  isNavOpen: boolean = false;
  dismissType: boolean = true;
  @ViewChild("callModal") callModal: ElementRef = new ElementRef("");
  constructor(private authDataService: AuthDataService, public toastService: ToastService, public accountDataService: AccountDataService, private router: Router, public videochatDataService: VideochatDataService, private audioService: AudioService, private modalService: NgbModal, private utilitesService: UtilitesService, private devicesService: DevicesService) { }

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
  acceptCall() {
    this.devicesService.checkDevices().then(() => {
      this.router.navigate(["/videochat"]).then(() => {
        this.audioService.stopAudio();
        setTimeout(() => {
          this.videochatDataService.hubConnection.invoke("React", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), !this.videochatDataService.caller.groupId ? "AcceptCall" : "AddMember", { Email: this.videochatDataService.caller.email });
        }, 500);
      })
    });
  }
  denyCall() {
    this.videochatDataService.hubConnection.invoke("React", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), "DenyCall", { Email: this.videochatDataService.caller.email});
  }
  ngOnInit(): void {
    if (localStorage.getItem("CURRENT_COMMUNICATION_EMAIL") && window.location.pathname == "/") {
      this.router.navigate(["/messenger"]);
    }
    window.addEventListener("beforeunload", () => {
      //this.logOut();
      this.authDataService.setTime(Number(localStorage.getItem("CURRENT_COMMUNICATION_ID")), 1);
    });
    this.videochatDataService.addConnectionListener("CallRequest", (caller: any,members:any) => {
      this.dismissType = true;
      this.videochatDataService.members = members;
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
      this.authDataService.logIn(localStorage.getItem("CURRENT_COMMUNICATION_EMAIL") || "");
      this.videochatDataService.checkConnection();
    }
  }
}
