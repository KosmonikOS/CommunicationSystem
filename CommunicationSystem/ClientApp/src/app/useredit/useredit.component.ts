import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Account } from "../account/account"
import { UsereditDataService } from "./useredit.data.service"
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountDataService } from "../account/account.data.service"
import { ToastService } from "../toast.service"
import { Role } from './role';
import { UtilitesService } from '../utilites.service';
import { ErrorHandler } from '../infrastructure/error.handler';

@Component({
  selector: 'app-useredit',
  templateUrl: './useredit.component.html',
  styleUrls: ['./useredit.component.css', '../app.component.css'],
  providers: [UsereditDataService, AccountDataService, ErrorHandler]
})
export class UsereditComponent implements OnInit {
  users: Account[] = [];
  roles: Role[] = [];
  currentUser: Account = new Account();
  currentRow: number = -1;
  isOnImage: boolean = false;
  errors: any = {};
  search: string = "";
  page: number = 0;
  @ViewChild("fileInput") fileInput: ElementRef = new ElementRef("");
  @ViewChild("userModal") userModal: ElementRef = new ElementRef("");
  @ViewChild("usersList") usersList: ElementRef = new ElementRef("");
  searchOptions = [
    { "id": 0, "name": "ФИО", "searchOn": "Поиск по ФИО" },
    { "id": 1, "name": "Никнейм", "searchOn": "Поиск по никнейму" },
    { "id": 2, "name": "Почта", "searchOn": "Поиск по почте" },
    { "id": 3, "name": "Роль", "searchOn": "Поиск по роли" },
  ];
  currentSearchOption: number = 0;

  constructor(private dataService: UsereditDataService, private modalService: NgbModal
    , private utilitesService: UtilitesService, private errorHandler: ErrorHandler
    , private accountDataService: AccountDataService, private toastService: ToastService) { }

  NextPage() {
    this.page++;
    this.GetUsers(this.page, this.search);
    this.ScrollUp();
  }
  PreviousPage() {
    this.page--;
    this.GetUsers(this.page, this.search);
    this.ScrollUp();
  }
  Search() {
    this.page = 0;
    this.GetUsers(-1, this.search);
    this.ScrollUp();
  }
  Reload() {
    this.page = 0;
    this.search = "";
    this.GetUsers(this.page)
    this.ScrollUp();
  }
  ScrollUp() {
    this.usersList.nativeElement.scroll(0, 0);
  }

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
    this.utilitesService.postImage(file).subscribe((result: any) => {
      this.toastService.showSuccess("Файл загружен");
      this.currentUser.accountImage = result.path;
    }, error => {
      this.errorHandler.Handle(error);
    });
  }
  SaveUser() {
    this.currentUser.role = Number(this.currentUser.role);
    if (this.currentUser.id == 0) {
      this.dataService.postUser(this.currentUser).subscribe(result => {
        this.errors = {};
        this.modalService.dismissAll();
        this.GetUsers(this.page, this.search)
      }, error => {
        this.errors = this.errorHandler.Handle(error);
      });
    } else {
      this.dataService.putUser(this.currentUser).subscribe(result => {
        this.errors = {};
        this.modalService.dismissAll();
        this.GetUsers(this.page, this.search)
      }, error => {
        this.errors = this.errorHandler.Handle(error);
      });
    }
  }
  DblSetUser(user: Account, i: number) {
    this.currentUser = user;
    this.currentRow = i;
    this.EditUser();
  }
  SetUser(user: Account, i: number) {
    this.currentUser = this.currentUser == user ? new Account() : user;
    this.currentRow = this.currentRow == i ? -1 : i;
  }
  OpenUserModal() {
    this.modalService.open(this.userModal, { size: "xl", "scrollable": true }).result.then(() => { }, () => {
      this.currentUser == new Account();
      this.currentRow = -1;
      this.errors = {};
    });
  }
  CreateUser() {
    this.currentUser = new Account();
    this.OpenUserModal();
  }
  EditUser() {
    if (this.currentUser.id) {
      this.OpenUserModal();
    }
  }
  DeleteUser() {
    if (this.currentUser.id) {
      this.dataService.deleteUser(this.currentUser.id).subscribe(result => {
        this.currentUser == new Account();
        this.currentRow = -1;
        this.GetUsers(this.page, this.search);
      }, error => {
        this.errorHandler.Handle(error);
      })
    }
  }
  GetUsers(page: number, search: string = "") {
    this.dataService.getUsers(page, search, this.currentSearchOption).subscribe((data: any) => {
      this.users = data;
      this.currentUser = new Account();
      this.currentRow = -1;
    }, error => {
      this.errorHandler.Handle(error);
    })
  }
  ngOnInit(): void {
    this.GetUsers(this.page);
    this.dataService.getRoles().subscribe((data: any) => {
      this.roles = data;
    })
  }

}
