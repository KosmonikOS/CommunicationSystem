import {Injectable} from "@angular/core"

@Injectable({ providedIn: 'root' })
export class ToastService {
  errorMessage: string = "Error";
  errorShow: boolean = false;
  successMessage: string = "Success";
  successShow: boolean = false;
  alertMessage: string = "Warning";
  alertShow: boolean = false;

  showError(message: string) {
    this.errorMessage = message;
    this.errorShow = true;
  }
  showSuccess(message: string) {
    this.successMessage = message;
    this.successShow = true;
  }
  showAlert(message: string) {
    this.alertMessage = message;
    this.alertShow = true;
  }
}
