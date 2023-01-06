import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VideochatComponent } from './videochat.component';
import { VideoFrameComponent } from './videoframe.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    VideochatComponent,
    VideoFrameComponent
  ],
  imports: [
    CommonModule,
    FormsModule
  ]
})
export class VideochatModule { }
