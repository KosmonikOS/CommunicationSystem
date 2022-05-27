import { Component } from '@angular/core';
import { RecoveryDataService } from "./recovery.data.service"
import { Router } from '@angular/router';
import { ErrorHandler } from "../infrastructure/error.handler"
import { ToastService } from '../toast.service';
@Component({
  selector: 'app-recovery',
  templateUrl: './recovery.component.html',
  styleUrls: ['./recovery.component.css'],
  providers: [RecoveryDataService, ErrorHandler]
})
export class RecoveryComponent {
  constructor(private dataService: RecoveryDataService, private errorHandler: ErrorHandler,
    private toastService: ToastService, private router: Router) { }
  errors: any = {};
  email: string = "";
  RecoverPassword() {
    this.dataService.putRecoverPassword(this.email).subscribe(
      result => {
        this.toastService.showSuccess("Письмо отправленно");
        this.RedirectToAuth();
      },
      error => {
        this.errors = this.errorHandler.Handle(error);
      })
  }
  RedirectToAuth() {
    this.router.navigate([""]);
  }
}
