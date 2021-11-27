import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastService } from "../toast.service"
import {Subject} from "./subject"
import { SubjectDataService } from './subject.data.service';
@Component({
  selector: 'app-subjects',
  templateUrl: './subjects.component.html',
  styleUrls: ['./subjects.component.css'],
  providers: [SubjectDataService]
})
export class SubjectsComponent implements OnInit {

  subjects: Subject[] = [];
  public currentSubject: Subject = new Subject();
  public currentRow: number = -1;
  errors: any = {};
  public search: string = "";
  @ViewChild("subjectModal") subjectModal: ElementRef = new ElementRef("");

  constructor(private dataService: SubjectDataService, private modalService: NgbModal,  private toastService: ToastService) { }

  saveSubject() {
    this.dataService.postSubject(this.currentSubject).subscribe(result => {
      this.errors = {};
      this.modalService.dismissAll();
      this.getSubjects()
    }, error => {
      this.toastService.showError("Ошибка сохранения");
      this.errors = error.error.errors;
    });
  }
  public dblSetSubject(subject: Subject, i: number) {
    this.currentSubject = subject;
    this.currentRow = i;
    this.editSubject();
  }
  public setSubject(subject: Subject, i: number) {
    this.currentSubject = this.currentSubject == subject ? new Subject() : subject;
    this.currentRow = this.currentRow == i ? -1 : i;
  }
  public opensubjectModal() {
    this.modalService.open(this.subjectModal, { size: "sm", centered:true }).result.then(() => { }, () => {
      this.currentSubject == new Subject();
      this.currentRow = -1;
      this.errors = {};
    });
  }
  public createSubject() {
    this.currentSubject = new Subject();
    this.opensubjectModal();
  }
  public editSubject() {
    if (this.currentSubject.id) {
      this.opensubjectModal();
    }
  }
  public deleteSubject() {
    this.dataService.deleteSubject(this.currentSubject.id).subscribe(result => {
      this.currentSubject == new Subject();
      this.currentRow = -1;
      this.getSubjects();
    }, error => {
      this.toastService.showError("Ошибка удаления");
    })
  }
  getSubjects() {
    this.dataService.getSubjects().subscribe((data: any) => {
      this.subjects = data;
    })
  }
 

  ngOnInit(): void {
    this.getSubjects();
  }

}




