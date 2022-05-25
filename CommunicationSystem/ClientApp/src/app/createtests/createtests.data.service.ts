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
  getAnswers(id: number, testId: string) {
    return this.http.get(this.url + "getanswers/" + id + "/" + testId);
  }
  delete(id: string, type: string) {
    return this.http.delete(this.url + type + "/" + id);
  }
  postTest(test: any) {
    return this.http.post(this.url, test);
  }
  putMark(id: number, testid: string, mark: number) {
    return this.http.put(this.url + id + "/" + testid + "/" + mark, {});
  }
}
