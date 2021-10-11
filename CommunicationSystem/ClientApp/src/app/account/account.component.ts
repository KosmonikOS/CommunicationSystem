import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Account } from './account';
import { AccountDataService } from './account.data.service';
import { ToastService } from "../toast.service"

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  errors: any = {};
  isOnImage: boolean = false;
  @ViewChild("fileInput") fileInput: ElementRef = new ElementRef("");

  constructor(public dataService: AccountDataService, private toastService: ToastService) { }

  ngOnInit(): void {
   this.dataService.getAccount(localStorage.getItem("CURRENT_COMMUNICATION_EMAIL") || "");
  }
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
    this.dataService.postAccount().subscribe(result => { this.toastService.showSuccess("Профиль сохранен") }, error => { this.toastService.showError("Ошибка сохранения") });
  }
}
