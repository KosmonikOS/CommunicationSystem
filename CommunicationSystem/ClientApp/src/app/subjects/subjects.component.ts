import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ErrorHandler } from '../infrastructure/error.handler';
import { ToastService } from "../toast.service"
import { Subject } from "./subject"
import { SubjectDataService } from './subject.data.service';
@Component({
  selector: 'app-subjects',
  templateUrl: './subjects.component.html',
  styleUrls: ['./subjects.component.css'],
  providers: [SubjectDataService, ErrorHandler]
})
export class SubjectsComponent implements OnInit {
  subjects: Subject[] = [];
  currentSubject: Subject = new Subject();
  currentRow: number = -1;
  errors: any = {};
  search: string = "";
  page: number = 0;
  @ViewChild("subjectModal") subjectModal: ElementRef = new ElementRef("");
  @ViewChild("subjectsList") subjectsList: ElementRef = new ElementRef("");

  constructor(private dataService: SubjectDataService, private modalService: NgbModal,
    private toastService: ToastService, private errorHandler: ErrorHandler) { }

  NextPage() {
    this.page++;
    this.GetSubjects(this.page, this.search);
    this.ScrollUp();
  }
  PreviousPage() {
    this.page--;
    this.GetSubjects(this.page, this.search);
    this.ScrollUp();
  }
  Search() {
    this.page = 0;
    this.GetSubjects(-1, this.search);
    this.ScrollUp();
  }
  Reload() {
    this.page = 0;
    this.search = "";
    this.GetSubjects(this.page)
    this.ScrollUp();
  }
  ScrollUp() {
    this.subjectsList.nativeElement.scroll(0,0);
  }
  SaveSubject() {
    if (this.currentSubject.id == 0) {
      this.dataService.postSubject(this.currentSubject).subscribe(result => {
        this.errors = {};
        this.modalService.dismissAll();
        this.GetSubjects(this.page, this.search);
      }, error => {
        this.errors = this.errorHandler.Handle(error);
      });
    } else {
      this.dataService.putSubject(this.currentSubject).subscribe(result => {
        this.errors = {};
        this.modalService.dismissAll();
        this.GetSubjects(this.page, this.search);
      }, error => {
        this.errors = this.errorHandler.Handle(error);
      });
    }
  }
  public DblSetSubject(subject: Subject, i: number) {
    this.currentSubject = subject;
    this.currentRow = i;
    this.EditSubject();
  }
  public SetSubject(subject: Subject, i: number) {
    this.currentSubject = this.currentSubject == subject ? new Subject() : subject;
    this.currentRow = this.currentRow == i ? -1 : i;
  }
  public OpensubjectModal() {
    this.modalService.open(this.subjectModal, { size: "sm", centered: true }).result.then(() => { }, () => {
      this.currentSubject == new Subject();
      this.currentRow = -1;
      this.errors = {};
    });
  }
  public CreateSubject() {
    this.currentSubject = new Subject();
    this.OpensubjectModal();
  }
  public EditSubject() {
    if (this.currentSubject.id) {
      this.OpensubjectModal();
    }
  }
  public DeleteSubject() {
    this.dataService.deleteSubject(this.currentSubject.id).subscribe(result => {
      this.currentSubject == new Subject();
      this.currentRow = -1;
      this.GetSubjects(this.page, this.search);
    }, error => {
      this.errorHandler.Handle(error);
    })
  }
  GetSubjects(page: number,search: string = "") {
    this.dataService.getSubjects(search, page).subscribe(
      (data: any) => {
        this.subjects = data;
      },
      error => {
        this.errorHandler.Handle(error);
      });
  }
  ngOnInit(): void {
    this.GetSubjects(this.page);
  }
}




