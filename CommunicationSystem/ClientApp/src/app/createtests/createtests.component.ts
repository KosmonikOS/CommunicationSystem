import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Test } from "../tests/test"
import { AccountDataService } from "../account/account.data.service"
import { CreatetestsDataService } from "./createtests.data.service"
import { Subject } from '../subjects/subject';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Question } from '../tests/question';
import { Option } from '../tests/option';
import { TestMember } from './testmember';
import { QuestionType } from "../tests/question"
import { UtilitesService } from '../utilites.service';
import { ErrorHandler } from '../infrastructure/error.handler';
import { Guid } from '../infrastructure/guid';
import { ToastService } from '../toast.service';

@Component({
  selector: 'app-createtests',
  templateUrl: './createtests.component.html',
  styleUrls: ['./createtests.component.css', '../app.component.css'],
  providers: [CreatetestsDataService, ErrorHandler]
})
export class CreatetestsComponent implements OnInit {
  tests: Test[] = [];
  currentTest: Test = new Test();
  currentRow: number = -1;
  currentQuestionRow: number = -1;
  currentOptionRow: number = -1;
  currentQuestion: Question = new Question();
  currentOption: Option = new Option();
  currentStudent: TestMember = new TestMember();
  currentStudentRow: number = -1;
  testStudents: TestMember[] = [];
  searchStudents: TestMember[] = [];
  fullStudentList: TestMember[] = [];
  subjects: Subject[] = [];
  errors: any = {};
  search: string = "";
  page: number = 0;
  questionAdding: boolean = true;
  optionAdding: boolean = true;
  @ViewChild("testModal") testModal: ElementRef = new ElementRef("");
  @ViewChild("questionModal") questionModal: ElementRef = new ElementRef("");
  @ViewChild("optionModal") optionModal: ElementRef = new ElementRef("");
  @ViewChild("studentModal") studentModal: ElementRef = new ElementRef("");
  @ViewChild("testsList") testsList: ElementRef = new ElementRef("")
  tempModals: ElementRef[] = [];
  questionTypes = QuestionType
  searchOptions = [
    { "id": 0, "name": "Название", "searchOn": "Поиск по названию" },
    { "id": 1, "name": "Предмет", "searchOn": "Поиск по предмету" },
    { "id": 2, "name": "Класс", "searchOn": "Поиск по классу" }
  ];
  currentSearchOption: number = 0;
  searchStudent: string = "";
  studentSearchOptions = [
    { "id": 0, "name": "ФИО", "searchOn": "Поиск по ФИО" },
    { "id": 1, "name": "Класс", "searchOn": "Поиск по классу" },
  ];
  currentStudentSearchOption: number = 0;

