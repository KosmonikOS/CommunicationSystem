import { Component, ElementRef, OnInit } from '@angular/core';
import { Test } from './test';
import { TestsDataService } from "./tests.data.service"
import { AccountDataService } from "../account/account.data.service"
import { ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Question } from './question';
import { Option } from './option';
import { ErrorHandler } from '../infrastructure/error.handler';

@Component({
  selector: 'app-tests',
  templateUrl: './tests.component.html',
  styleUrls: ['./tests.component.css'],
  providers: [TestsDataService, ErrorHandler]
})
export class TestsComponent implements OnInit {
  tests: Test[] = [];
  currentTest: Test = new Test();
  currentRow: number = -1;
  Timer: any = {};
  search: string = "";
  page: number = 0;
  @ViewChild("testModal") testModal: ElementRef = new ElementRef("");
  @ViewChild("testsList") testsList: ElementRef = new ElementRef("")
  searchOptions = [
    { "id": 0, "name": "Название", "searchOn": "Поиск по названию" },
    { "id": 1, "name": "Предмет", "searchOn": "Поиск по предмету" },
  ];
  currentSearchOption: number = 0;

  constructor(private dataService: TestsDataService, private accountDataService: AccountDataService
    , private modalService: NgbModal, private errorHandler: ErrorHandler) { }

  NextPage() {
    this.page++;
    this.GetTests(this.page, this.search);
    this.ScrollUp();
  }
  PreviousPage() {
    this.page--;
    this.GetTests(this.page, this.search);
    this.ScrollUp();
  }
  Search() {
    this.page = 0;
    this.GetTests(-1, this.search);
    this.ScrollUp();
  }
  Reload() {
    this.page = 0;
    this.search = "";
    this.GetTests(this.page)
    this.ScrollUp();
  }
  ScrollUp() {
    this.testsList.nativeElement.scroll(0, 0);
  }

  DblSet(test: any, i: number) {
    this.Set(test, i, true);
    this.OpenTest();
  }
  Set(test: any, i: number, dbl: boolean = false) {
    this.currentTest = this.currentRow == i && !dbl ? new Test() : test;
    this.currentRow = this.currentRow == i ? -1 : i;
  }
  OpenTest() {
    if (this.currentRow != -1) {
      this.GetQuestions();
      this.modalService.open(this.testModal, { "size": "xl", "scrollable": true }).result.then(() => { }, () => {
        clearInterval(this.Timer);
        this.currentTest.time = this.currentTest.time / 60;
      });
      this.StartTimer();
    }
  }
  SubmitTest() {
    this.dataService.postTest(this.currentTest.questions, this.accountDataService.currentAccount.id, this.currentTest.id).subscribe(() => {
      this.modalService.dismissAll();
      this.GetTests(this.page, this.search);
    });
  }
  SetAnswer(question: Question, option: Option) {
    if (question.questionType == 0) {
      question.answers[0] = option.id.toString();
    } else {
      var index = question.answers.indexOf(option.id.toString());
      if (index != -1) {
        question.answers.splice(index, 1);
      } else {
        question.answers.push(option.id.toString());
      }
    }
  }
  GetTests(page: number, search: string = "") {
    this.dataService.getTests(this.accountDataService.currentAccount.id,
      page, this.currentSearchOption, search).subscribe((data: any) => {
        this.tests = data;
      }, error => this.errorHandler.Handle(error))
  }
  GetQuestions() {
    this.dataService.getQuestions(this.currentTest.id).subscribe(
      (data: any) => {
        this.currentTest.questions = data.map((x: Question) => {
          x.answers = [];
          return x
        });
      }, error => this.errorHandler.Handle(error));
  }
  StartTimer() {
    this.currentTest.time = this.currentTest?.time * 60;
    this.Timer = setInterval(() => {
      this.currentTest.time--;
      if (this.currentTest.time == 0) {
        this.SubmitTest();
      }
    }, 1000);
  }

  ngOnInit(): void {
    if (this.accountDataService.currentAccount.id != 0) {
      this.GetTests(this.page);
    } else {
      setTimeout(() => {
        this.GetTests(this.page);
      }, 500);
    }

  }

}
