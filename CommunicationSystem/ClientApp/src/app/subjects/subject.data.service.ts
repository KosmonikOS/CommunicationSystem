import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"

@Injectable()
export class SubjectDataService {
  url = "/api/subjects/"
  constructor(private http: HttpClient) { };

  getSubjects() {
    return this.http.get(this.url);
  }
  postSubject(subject: any) {
    return this.http.post(this.url, subject)
  }
  deleteSubject(id: number) {
    return this.http.delete(this.url + id);
  }
}
