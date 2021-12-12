import { HttpClient } from "@angular/common/http"
import { Injectable } from "@angular/core"
import { Question } from "./question";
import { Test } from "./test";

@Injectable()
export class TestsDataService {
  url = "api/tests/";
  constructor(private http: HttpClient) { }
  getTests(id: number) {
    return this.http.get(this.url + id);
  }
  postTest(questions: Question[], id: number, testId: number) {
    return this.http.post(this.url, { "questions": questions, "userId": id, "testId": testId });
  }
}
