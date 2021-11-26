import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Account } from "../account/account"
import { UsereditDataService } from "./useredit.data.service"
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountDataService } from "../account/account.data.service"
import { ToastService } from "../toast.service"

@Component({
  selector: 'app-useredit',
  templateUrl: './useredit.component.html',
  styleUrls: ['./useredit.component.css'],
  providers: [UsereditDataService,AccountDataService]
})
export class UsereditComponent implements OnInit {
  users: Account[] = [];
  public currentUser: Account = new Account();
  public currentRow: number = -1;
  isOnImage: boolean = false;
  errors: any = {};
  public search: string = "";
  @ViewChild("fileInput") fileInput: ElementRef = new ElementRef("");
  @ViewChild("userModal") userModal: ElementRef = new ElementRef("");

  constructor(private dataService: UsereditDataService, private modalService: NgbModal, private accountDataService: AccountDataService, private toastService: ToastService) { }

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
    this.accountDataService.putImage(file, this.currentUser.id).subscribe(result => { this.toastService.showSuccess("Файл загружен") }, error => { this.toastService.showError("Ошибка загрузки") });
  }
  saveUser() {
    this.dataService.postUser(this.currentUser).subscribe(result => {
      this.errors = {};
      this.currentUser == new Account();
      this.currentRow = -1;
      this.modalService.dismissAll();
    }, error => {
      this.toastService.showError("Ошибка сохранения");
      this.errors = error.error.errors;
    });
  }
  public setUser(user: Account, i: number) {
    this.currentUser = this.currentUser == user ? new Account() : user;
    this.currentRow = this.currentRow == i ? -1 : i;
  }
  public createUser() {
    this.currentUser = new Account();
    this.modalService.open(this.userModal, { size: "xl" });
  }
  public editUser() {
    if (this.currentUser.id) {
      this.modalService.open(this.userModal, { size: "xl" });
    }
  }
  public deleteUser() {
    this.dataService.deleteUser(this.currentUser.id).subscribe(result => {
      this.currentUser == new Account();
      this.currentRow = -1;
      this.getUsers();
    }, error => {
      this.toastService.showError("Ошибка удаления");
    })
  }
  getUsers() {
    this.dataService.getUsers().subscribe((data: any) => {
      this.users = data;
    })
  }
  ngOnInit(): void {
    this.getUsers();
  }

}
