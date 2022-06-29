import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Guid } from 'guid-typescript';
import { DevicesService } from '../devices.service';
import { ToastService } from '../toast.service';
@Component({
  selector: 'app-createvideochat',
  templateUrl: './createvideochat.component.html',
  styleUrls: ['./createvideochat.component.css'],
})
export class CreateVideochatComponent implements OnInit {
  constructor(private modalService: NgbModal, private router: Router
    , private toastService: ToastService, private deviceService: DevicesService) { }
  roomId: string = "";
  @ViewChild("conferenceModal") conferenceModal: ElementRef = new ElementRef("");
  OpenConferenceModal() {
    this.modalService.open(this.conferenceModal, { size: "md", centered: true });
  }
  EnterConference() {
    if (Guid.isGuid(this.roomId)) {
      this.router.navigate(["videochat/" + this.roomId]);
      this.modalService.dismissAll();
    } else {
      this.toastService.showAlert("Неверный код доступа");
    }
  }
  CreateConference() {
    var roomId = Guid.create().toString();
    this.router.navigate(["videochat/" + roomId]);
  }
  ngOnInit(): void {
    this.deviceService.checkDevices().catch(() => {
      this.toastService.showAlert("Разрешите доступ в видео");
      this.router.navigate(["messenger"]);
    });
  }
}
