import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
@Injectable()
export class CreatetestsDataService {
  url = "/api/createtests/"
  constructor(private http: HttpClient) { }
  getTests(id: number) {
    return this.http.get(this.url  + id);
  }
  getSubjects() {
    return this.http.get("/api/subjects");
  }
  getUsers(param: string) {
    return this.http.get(this.url + "getusers/" + param);
  }
  delete(id: number, type: string) {
    return this.http.delete(this.url + type + "/" + id);
  }
}
