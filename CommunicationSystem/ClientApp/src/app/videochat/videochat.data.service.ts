import { Injectable } from "@angular/core"
import * as signalR from "@microsoft/signalr";
import { Router } from "@angular/router"
import { AudioService } from "../audio.service"
import { DevicesService } from "../devices.service"

@Injectable({ providedIn: 'root' })
export class VideochatDataService {
  hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder().withUrl("/videochathub", { accessTokenFactory: () => window.localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY") || "" }).build();
  calling: any = null;
  caller: any = null;
  callState: boolean = false;
  members: any[] = [];
  constructor(private router: Router, private audioService: AudioService, private devicesService: DevicesService) { }

  startCall(calling: any) {
    this.devicesService.checkDevices().then(() => {
      calling["Caller"] = true;
      this.calling = calling;
      this.hubConnection.invoke("Ask", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), calling);
      this.callState = true;
      console.log(calling.email);
      if (calling.email != "Group") {
        this.audioService.startAudio("assets/caller.mp3");
      } else {
        this.router.navigate(["/videochat"]);
      }
    });
  }
  resetCall() {
    this.hubConnection.invoke("React", localStorage.getItem("CURRENT_COMMUNICATION_EMAIL"), "ResetCall", this.calling);
    this.calling = null;
    this.callState = false;
    this.audioService.stopAudio();
  }
  startConnection() {
    return new Promise((resolve, reject) => {
      this.hubConnection.start().then(() => {
        resolve("");
      }).catch((err => console.log(err)));
      this.hubConnection.onclose(() => {
        setTimeout(() => {
          this.checkConnection();
        }, 2000);
      });
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
    //this.hubConnection.stop();
  }
  checkConnection() {
    return new Promise((resolve, reject) => {
      if (this.hubConnection.state == "Disconnected") {
        this.hubConnection.start().then(() => {
          resolve("");
        }).catch((err => console.log(err)));
      } else {
        resolve("");
      };
    });
  };
}
