import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms"
import { DirectivesModule } from "../directives/directives.module"
import { RegistrationComponent } from "./registration.component"


@NgModule({
  declarations: [
    RegistrationComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    DirectivesModule
  ]
})
export class RegistrationModule { }
