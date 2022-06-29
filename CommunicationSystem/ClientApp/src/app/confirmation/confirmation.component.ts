import { Component, OnInit } from '@angular/core';
import { ConfirmationDataService } from "./confirmation.data.service"
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.css'],
  providers: [ConfirmationDataService]
})
export class ConfirmationComponent implements OnInit {
  message: string = "";
  constructor(private dataService: ConfirmationDataService,
    private router: Router, private activateRoute: ActivatedRoute) { }
  ngOnInit(): void {
    var token = this.activateRoute.snapshot.params["token"];
    this.dataService.getConfirmAccount(token).subscribe(
      result => {
        this.router.navigate([""]);
      },
      error => {
        if (error.status != 500) {
          this.message = error.error;
        } else {
          this.message = "Что-то пошло не так";
        }
      });
  };

}
