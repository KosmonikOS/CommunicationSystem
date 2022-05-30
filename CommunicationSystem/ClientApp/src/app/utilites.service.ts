import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core"

@Injectable({ "providedIn": "root" })
export class UtilitesService {
  url = "api/savefile/";
  constructor(private http: HttpClient) { }

  postImage(image: File) {
    var formData = new FormData();
    formData.append("File", image);
    return this.http.post(this.url + "image", formData);
  }
}
