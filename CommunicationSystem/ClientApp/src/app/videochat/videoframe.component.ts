import { Component, OnInit, Input, OnChanges, SimpleChanges, ElementRef, ViewChild } from "@angular/core"
import { Member } from "./memberV2";

@Component({
  selector: 'videoframe',
  templateUrl: './videoframe.component.html',
  styleUrls: ['./videochat.component.css'],
})
export class VideoFrameComponent implements OnInit, OnChanges {
  @ViewChild("videoFrame", { static: false }) video: ElementRef = new ElementRef(null);
  @Input() member: Member = new Member();
  @Input() state: boolean = false;
  @Input() screenState: boolean = false;
  startUpdate: boolean = false;


  ngOnChanges(changes: SimpleChanges): void {
    this.UpdateVideo();
  }
  ngOnInit(): void {
    setTimeout(() => {
      this.LoadVideo();
      this.startUpdate = true;
    }, 1000)
  }
  LoadVideo() {
    var video = this.video?.nativeElement;
    if (video != null && this.member.stream != null) {
      //video.setAttribute('autoplay', '');
      //video.setAttribute('playsinline', '');
      if (this.member.myself)
        video.muted = true;
      video.srcObject = this.member.stream;
      //video.addEventListener("loadedmetadata", () => {
      //  video.play()
      //})
    }
  }
  UpdateVideo() {
    if (this.startUpdate) {
      var video = this.video?.nativeElement;
      if (video != null && this.member.stream != null) {
        if (!this.member.myself)
          video.muted = !this.member.audioState;
      }
    }
  }
}
