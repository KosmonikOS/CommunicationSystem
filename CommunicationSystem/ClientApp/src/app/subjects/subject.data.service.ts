import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"

@Injectable()
export class SubjectDataService {
  url = "/api/subjects/"
  constructor(private http: HttpClient) { };

  getSubjects(search: string, page: number) {
    return this.http.get(this.url + page + "/" + search);
  }
  postSubject(subject: any) {
    return this.http.post(this.url, subject);
  }
  putSubject(subject: any) {
    return this.http.put(this.url, subject);
  }
  deleteSubject(id: number) {
    return this.http.delete(this.url + id);
  }
}
