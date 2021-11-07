import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VideochatComponent } from './videochat.component';
import { VideoFrameComponent } from './videoframe.component';


@NgModule({
  declarations: [
    VideochatComponent,
    VideoFrameComponent
  ],
  imports: [
    CommonModule,
  ]
})
export class VideochatModule { }
