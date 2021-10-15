import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { AccountDataService } from "../account/account.data.service"

@Injectable()
export class MessengerDataService {
  url = "/api/messenger/";
  constructor(private http: HttpClient, private accountDataService: AccountDataService) { }
  getUsers(nickname: string) {
    return this.http.get(this.url + localStorage.getItem("CURRENT_COMMUNICATION_ID") + "/" + nickname);
  }
  getMessages(userid: number) {
    return this.http.get(this.url + "getmessages/" + localStorage.getItem("CURRENT_COMMUNICATION_ID") + "/" + userid);
  }
  postMessage(message: any) {
    return this.http.post(this.url, message);
  }
}
