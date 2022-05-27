import { Component, ElementRef, ViewChild } from '@angular/core';
import { Account } from './account';
import { AccountDataService } from './account.data.service';
import { ToastService } from "../toast.service"
import { ErrorHandler } from '../infrastructure/error.handler';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css'],
  providers: [ErrorHandler]
})
export class AccountComponent {
  errors: any = {};
  isOnImage: boolean = false;
  @ViewChild("fileInput") fileInput: ElementRef = new ElementRef("");

  constructor(public dataService: AccountDataService, private errorHandler: ErrorHandler
    , private toastService: ToastService) { }
  EnterImage() {
    this.isOnImage = true;
  }
  LeaveImage() {
    this.isOnImage = false;
  }
  OpenFileInput() {
    this.fileInput.nativeElement.click();
  }
  FileSelected(event: any) {
    var file = <File>event.target.files[0];
    this.dataService.putImage(file, this.dataService.currentAccount.id).subscribe(
      (result: any) => {
      this.toastService.showSuccess("Файл загружен");
      this.dataService.currentAccount.accountImage = result;
      }, error => {
        this.errorHandler.Handle(error);
    });
  }
  SaveAccount() {
    this.dataService.postAccount().subscribe(
      result => {
      this.toastService.showSuccess("Профиль сохранен");
      this.errors = {};
      }, error => {
        this.errors = this.errorHandler.Handle(error);
    });
  }
}
