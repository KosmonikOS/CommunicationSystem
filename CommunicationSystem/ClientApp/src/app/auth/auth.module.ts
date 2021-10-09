import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthComponent } from './auth.component';
import { FormsModule } from "@angular/forms"
import { DirectivesModule } from "../directives/directives.module"

@NgModule({
  declarations: [
    AuthComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    DirectivesModule,
   
  ]
})
export class AuthModule { }
