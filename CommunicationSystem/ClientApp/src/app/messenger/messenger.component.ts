import { Component, ElementRef, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { UserLastMessage } from "./userlastmessage"
import { MessageBetweenUsers } from "./messagebetweenusers"
import { MessengerDataService } from "./messenge.data.service"
import { AccountDataService } from "../account/account.data.service"
import { ToastService } from "../toast.service"
import { NgbModal} from '@ng-bootstrap/ng-bootstrap';
import { Group } from './group';

@Component({
  selector: 'app-messenger',
  templateUrl: './messenger.component.html',
  styleUrls: ['./messenger.component.css'],
  providers: [
    MessengerDataService,
    AccountDataService
  ]
})
export class MessengerComponent implements OnInit {
  search: string = "";
  @ViewChild("fileInput") fileInput: ElementRef = new ElementRef("");
  @ViewChild("fileGroupInput") fileGroupInput: ElementRef = new ElementRef("");
  @ViewChild("messagesArea") messagesArea: ElementRef = new ElementRef("");
  usersList: UserLastMessage[] = [];
  messagesList: MessageBetweenUsers[] = [];
  currentMessage: MessageBetweenUsers = new MessageBetweenUsers(Number(window.localStorage.getItem("CURRENT_COMMUNICATION_ID")), 0, false, "", "");
  selectedUser = 0;
  currentFiles: File[] = [];
  currentFilesLength: number = 0;
  isOnGroupImage: boolean = false;
  newGroup: Group = new Group();
  userGroupList: any[] = [];


  constructor(private dataService: MessengerDataService, private toastService: ToastService, private modalService: NgbModal) { }
  ///////////////////////////////////////////////////////////////USERS///////////////////////////////////////////////////////////////////////////////////////////////////////
  searchUsers() {
    this.dataService.getUsers(this.search).subscribe((data: any) => {
      this.usersList = data;
      this.toGroupUsersList(data);
    });
  }
  selectUser(index: number, user: UserLastMessage) {
    this.selectedUser = index;
    this.getMessages(user.id);
    this.currentMessage.to = user.id;
    this.currentMessage.toEmail = user.email;
    user.notViewed = 0;
  }
  sendMessage() {
    this.dataService.postMessage(this.currentMessage, this.currentFiles).subscribe(result => {
      this.getMessages(this.currentMessage.to);
      this.updateUserLastMessage(this.currentMessage, "to");
      this.currentMessage.content = "";
      this.currentFiles = [];
      this.currentFilesLength = 0;
    }, error => {
      this.toastService.showError("Ошибка отправки")
    });
  }
  getMessages(userid: number) {
    this.dataService.getMessages(userid).subscribe((data: any) => {
      this.messagesList = data;
      setTimeout(this.scrollMessages, 10, this.messagesArea.nativeElement);
    });
  }
  scrollMessages(element: any) {
    element.scrollTop = element.scrollHeight;
  }
  updateUserLastMessage(message: MessageBetweenUsers, type: string) {
    for (var i in this.usersList) {
      var val = this.usersList[i];
      if (type == "to" && val.id == message.to) {
        val.content = message.content;
        val.date = message.date;
      }
      if (type == "from" && val.id == message.from) {
        val.content = message.content;
        val.date = message.date;
        val.notViewed++;
      }
    }
  }
  openFileInput() {
    this.fileInput.nativeElement.click();
  }
  fileSelected(event: any) {
    var files = <FileList>event.target.files
    for (var i = 0; i <= files.length; i++) {
      this.currentFiles.push(files[i]);
      this.currentFilesLength++
    }
  }
  ///////////////////////////////////////////////////////////////USERS///////////////////////////////////////////////////////////////////////////////////////////////////////

  ///////////////////////////////////////////////////////////////GROUPS///////////////////////////////////////////////////////////////////////////////////////////////////////
  openGroupModal(modal: any) {
    this.modalService.open(modal);
  }
  enterImage() {
    this.isOnGroupImage = true;
  }
  leaveImage() {
    this.isOnGroupImage = false;
  }
  openGroupFileInput() {
    this.fileGroupInput.nativeElement.click();
  }
  groupFileSelected(event:any) {
    var file = <File>event.target.files[0];
    this.dataService.putGroupImage(file).subscribe((result:any) => this.newGroup.groupImage = result.path);
  }
  toGroupUsersList(list: UserLastMessage[]) {
    for (var i in list) {
      this.userGroupList.push({ "id": list[i].id, "itemName": list[i].nickName });
    }
  }
  saveGroup() {
    this.newGroup.users?.push({ "id": Number(localStorage.getItem("CURRENT_COMMUNICATION_ID")) ,"itemName": "Me" });
    this.dataService.postGroup(this.newGroup).subscribe(result => { });
  }
  ///////////////////////////////////////////////////////////////GROUPS///////////////////////////////////////////////////////////////////////////////////////////////////////
  ngOnInit(): void {
    this.dataService.getUsers("").subscribe((data: any) => {
      this.usersList = data;
      this.getMessages(data[0].id);
      this.currentMessage.to = data[0].id;
      this.currentMessage.toEmail = data[0].email;
      this.toGroupUsersList(data);
    });
    this.dataService.startConnection();
    this.dataService.addConnectionListener("Recive", (message: MessageBetweenUsers) => {
      this.updateUserLastMessage(message, "from");
      if (this.currentMessage.to == message.from) {
        this.getMessages(message.from);
      }
    });
  }
}
