import { Component, OnInit } from '@angular/core';
import { UserLastMessage } from "./userlastmessage"
import { MessageBetweenUsers } from "./messagebetweenusers"
import { MessengerDataService } from "./messenge.data.service"
import { AccountDataService } from "../account/account.data.service"

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
  usersList: UserLastMessage[] = [];
  messagesList: MessageBetweenUsers[] = [];
  currentMessage: MessageBetweenUsers = new MessageBetweenUsers(Number(window.localStorage.getItem("CURRENT_COMMUNICATION_ID")), 0, false, "");
  selectedUser = 0;

  constructor(private dataService: MessengerDataService) { }

  searchUsers() {
    this.dataService.getUsers(this.search).subscribe((data: any) => { this.usersList = data });
  }
  selectUser(index: number, userid: number) {
    this.selectedUser = index;
    this.dataService.getMessages(userid).subscribe((data: any) => { this.messagesList = data });
    this.currentMessage.to = userid;
  }

  ngOnInit(): void {
    this.dataService.getUsers("").subscribe((data: any) => {
      this.usersList = data;
      this.dataService.getMessages(data[0].id).subscribe((data: any) => { this.messagesList = data });
      this.currentMessage.to = data[0].id;
    });
  }
  sendMessage() {
    this.dataService.postMessage(this.currentMessage).subscribe(result => { });
    this.currentMessage.content = "";
  }
}
