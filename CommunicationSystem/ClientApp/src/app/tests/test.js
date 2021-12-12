"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Test = void 0;
var Test = /** @class */ (function () {
    function Test(id, subject, subjectName, name, grade, questions, time, date, creator, creatorName, students, questionsList) {
        if (id === void 0) { id = 0; }
        if (time === void 0) { time = 40; }
        if (students === void 0) { students = []; }
        if (questionsList === void 0) { questionsList = []; }
        this.id = id;
        this.subject = subject;
        this.subjectName = subjectName;
        this.name = name;
        this.grade = grade;
        this.questions = questions;
        this.time = time;
        this.date = date;
        this.creator = creator;
        this.creatorName = creatorName;
        this.students = students;
        this.questionsList = questionsList;
    }
    ;
    return Test;
}());
exports.Test = Test;
//# sourceMappingURL=test.js.map