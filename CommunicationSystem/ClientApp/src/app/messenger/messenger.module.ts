import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessengerComponent } from './messenger.component';
import { FormsModule } from '@angular/forms';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';



@NgModule({
  declarations: [
    MessengerComponent
  ],
  imports: [
    CommonModule,
    AngularMultiSelectModule,
    FormsModule,
  ]
})
export class MessengerModule { }
