import { Injectable } from "@angular/core"

@Injectable({ providedIn: "root" })
export class AudioService {
  audio: HTMLAudioElement = new Audio();
  constructor() {
    this.audio.loop = true;
  }
  startAudio(src: string) {
    this.audio.src = src;
    this.audio.play();
  }
  stopAudio() {
    this.audio.pause();
    this.audio.currentTime = 0;
  }
}
