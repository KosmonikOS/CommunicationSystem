import { Injectable } from "@angular/core"
import { HttpClient, HttpHeaders } from "@angular/common/http"
import { AccountDataService } from "../account/account.data.service"
import * as signalR from "@microsoft/signalr";
import { MessageBetweenUsers } from "./messagebetweenusers";

@Injectable()
export class MessengerDataService {
  url = "/api/messenger/";
  hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder().withUrl("/messengerhub", { accessTokenFactory: () => window.localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY") || "" }).build();
  constructor(private http: HttpClient, private accountDataService: AccountDataService) { }
  getUsers(nickname: string) {
    return this.http.get(this.url + localStorage.getItem("CURRENT_COMMUNICATION_ID") + "/" + nickname);
  }
  getMessages(userid: number,togroup:number) {
    this.checkConnection();
    return this.http.get(this.url + "getmessages/" + localStorage.getItem("CURRENT_COMMUNICATION_ID") + "/" + userid + "/" + togroup);
  }
  postMessage(message: any, fileList: File[]) {
    this.checkConnection();
    if (fileList.length > 0) {
      var formData = new FormData();
      for (var i in fileList) {
        formData.append('ImageToSave-' + i, fileList[i]);
      }
      for (var j in message) {
        formData.append(j, message[j]);
      }
      return this.http.post(this.url + "filemessage/" + fileList.length, formData);
    }
    return this.http.post(this.url, message);
  }
  putGroupImage(file: File) {
    var formData = new FormData();
    formData.append("GroupImage", file);
    return this.http.put(this.url, formData);
  }
  postGroup(group: any) {
    return this.http.post(this.url + "groups", group);
  }
  getGroup(id: number) {
    return this.http.get(this.url + "groups/" + id);
  }
  deleteMessage(id: number,email:string) {
    return this.http.delete(this.url + id + "/" + email);
  }
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
}
