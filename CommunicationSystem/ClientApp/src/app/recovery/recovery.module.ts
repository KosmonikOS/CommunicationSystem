import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms"
import { DirectivesModule } from "../directives/directives.module"
import { RecoveryComponent } from "./recovery.component"


@NgModule({
  declarations: [
    RecoveryComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    DirectivesModule
  ]
})
export class RecoveryModule { }
