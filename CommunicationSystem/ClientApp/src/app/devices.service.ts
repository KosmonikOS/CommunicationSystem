import { Injectable } from "@angular/core"

@Injectable({ providedIn: "root" })
export class DevicesService {
  checkMedia(): Promise<any> {
    return new Promise((resolve, reject) => {
      navigator.mediaDevices.enumerateDevices().then((devices) => {
        var video = false;
        var audio = false;
        devices.forEach(function (device) {
          video = video || device.kind.indexOf("video") > -1 ? true : false;
          audio = audio || device.kind.indexOf("audio") > -1 ? true : false;
        });
        resolve({ video: video, audio: audio });
      });
    })
  }
  checkDevices() {
    return new Promise(async (resolve, reject) => {
      try {
        var mediaConfig = await this.checkMedia();
        //var video = await navigator.permissions.query({ name: "camera" }).then((res) => res.state == "granted");
        //var audio = await navigator.permissions.query({ name: "microphone" }).then((res) => res.state == "granted");
        //if ((mediaConfig.video && !video) || (mediaConfig.audio && !audio)) {
        await navigator.mediaDevices.getUserMedia({ video: mediaConfig.video, audio: mediaConfig.audio }).then((stream) => {
          stream.getTracks()?.forEach(function (track: any) {
            track?.stop();
          });
        });
        //}
        resolve("");
      } catch {
        reject();
      }
    });
  }
}
