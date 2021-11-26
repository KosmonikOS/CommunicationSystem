import { Component, ElementRef, ViewChild } from '@angular/core';
import { Account } from './account';
import { AccountDataService } from './account.data.service';
import { ToastService } from "../toast.service"

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent {
  errors: any = {};
  isOnImage: boolean = false;
  @ViewChild("fileInput") fileInput: ElementRef = new ElementRef("");

  constructor(public dataService: AccountDataService, private toastService: ToastService) { }
  enterImage() {
    this.isOnImage = true;
  }
  leaveImage() {
    this.isOnImage = false;
  }
  openFileInput() {
    this.fileInput.nativeElement.click();
  }
  fileSelected(event: any) {
    var file = <File>event.target.files[0];
    this.dataService.putImage(file, this.dataService.currentAccount.id).subscribe(result => { this.toastService.showSuccess("Файл загружен") }, error => { this.toastService.showError("Ошибка загрузки") });
  }
  saveAccount() {
    this.dataService.postAccount().subscribe(result => {
      this.toastService.showSuccess("Профиль сохранен");
      this.errors = {};
    }, error => {
      this.toastService.showError("Ошибка сохранения");
      this.errors = error.error.errors;
    });
  }
}