  constructor(private dataService: CreatetestsDataService, public accountDataService: AccountDataService,
    private modalService: NgbModal, private utilitesService: UtilitesService
    , private errorHandler: ErrorHandler, private toastService: ToastService) { }

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
    this.testStudents = [];
    this.searchStudents = [];
    this.ScrollUp();
  }
  ScrollUp() {
    this.testsList.nativeElement.scroll(0, 0);
  }

  DblSet(objectToSet: any, object: any, i: number) {
    this.Set(objectToSet, object, i, true);
    switch (objectToSet) {
      case "test":
        this.EditTest();
        break;
      case "question":
        this.EditQuestion();
        break;
      case "option":
        this.EditOption();
        break;
      case "student":
        this.OpenStudentAnswers()
        break;
    }
  }
  Set(objectToSet: string, object: any, i: number, dbl: boolean = false) {
    switch (objectToSet) {
      case "test":
        this.currentTest = this.currentRow == i && !dbl ? new Test() : object;
        this.currentRow = this.currentRow == i ? -1 : i;
        break;
      case "question":
        this.currentQuestion = this.currentQuestionRow == i && !dbl ? new Question() : object;
        this.currentQuestionRow = this.currentQuestionRow == i ? -1 : i;
        break;
      case "option":
        this.currentOption = this.currentOptionRow == i && !dbl ? new Option() : object;
        this.currentOptionRow = this.currentOptionRow == i ? -1 : i;
        break;
      case "student":
        this.currentStudent = this.currentStudentRow == i && !dbl ? new TestMember() : object;
        this.currentStudentRow = this.currentStudentRow == i ? -1 : i;
        break;
    }
  }
  FileSelected(event: any) {
    var file = <File>event.target.files[0];
    if (file !== undefined) {
      if (file.type.includes('image')) {
        this.utilitesService.postImage(file).subscribe((response: any) => {
          this.currentQuestion.image = response;
        }, error => this.errorHandler.Handle(error));
      } else {
        this.toastService.showAlert("Пожалуйста загрузите изображение");
      }
    }
  }
  OpenModal(modal: any, initial: boolean = true) {
    if (this.tempModals.length > 0) {
      this.modalService.dismissAll("background");
    }
    this.modalService.open(modal, { size: "xl", "scrollable": true }).result.then(() => { }, (reason) => {
      if (reason != "background") {
        let length = this.tempModals.length;
        if (length > 1) {
          this.OpenModal(this.tempModals[length - 2], false);
        }
        this.tempModals.splice(length - 1, 1);
      }
    });
    if (this.tempModals.length < 3 && initial) {
      this.tempModals.push(modal);
    }
  }
  OpenStudentAnswers() {
    if (this.currentStudent.testId !== undefined) {
      this.dataService.getAnswers(this.currentStudent.userId, this.currentTest.id).subscribe((data: any) => {
        this.currentStudent.answers = data;
        this.OpenModal(this.studentModal);
      })
    }
  }
  SaveStudent() {
    this.dataService.putMark(this.currentStudent)
      .subscribe(
        result => {
          this.modalService.dismissAll();
          this.currentStudent = new TestMember();
        }, error => { this.errors = this.errorHandler.Handle(error);})
  }
  CreateTest() {
    this.search = "";
    this.currentTest = new Test();
    this.testStudents = [];
    this.searchStudents = [];
    this.fullStudentList = [];
    this.OpenModal(this.testModal);
  }
  EditTest() {
    if (this.currentRow != -1) {
      this.GetQuestions();
      this.GetStudents();
      this.currentTest.students = [];
      this.OpenModal(this.testModal);
    }
  }
  DeleteTest() {
    if (!Guid.IsEmpty(this.currentTest.id)) {
      this.dataService.delete(this.currentTest.id, "test").subscribe(result => {
        this.GetTests(this.page, this.search);
      }, error => this.errorHandler.Handle(error));
    }
  }
  CreateQuestion() {
    this.questionAdding = true;
    this.currentQuestion = new Question();
    this.OpenModal(this.questionModal);
  }
  EditQuestion() {
    this.questionAdding = false;
    if (this.currentQuestionRow != -1) {
      this.OpenModal(this.questionModal);
    }
  }
  DeleteQuestion() {
    if (!Guid.IsEmpty(this.currentQuestion.id)) {
      this.dataService.delete(this.currentQuestion.id, "question")
        .subscribe(result => { },
          error => this.errorHandler.Handle(error));
    }
    this.currentTest.questions.splice(this.currentQuestionRow, 1);
  }
  CreateOption() {
    this.optionAdding = true;
    this.currentOption = new Option();
    this.OpenModal(this.optionModal);
  }
  EditOption() {
    this.optionAdding = false;
    if (this.currentOptionRow != -1) {
      this.OpenModal(this.optionModal);
    }
  }
  DeleteOption() {
    if (!Guid.IsEmpty(this.currentOption.id)) {
      this.dataService.delete(this.currentOption.id, "option").subscribe(result => { },
        error => this.errorHandler.Handle(error));
    }
    this.currentQuestion.options.splice(this.currentOptionRow, 1);
  }
  SaveQuestion() {
    this.currentQuestion.questionType = Number(this.currentQuestion.questionType);
    if (this.currentQuestion.questionType != 3) {
      if (this.currentQuestion.options.length == 0) {
        this.toastService.showError("Добавьте вариант ответа");
        return;
      }
      if (this.FindRightAnswer(this.currentQuestion) == "") {
        this.toastService.showError("Добавьте верный вариант ответа");
        return;
      }
    }
    if (!this.questionAdding) {
      this.currentTest.questions[this.currentQuestionRow] = this.currentQuestion;
    } else {
      this.currentTest.questions.push(this.currentQuestion);
    }
    this.currentQuestion = new Question();
    this.currentQuestionRow = -1;
    this.modalService.dismissAll();
  }
  SaveOption() {
    if (!this.optionAdding) {
      this.currentQuestion.options[this.currentOptionRow] = this.currentOption;
    } else {
      this.currentQuestion.options.push(this.currentOption);
    }
    this.currentOption = new Option();
    this.currentOptionRow = -1;
    this.modalService.dismissAll();
  }
  SaveTest() {
    this.currentTest.creator = this.accountDataService.currentAccount.id;
    this.currentTest.subject = Number(this.currentTest.subject);
    if (Guid.IsEmpty(this.currentTest.id)) {
      this.dataService.postTest(this.currentTest).subscribe(result => {
        this.modalService.dismissAll();
        this.GetTests(this.page, this.search);
      }, error => this.errors = this.errorHandler.HandleWitoutValidation(error));
    } else {
      this.dataService.putTest(this.currentTest).subscribe(result => {
        this.modalService.dismissAll();
        this.GetTests(this.page, this.search);
      }, error => this.errors = this.errorHandler.HandleWitoutValidation(error));
    }
    this.searchStudent = "";
  }
  FindRightAnswer(question: Question) {
    let answer = "";
    question.options.forEach((option, index) => {
      if (option.isRightOption) {
        answer += option.text + "  ";
      }
    });
    return answer;
  }
  GetTests(page: number, search: string = "") {
    this.dataService.getTests(this.accountDataService.currentAccount.id,
      this.accountDataService.currentAccount.role, page,
      this.currentSearchOption, search).subscribe(
        (data: any) => {
          this.tests = data;
          this.currentTest = new Test();
          this.currentRow = -1;
        }, error => this.errorHandler.Handle(error));
    this.errors = [];
  }
  GetQuestions() {
    this.dataService.getQuestions(this.currentTest.id).subscribe(
      (data: any) => {
        this.currentTest.questions = data;
      }, error => this.errorHandler.Handle(error));
  }
  GetStudents() {
    this.dataService.getStudents(this.currentTest.id).subscribe((data: any) => {
      this.testStudents = data;
      this.GetStudentsList();
    }, error => this.errorHandler.Handle(error));
  }
  SearchStudents() {
    if (this.searchStudent != "") {
      this.dataService.getSearchStudents(this.currentStudentSearchOption, this.searchStudent).subscribe((data: any) => {
        this.searchStudents = data;
        this.GetStudentsList();
      }, error => this.errorHandler.Handle(error));
    } else {
      this.searchStudents = [];
      this.GetStudentsList();
    }
  }
  GetStudentsList() {
    this.fullStudentList = [...new Map(this.searchStudents.concat(this.testStudents).map(item =>
      [item.userId, item])).values()];
  }
  ChangeStudentState(student: TestMember) {
    var index = this.currentTest.students.findIndex(s => s.userId == student.userId);
    if (student.testId !== undefined) {
      if (index != -1) {
        if (!student.isSelected) {
          this.currentTest.students[index].state = 3;
        } else {
          this.currentTest.students?.splice(index, 1);
        }
      } else {
        if (!student.isSelected) {
          student.state = 3;
          this.currentTest.students.push(student); //Проверить
        }
      }
    } else {
      if (student.isSelected) {
        student.state = 1;
        this.testStudents.unshift(student);
        this.currentTest.students.push(student);
      } else {
        var subindex = this.testStudents.findIndex(s => s.userId == student.userId);
        this.testStudents.splice(subindex, 1);
        this.currentTest.students?.splice(index, 1);
      }
      this.GetStudentsList();
    }
  }
  ChangeStudent(student: TestMember) {
    if (student.testId !== undefined && student.isSelected) {
      var entity = this.currentTest.students.find(s => s.userId == student.userId);
      if (entity !== undefined) {
        if (entity.state != 3) {
          entity.state = 2;
        }
      } else {
        student.state = 2;
        this.currentTest.students.push(student);
      }
    }
  }
  ngOnInit(): void {
    this.dataService.getSubjects().subscribe((data: any) => {
      this.subjects = data;
    }, error => this.errorHandler.Handle(error));
    this.GetTests(this.page);
  }

}
