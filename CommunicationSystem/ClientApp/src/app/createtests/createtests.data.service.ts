import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { TestMember } from "./testmember";
@Injectable()
export class CreatetestsDataService {
  url = "/api/createtests/"
  constructor(private http: HttpClient) { }
  getTests(userId: number, roleId: number, page: number, searchOption: number, search: string) {
    return this.http.get(this.url + "tests/" + userId + "/" + roleId + "/" + page + "/" + searchOption + "/" + search);
  }
  getQuestions(testId: string) {
    return this.http.get(this.url + "questions/" + testId);
  }
  getSubjects() {
    return this.http.get("/api/subjects");
  }
  getSearchStudents(searchOption: number, search: string) {
    return this.http.get(this.url + "students/" + searchOption + "/" + search);
  }
  getStudents(testId: string) {
    return this.http.get(this.url + "students/" + testId);
  }
  getAnswers(userId: number, testId: string) {
    return this.http.get(this.url + "answers/" + userId + "/" + testId);
  }
  delete(id: string, type: string) {
    return this.http.delete(this.url + type + "/" + id.toString());
  }
  postTest(test: any) {
    return this.http.post(this.url, test);
  }
  putTest(test: any) {
    return this.http.put(this.url, test);
  }
  putMark(student: TestMember) {
    return this.http.put(this.url + "mark", student);
  }
}
