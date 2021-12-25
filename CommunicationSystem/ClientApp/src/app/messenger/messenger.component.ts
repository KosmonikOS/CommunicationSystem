import { Component, ElementRef, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { UserLastMessage } from "./userlastmessage"
import { MessageBetweenUsers } from "./messagebetweenusers"
import { MessengerDataService } from "./messenge.data.service"
import { AccountDataService } from "../account/account.data.service"
import { ToastService } from "../toast.service"
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Group } from './group';
import { VideochatDataService } from '../videochat/videochat.data.service';

@Component({
  selector: 'app-messenger',
  templateUrl: './messenger.component.html',
  styleUrls: ['./messenger.component.css'],
  providers: [
    MessengerDataService,
    AccountDataService,
  ]
})
export class MessengerComponent implements OnInit {
  search: string = "";
  @ViewChild("fileInput") fileInput: ElementRef = new ElementRef("");
  @ViewChild("fileGroupInput") fileGroupInput: ElementRef = new ElementRef("");
  @ViewChild("messagesArea") messagesArea: ElementRef = new ElementRef("");
  usersList: UserLastMessage[] = [];
  messagesList: MessageBetweenUsers[] = [];
  currentMessage: MessageBetweenUsers = new MessageBetweenUsers(Number(window.localStorage.getItem("CURRENT_COMMUNICATION_ID")), 0, 0, "", "");
  currentUser: UserLastMessage = new UserLastMessage(0, "", "", "", 0, 0, 0, "", 0);
  currentImage: number = 0;
  ImagesList: string[] = [];
  selectedUser = 0;
  currentFiles: File[] = [];
  currentFilesLength: number = 0;
  isOnGroupImage: boolean = false;
  newGroup: Group = new Group([]);
  userGroupList: any[] = [];
  isOpen: boolean = true;

