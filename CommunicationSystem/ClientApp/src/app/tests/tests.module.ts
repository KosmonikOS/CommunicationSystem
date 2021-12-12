import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TestsComponent } from './tests.component';
import { FormsModule } from '@angular/forms';
import { PipesModule } from "../pipes/pipes.module"

@NgModule({
  declarations: [
    TestsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    PipesModule
  ]
})
export class TestsModule { }
