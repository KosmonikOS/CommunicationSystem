import { HttpClient } from "@angular/common/http"
import { Injectable } from "@angular/core"
import { Question } from "./question";
import { Test } from "./test";

@Injectable()
export class TestsDataService {
  url = "api/tests/";
  constructor(private http: HttpClient) { }
  getTests(userId: number, page: number, searchOption: number, search: string) {
    return this.http.get(this.url + "tests/" + userId + "/" + page + "/" + searchOption + "/" + search);
  }
  getQuestions(testId: string) {
    return this.http.get(this.url + "questions/" + testId);
  }
  postTest(questions: Question[], userId: number, testId: string) {
    return this.http.post(this.url, { "questions": questions, "userId": userId, "testId": testId });
  }
}
