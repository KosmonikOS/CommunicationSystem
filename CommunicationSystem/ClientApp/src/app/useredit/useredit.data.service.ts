import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"

@Injectable()
export class UsereditDataService {
  url = "api/useredit/"
  constructor(private http: HttpClient) { }

  getUsers(page: number, search: string, searchOption: number) {
    return this.http.get(this.url + page + "/" + searchOption + "/" + search);
  }
  getRoles() {
    return this.http.get(this.url + "getroles");
  }
  postUser(user: any) {
    return this.http.post(this.url, user);
  }
  putUser(user: any) {
    return this.http.put(this.url, user);
  }
  deleteUser(id: number) {
    return this.http.delete(this.url + id);
  }
}
