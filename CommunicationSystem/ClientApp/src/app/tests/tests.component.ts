import { Component, ElementRef, OnInit } from '@angular/core';
import { Test } from './test';
import { TestsDataService } from "./tests.data.service"
import { AccountDataService } from "../account/account.data.service"
import { ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Question } from './question';
import { Option } from './option';

@Component({
  selector: 'app-tests',
  templateUrl: './tests.component.html',
  styleUrls: ['./tests.component.css'],
  providers: [TestsDataService]
})
export class TestsComponent implements OnInit {
  tests: Test[] = [];
  currentTest: Test = new Test();
  currentRow: number = -1;
  Timer: any = {};
  search: string = "";
  @ViewChild("testModal") testModal: ElementRef = new ElementRef("");
  constructor(private dataService: TestsDataService, private accountDataService: AccountDataService, private modalService: NgbModal) { }
  dblSet(test: any, i: number) {
    this.set(test, i, true);
    this.openTest();
  }
  set(test: any, i: number, dbl: boolean = false) {
    this.currentTest = this.currentTest == test && !dbl ? new Test() : test;
    this.currentRow = this.currentRow == i ? -1 : i;
  }

  openTest() {
    if (this.currentRow != -1) {
      this.modalService.open(this.testModal, { "size": "xl", "scrollable": true }).result.then(() => { }, () => {
        clearInterval(this.Timer);
        this.currentTest.time = this.currentTest.time / 60;
      });
      this.startTimer();
    }
  }
  submitTest() {
    this.dataService.postTest(this.currentTest.questionsList, this.accountDataService.currentAccount.id, this.currentTest.id).subscribe(() => {
      this.modalService.dismissAll();
      this.getTests();
    });
  }
  setAnswer(question: Question, option: Option) {
    if (question.questionType == 0) {
      question.studentAnswers[0] = option.id.toString();
    } else {
      var index = question.studentAnswers.indexOf(option.id.toString());
      if (index != -1) {
        question.studentAnswers.splice(index, 1);
      } else {
        question.studentAnswers.push(option.id.toString());
      }
    }
  }
  getTests() {
    this.dataService.getTests(this.accountDataService.currentAccount.id).subscribe((data: any) => {
      this.tests = data;
    })
  }
  startTimer() {
    this.currentTest.time = this.currentTest?.time * 60;
    this.Timer = setInterval(() => {
      this.currentTest.time--;
      if (this.currentTest.time == 0) {
        this.submitTest();
      }
    }, 1000);
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.getTests();
    }, 50)
  }

}
