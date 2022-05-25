import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms"
import { DirectivesModule } from "../directives/directives.module"
import { ConfirmationComponent } from "./confirmation.component"


@NgModule({
  declarations: [
    ConfirmationComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    DirectivesModule
  ]
})
export class ConfirmationModule { }
