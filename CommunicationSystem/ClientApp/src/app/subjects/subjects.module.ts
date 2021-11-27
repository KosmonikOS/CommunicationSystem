import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubjectsComponent } from "./subjects.component"
import { FormsModule } from "@angular/forms"
import { DirectivesModule } from "../directives/directives.module"
import { PipesModule } from "../pipes/pipes.module"
@NgModule({
  declarations: [
    SubjectsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    DirectivesModule,
    PipesModule
  ]
})
export class SubjectsModule { }
