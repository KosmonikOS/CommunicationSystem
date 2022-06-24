import { Injectable } from "@angular/core"
import { HttpClient, HttpHeaders } from "@angular/common/http"
import { AccountDataService } from "../account/account.data.service"
import * as signalR from "@microsoft/signalr";
import { SendMessage } from "./sendmessage";
import { Message } from "./message";

@Injectable()
export class MessengerDataService {
  messageUrl = "/api/messenger/";
  contactUrl = "/api/contacts/";
  groupUrl = "/api/groups/";
  hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder().withUrl("/messengerhub", { accessTokenFactory: () => window.localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY") || "" }).build();
  constructor(private http: HttpClient, private accountDataService: AccountDataService) { }
  getUsers(nickname: string) {
    return this.http.get(this.messageUrl + localStorage.getItem("CURRENT_COMMUNICATION_ID") + "/" + nickname);
  }
  getMessages(userid: number, togroup: number) {
    this.checkConnection();
    return this.http.get(this.messageUrl + "getmessages/" + localStorage.getItem("CURRENT_COMMUNICATION_ID") + "/" + userid + "/" + togroup);
  }


  sendFileMessages(message: any, file: File) {
    var formData = new FormData();
    formData.append("File", file);
    for (var j in message) {
      formData.append(j, message[j]);
    }
    return this.http.post(this.messageUrl + "files", formData);
  }
  deleteMessage(id: number) {
    return this.http.delete(this.messageUrl + id);
  }
  updateMessage(id: number, content: string) {
    return this.http.put(this.messageUrl + "content", {
      "Id": id,
      "Content": content
    });
  }
  viewMessage(id: number) {
    return this.http.put(this.messageUrl + "view/" + id, {});
  }
  sendMessage(message: SendMessage) {
    //this.hubConnection.invoke("SendMessage", message);
    return this.http.post(this.messageUrl, message);
  }
  searchContacts(search: string) {
    return this.http.get(this.contactUrl + "search/" + search);
  }
  getContacts() {
    return this.http.get(this.contactUrl + localStorage.getItem("CURRENT_COMMUNICATION_ID"));
  }
  getContact(from: number) {
    return this.http.get(this.contactUrl + "contact/" + from);
  }
  getGroupContact(fromGroup: string) {
    return this.http.get(this.contactUrl + "group/" + fromGroup);
  }
  getContactMessages(contactId: number, page: number) {
    return this.http.get(this.messageUrl + "contact/messages/" + localStorage.getItem("CURRENT_COMMUNICATION_ID") + "/" + contactId + "/" + page);
  }
  getGroupMessages(groupId: string, page: number) {
    return this.http.get(this.messageUrl + "group/messages/" + localStorage.getItem("CURRENT_COMMUNICATION_ID") + "/" + groupId + "/" + page);
  }
  /////////////////////////////GROUPS/////////////////////////////////////
  postGroup(group: any) {
    return this.http.post(this.groupUrl, group);
  }
  putGroup(group: any) {
    return this.http.put(this.groupUrl, group);
  }
  getGroup(id: string) {
    return this.http.get(this.groupUrl + id);
  }
  getMembersBySearch(search: string) {
    return this.http.get(this.groupUrl + "members/" + search);
  }
  /////////////////////////////GROUPS/////////////////////////////////////

  /////////////////////////////Hub////////////////////////////////////////
  startConnection() {
    this.hubConnection.start().catch((err => console.log(err)));
  }
  addConnectionListener(name: string, func: any) {
    this.hubConnection.on(name, func);
  }
  checkConnection() {
    if (this.hubConnection.state == "Disconnected") {
      this.startConnection();
    }
  }
  /////////////////////////////Hub////////////////////////////////////////
}
