import { Component, OnInit, Input, OnChanges, SimpleChanges, ElementRef, ViewChild } from "@angular/core"
import { Member } from "./member"

@Component({
  selector: 'videoframe',
  templateUrl: './videoframe.component.html',
  styleUrls: ['./videochat.component.css'],
})
export class VideoFrameComponent implements OnInit, OnChanges {
  @ViewChild("videoFrame", { static: false }) video: ElementRef = new ElementRef(null);
  @Input() member: Member = new Member();
  @Input() state: boolean = false;


  ngOnChanges(changes: SimpleChanges): void {
    //console.log(this.member.remoteStream);
    this.loadVideo();
    console.log(this.member.remoteStream);
  }
  ngOnInit(): void {
    setTimeout(() => {
      this.loadVideo();
    }, 1000);
  }
  loadVideo() {
    var video = this.video?.nativeElement;
    if (video != null && this.member.remoteStream != null) {
      video.srcObject = this.member.remoteStream
      if (this.member.myself) {
        video.muted = true;
      } else {
        video.muted = this.member.audioState
      }
    }
  }

}
