import { Component } from '@angular/core';
import { Registration } from './registration';
import { RegistrationDataService } from "./registration.data.service"
import { Router } from '@angular/router';
@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  providers: [RegistrationDataService]
})
export class RegistrationComponent {
  registration: Registration = new Registration();
  errors: any = {};
  approval: boolean = false;
  constructor(private dataService: RegistrationDataService, private router: Router) { };
  Register() {
    if (this.approval) {
      this.dataService.postRegistration(this.registration).subscribe(
        result => {
          this.router.navigate([""])
        },
        error => {
          this.errors = error.error.errors;
        }
      )
    }
  }

}
