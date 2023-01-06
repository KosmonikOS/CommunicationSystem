import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms"
import { DirectivesModule } from "../directives/directives.module"
import { CreateVideochatComponent } from "./createvideochat.component"


@NgModule({
  declarations: [
    CreateVideochatComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    DirectivesModule
  ]
})
export class CreateVideochatModule { }
