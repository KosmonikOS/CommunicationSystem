import { Injectable } from "@angular/core"
import * as signalR from "@microsoft/signalr";
import { Router } from "@angular/router"

@Injectable({ providedIn: 'root' })
export class VideochatDataService {
  //hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder().withUrl("https://localhost:5001/videochathub", { accessTokenFactory: () => window.localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY") || "" }).build();
  hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder().withUrl("/videochathub", { accessTokenFactory: () => window.localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY") || "" }).build();
  connectionStatus: boolean = false;
  calling: any = null;
  constructor(private router: Router) { }

  startCall(calling: any) {
    this.calling = calling;
    this.router.navigate(["/videochat"]);
    //this.startConnection();
  }
  sendRequestToStartChat(peer: any, calling: any) {
    //peer.on("signal", (data: any) => {
    //  this.hubConnection.invoke("StartCall", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), calling, {Data: data,Dst:"RemotePeer" });
    //});
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
  checkConnection() {
    if (!this.connectionStatus) {
      this.startConnection();
    }
  }
}
