import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Test } from "../tests/test"
import { AccountDataService } from "../account/account.data.service"
import { CreatetestsDataService } from "./createtests.data.service"
import { Subject } from '../subjects/subject';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Question } from '../tests/question';
import { Option } from '../tests/option';
import { ToastService } from '../toast.service';
import { TestMember } from './testmember';
import { QuestionType } from "../tests/question"

@Component({
  selector: 'app-createtests',
  templateUrl: './createtests.component.html',
  styleUrls: ['./createtests.component.css', '../app.component.css'],
  providers: [CreatetestsDataService, ToastService]
})
export class CreatetestsComponent implements OnInit {
  tests: Test[] = [];
  currentTest: Test = new Test();
  currentRow: number = -1;
  currentQuestionRow: number = -1;
  currentOptionRow: number = -1;
  currentQuestion: Question = new Question();
  currentOption: Option = new Option();
  userList: TestMember[] = [];
  subjects: Subject[] = [];
  errors: any = {};
  search: string = "";
  searchUsers: string = "";
  @ViewChild("testModal") testModal: ElementRef = new ElementRef("");
  @ViewChild("questionModal") questionModal: ElementRef = new ElementRef("");
  @ViewChild("optionModal") optionModal: ElementRef = new ElementRef("");
  tempModals: ElementRef[] = [];
  questionTypes = QuestionType
  constructor(private dataService: CreatetestsDataService, public accountDataService: AccountDataService, private modalService: NgbModal, private toastService: ToastService) { }

  public dblSet(objectToSet: any, object: any, i: number) {
    this.set(objectToSet, object, i, true);
    switch (objectToSet) {
      case "test":
        this.editTest();
        break;
      case "question":
        this.editQuestion();
        break;
      case "option":
        this.editOption();
        break;
    }
  }
  public set(objectToSet: string, object: any, i: number, dbl: boolean = false) {
    switch (objectToSet) {
      case "test":
        this.currentTest = this.currentTest == object && !dbl ? new Test() : object;
        this.currentRow = this.currentRow == i ? -1 : i;
        break;
      case "question":
        this.currentQuestion = this.currentQuestion == object && !dbl ? new Question() : object;
        this.currentQuestionRow = this.currentQuestionRow == i ? -1 : i;
        break;
      case "option":
        this.currentOption = this.currentOption == object && !dbl ? new Option() : object;
        this.currentOptionRow = this.currentOptionRow == i ? -1 : i;
        break;
    }
  }
  fileSelected(event: any) {
    var file = <File>event.target.files[0];
  }
  openModal(modal: any, initial: boolean = true) {
    if (this.tempModals.length > 0) {
      this.modalService.dismissAll("background");
    }
    this.modalService.open(modal, { size: "xl" }).result.then(() => { }, (reason) => {
      if (reason != "background") {
        let length = this.tempModals.length;
        if (length > 1) {
          this.openModal(this.tempModals[length - 2], false);
        }
        this.tempModals.splice(length - 1, 1);
      }
    });
    if (this.tempModals.length < 3 && initial) {
      this.tempModals.push(modal);
    }
  }
  createTest() {
    this.search = "";
    this.currentTest = new Test(-1);
    this.openModal(this.testModal);
  }
  editTest() {
    if (this.currentRow != -1) {
      this.openModal(this.testModal);
      this.userList = this.currentTest.students;
      console.log(this.userList);
    }
  }
  deleteTest() {
    if (this.currentTest.id > 0) {
      this.dataService.delete(this.currentTest.id, "test").subscribe(() => {
        this.getTests();
      });
    }
  }
  createQuestion() {
    this.search = "";
    this.currentQuestion = new Question(-1);
    this.openModal(this.questionModal);
  }
  editQuestion() {
    if (this.currentQuestionRow != -1) {
      this.openModal(this.questionModal);
    }
  }
  deleteQuestion() {
    if (this.currentQuestion.id > 0) {
      this.dataService.delete(this.currentQuestion.id, "question").subscribe(() => { });
    }
    var index = this.currentTest.questionsList.findIndex(q => q.id == this.currentQuestion.id)
    this.currentTest.questionsList.splice(index, 1);
  }
  createOption() {
    this.currentOption = new Option(-1);
    this.openModal(this.optionModal);
  }
  editOption() {
    if (this.currentOptionRow != -1) {
      this.openModal(this.optionModal);
    }
  }
  deleteOption() {
    if (this.currentOption.id > 0) {
      this.dataService.delete(this.currentOption.id, "option").subscribe(() => { });
    }
    var index = this.currentQuestion.options.findIndex(o => o.id == this.currentOption.id)
    this.currentQuestion.options.splice(index, 1);
  }
  saveQuestion() {
    this.currentQuestion.questionType = Number(this.currentQuestion.questionType);
    if (this.currentQuestion.id != -1) {
      this.currentTest.questionsList[this.currentTest.questionsList.findIndex(q => q.id == this.currentQuestion.id)] = this.currentQuestion;
    } else {
      this.currentQuestion.id = Math.round((Math.random() * -1000));
      this.currentTest.questionsList.push(this.currentQuestion);
      this.currentTest.questions = this.currentTest.questionsList.length;
    }
    this.modalService.dismissAll();
  }
  saveOption() {
    if (this.currentOption.id != -1) {
      this.currentQuestion.options[this.currentQuestion.options.findIndex(o => o.id == this.currentOption.id)] = this.currentOption;
    } else {
      this.currentOption.id = Math.round((Math.random() * -1000));
      this.currentQuestion.options.push(this.currentOption);
    }
    this.modalService.dismissAll();
  }
  saveTest() {
    this.currentTest.creator = this.accountDataService.currentAccount.id;
    this.currentTest.questions = this.currentTest.questionsList.length;
    this.currentTest.subject = Number(this.currentTest.subject);
    this.dataService.postTest(this.currentTest).subscribe(() => {
      this.modalService.dismissAll();
      this.getTests();
    }, () => {
      this.toastService.showError("Ошибка сохранения");
    })
  }
  findRightAnswer(question: Question) {
    let answer = "";
    question.options.forEach((option, index) => {
      if (option.isRightOption) {
        answer += option.text + "  ";
      }
    });
    return answer;
  }
  getTests() {
    this.dataService.getTests(this.accountDataService.currentAccount.id).subscribe((data: any) => {
      this.tests = data;
      console.log(data);
    });
  }
  getUsers() {
    this.dataService.getUsers(this.searchUsers).subscribe((data: any) => {
      this.userList = this.currentTest.students.concat(data);
    })
  }
  addTestMember(user: TestMember) {
    if (!user.isSelected) {
      var index = this.currentTest.students.findIndex(s => s.userId == user.id);
      this.currentTest.students?.splice(index, 1);
    } else {
      this.currentTest.students?.push(new TestMember(this.currentTest.id, user.userId, user.name, user.grade, user.isSelected));
    }
  }
  ngOnInit(): void {
    this.dataService.getSubjects().subscribe((data: any) => {
      this.subjects = data;
    })
    this.getTests();
  }

}
