import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessengerComponent } from './messenger.component';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    MessengerComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
  ]
})
export class MessengerModule { }
