import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsereditComponent } from "./useredit.component"
import { DirectivesModule } from "../directives/directives.module"
import { PipesModule } from "../pipes/pipes.module"
import { FormsModule } from "@angular/forms"


@NgModule({
  declarations: [
    UsereditComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    DirectivesModule,
    PipesModule,
  ]
})
export class UsereditModule { }
