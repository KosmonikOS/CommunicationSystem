import { Injectable } from "@angular/core"
import * as signalR from "@microsoft/signalr";
import { State } from "./state";

@Injectable({ providedIn: 'root' })
export class VideochatDataService {
  hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder().withUrl("/videochathub", { accessTokenFactory: () => window.localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY") || "" }).build();
  startConnection() {
    return this.hubConnection.start();
  }
  closeConnection() {
    this.hubConnection.off("UserConnected");
    this.hubConnection.off("UserDisconnected");
    this.hubConnection.off("StateToggled");
    return this.hubConnection.stop();
  }
  addConnectionListener(name: string, func: any) {
    this.hubConnection.on(name, func);
  }
  connectToRoom(roomId: string, peerId: string) {
    return this.hubConnection.invoke("ConnectToRoom", roomId, peerId);
  }
  disconnectFromRoom(roomId: string, peerId: string) {
    return this.hubConnection.invoke("DisconnectFromRoom", roomId, peerId);
  }
  toggelState(roomId: string, peerId: string, type: State, value: boolean) {
    return this.hubConnection.invoke("ToggleState", roomId, peerId, type, value);
  }
}
