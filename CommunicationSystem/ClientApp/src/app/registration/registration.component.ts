import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Registration } from './registration';
import { RegistrationDataService } from "./registration.data.service"
import { Router } from '@angular/router';
import { ToastService } from "../toast.service"
import { ErrorHandler } from "../infrastructure/error.handler"
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  providers: [RegistrationDataService, ErrorHandler]
})
export class RegistrationComponent {
  registration: Registration = new Registration();
  errors: any = {};
  timer: number = 60;
  @ViewChild("confirmationModal") confirmationModal: ElementRef = new ElementRef("");
  constructor(private dataService: RegistrationDataService, private router: Router
    , private toastService: ToastService, private modalService: NgbModal
    , private errorHandler: ErrorHandler) { };
  Register() {
    this.dataService.postRegistration(this.registration).subscribe(
      result => {
        this.StartCountDown()
        this.modalService.open(this.confirmationModal, { size: "md",centered: true  });
        this.router.navigate([""])
      },
      error => {
        this.errors = this.errorHandler.Handle(error);
      }
    )
  }
  ResendConfirmation() {
    this.dataService.getSendConfirmation(this.registration.email).subscribe(
      result => {
        this.StartCountDown();
        this.toastService.showSuccess("Письмо отправленно");
      },
      error => {
        this.errors = this.errorHandler.Handle(error);
      });
  }
  StartCountDown() {
    this.timer = 60;
    var interval = setInterval(() => {
      if (this.timer <= 0) {
        clearInterval(interval);
      }
      this.timer -= 1;
    }, 1000);
  }
  redirectToAuth() {
    this.router.navigate([""]);
  }
}
