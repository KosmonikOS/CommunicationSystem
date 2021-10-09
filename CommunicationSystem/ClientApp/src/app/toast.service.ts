import { Injectable } from "@angular/core"

@Injectable({ providedIn: 'root' })
export class ToastService {
  errorMessage: string = "Error";
  errorShow: boolean = false;

  showError(message: string) {
    this.errorMessage = message;
    this.errorShow = true;
  }
}
