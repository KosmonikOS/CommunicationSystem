import { Injectable } from "@angular/core"
import * as signalR from "@microsoft/signalr";
import { Router } from "@angular/router"

@Injectable({ providedIn: 'root' })
export class VideochatDataService {
  hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder().withUrl("/videochathub", { accessTokenFactory: () => window.localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY") || "" }).build();
  connectionStatus: boolean = false;
  calling: any = null;
  constructor(private router: Router) { }

  startCall(calling: any) {
    calling["Caller"] = true;
    this.calling = calling;
    this.router.navigate(["/videochat"]);
  }
  startConnection() {
    return new Promise((resolve, reject) => {
      this.hubConnection.start().then(() => {
        this.connectionStatus = true;
        resolve("");
      }).catch((err => console.log(err)));
      this.hubConnection.onclose(() => this.connectionStatus = false);
    })
  }
  addConnectionListener(name: string, func: any) {
    this.hubConnection.on(name, func);
  }
  closeConnection() {
    this.hubConnection.off("Accept");
    this.hubConnection.off("DestroyConnection");
    this.hubConnection.off("ToggleVideo");
    this.hubConnection.off("ToggleAudio");
    this.hubConnection.stop().then(() => {
      this.connectionStatus = false;
    });
  }
  checkConnection() {
    return new Promise((resolve, reject) => {
      if (!this.connectionStatus) {
        this.hubConnection.start().then(() => {
          this.connectionStatus = true;
          resolve("");
        }).catch((err => console.log(err)));
        this.hubConnection.onclose(() => this.connectionStatus = false);
      } else {
        resolve("");
      };
    });
  };
}
