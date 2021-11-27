import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreatetestsComponent } from './createtests.component'
import { FormsModule } from '@angular/forms';
import { DirectivesModule } from "../directives/directives.module"
import { PipesModule } from "../pipes/pipes.module"

@NgModule({
  declarations: [
    CreatetestsComponent],
  imports: [
    CommonModule,
    FormsModule,
    DirectivesModule,
    PipesModule,
  ]
})
export class CreatetestsModule { }
