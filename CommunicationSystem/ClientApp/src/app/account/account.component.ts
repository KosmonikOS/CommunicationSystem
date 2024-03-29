import { Component, ElementRef, ViewChild } from '@angular/core';
import { Account } from './account';
import { AccountDataService } from './account.data.service';
import { ToastService } from "../toast.service"
import { ErrorHandler } from '../infrastructure/error.handler';
import { UtilitesService } from '../utilites.service';

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

  constructor(public dataService: AccountDataService, private utilitesService: UtilitesService,
    private errorHandler: ErrorHandler
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
    if (file !== undefined) {
      if (file.type.includes('image')) {
        this.utilitesService.postImage(file).subscribe(
          (result: any) => {
            console.log(result);
            this.toastService.showSuccess("Файл загружен");
            this.dataService.currentAccount.accountImage = result;
          }, error => {
            this.errorHandler.Handle(error);
          });
      } else {
        this.toastService.showAlert("Пожалуйста загрузите изображение");
      }
    }
  }
  SaveAccount() {
    this.dataService.putAccount().subscribe(
      result => {
        this.toastService.showSuccess("Профиль сохранен");
        this.errors = {};
      }, error => {
        this.errors = this.errorHandler.Handle(error);
      });
  }
}
