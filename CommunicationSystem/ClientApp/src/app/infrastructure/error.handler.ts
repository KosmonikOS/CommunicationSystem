import { Injectable } from "@angular/core"
import { ToastService } from "../toast.service"
@Injectable()
export class ErrorHandler {
  constructor(private toastService: ToastService) { };
  Handle(error: any): any {
    if (typeof error.error == "string" && error.status != 500) {
      this.toastService.showError(error.error);
    }
    if (error.status == 500) {
      this.toastService.showError("Что-то пошло не так");
    }
    return error.error.errors;
  }
  HandleWitoutValidation(error: any): any {
    if (typeof error.error == "string" && error.status != 500) {
      this.toastService.showError(error.error);
    }
    if (error.status == 500) {
      this.toastService.showError("Что-то пошло не так");
    }
    if (error.error.errors !== undefined) {
      this.toastService.showError("Не все поля корректно заполнены");
    }
    return error.error.errors;
  }
}
