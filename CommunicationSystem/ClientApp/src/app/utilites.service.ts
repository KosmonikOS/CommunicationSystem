import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core"

@Injectable({ "providedIn": "root" })
export class UtilitesService {
  url = "api/utilites/";
  constructor(private http: HttpClient) { }

  putImage(image: File) {
    var formData = new FormData();
    formData.append("ImageToSave", image);
    return this.http.put(this.url + "saveimage", formData);
  }
}