  constructor(private dataService: MessengerDataService, private toastService: ToastService, private modalService: NgbModal, public videochatDataService: VideochatDataService) { }
  ///////////////////////////////////////////////////////////////USERS///////////////////////////////////////////////////////////////////////////////////////////////////////
  openUserList() {
    this.isOpen = true;
  }
  searchUsers() {
    this.dataService.getUsers(this.search).subscribe((data: any) => {
      this.usersList = data;
      this.toGroupUsersList(data);
    });
  }
  selectUser(index: number, user: UserLastMessage) {
    this.isOpen = false;
    this.selectedUser = index;
    this.currentUser = user
    if (user.email == "Group") {
      this.currentMessage.togroup = user.id;
      this.currentMessage.to = 0;
      this.currentMessage.toEmail = "Group"
    } else {
      this.currentMessage.togroup = 0;
      this.currentMessage.to = user.id;
      this.currentMessage.toEmail = user.email;
    }
    this.getMessages(user.id, this.currentMessage.togroup);
    user.notViewed = 0;
  }
  sendMessage() {
    this.dataService.postMessage(this.currentMessage, this.currentFiles).subscribe(result => {
      this.getMessages(this.currentMessage.to, this.currentMessage.togroup);
      this.updateUserLastMessage(this.currentMessage, "to");
      //this.searchUsers();
      this.currentMessage.content = "";
      this.currentFiles = [];
      this.currentFilesLength = 0;
    }, error => {
      this.toastService.showError("Ошибка отправки")
    });
  }
  getMessages(userid: number, togroup: number) {
    this.dataService.getMessages(userid, togroup).subscribe((data: any) => {
      this.messagesList = data;
      setTimeout(this.scrollMessages, 10, this.messagesArea.nativeElement);
      this.loadImagesList(data);
    });
  }
  scrollMessages(element: any) {
    element.scrollTop = element.scrollHeight;
  }
  updateUserLastMessage(message: any, type: string) {
    for (var i in this.usersList) {
      var val = this.usersList[i];
      if (type == "to" && val.id == (message.to == 0 ? message.toGroup : message.to)) {
        val.content = message.content;
        val.date = message.date;
      }
      if (type == "from" && (val.id == (message.to == 0 ? message.toGroup : message.from))) {
        val.notViewed += (this.currentMessage.to != message.from && message.toEmail != "Group") || (this.currentMessage.togroup != message.toGroup)? 1 : 0;
        val.content = message.content;
        val.date = message.date;
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
  openImage(image: string, modal: any) {
    this.currentImage = this.ImagesList.indexOf(image);
    this.modalService.open(modal, { size: 'xl' });
    console.log(this.currentImage);
  }
  toggleImage(step: string) {
    var length = this.ImagesList.length;
    if (step == "next" && this.currentImage + 1 < length) {
      this.currentImage++;
    }
    if (step == "previous" && this.currentImage - 1 >= 0) {
      this.currentImage--;
    }
  }
  loadImagesList(messages: any) {
    for (var i in messages) {
      var message = messages[i];
      if (message.type == 1) {
        this.ImagesList.push(message.content);
      }
    }
  }
  deleteMessage(id?: number) {
    if (confirm("Вы уверены , что хотите удалить сообщение?")) {
      this.dataService.deleteMessage(id || 0, this.currentMessage.toEmail).subscribe((result: any) => {
        this.getMessages(this.currentMessage.to, this.currentMessage.togroup);
        if (result.message.id >= (this.messagesList[this.messagesList.length - 1].id || 0)) {
          this.updateUserLastMessage(result.message, "to");
        }
      });
    }
  }
  ///////////////////////////////////////////////////////////////USERS///////////////////////////////////////////////////////////////////////////////////////////////////////

  ///////////////////////////////////////////////////////////////GROUPS///////////////////////////////////////////////////////////////////////////////////////////////////////
  openGroupModal(modal: any) {
    this.newGroup = new Group([]);
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
  groupFileSelected(event: any) {
    var file = <File>event.target.files[0];
    this.dataService.putGroupImage(file).subscribe((result: any) => this.newGroup.groupImage = result.path);
  }
  toGroupUsersList(list: UserLastMessage[]) {
    var temp = [];
    for (var i in list) {
      if (list[i].email != "Group") {
        temp.push({ "id": list[i].id, "itemName": list[i].nickName, "accountImage": list[i].accountImage });
      }
    }
    this.userGroupList = temp;
  }
  saveGroup() {
    if (this.newGroup.id == null) {
      this.newGroup.users.push({ "id": Number(localStorage.getItem("CURRENT_COMMUNICATION_ID")), "itemName": "Me" });
    }
    this.dataService.postGroup(this.newGroup).subscribe(result => {
      this.modalService.dismissAll();
      this.newGroup = new Group([]);
      this.searchUsers();
    });
  }
  openGroupSettings(id: number, modal: any) {
    this.dataService.getGroup(id).subscribe((data: any) => {
      this.newGroup = data;
      this.modalService.open(modal);
    })
  }
  ///////////////////////////////////////////////////////////////GROUPS///////////////////////////////////////////////////////////////////////////////////////////////////////
  ngOnInit(): void {
    this.dataService.getUsers("").subscribe((data: any) => {
      this.usersList = data;
      if (data[0].email == "Group") {
        this.getMessages(data[0].id, data[0].id);
      } else {
        this.getMessages(data[0].id, 0);
      }
      this.currentMessage.to = data[0].id;
      if (data[0].email == "Group") {
        this.currentMessage.togroup = data[0].id;
      }
      this.currentMessage.toEmail = data[0].email;
      this.toGroupUsersList(data);
      this.currentUser = data[0];
    });
    this.dataService.startConnection();
    this.dataService.addConnectionListener("Recive", (message: any) => {
      if (message.id >= (this.messagesList[this.messagesList.length - 1].id || 0)) {
        this.updateUserLastMessage(message, "from");
      }
      if ((this.currentMessage.to == message.from && message.toEmail != "Group") || (this.currentMessage.togroup == message.toGroup)) {
        this.getMessages(message.from, message.toGroup);
      }
    });
  }
}
