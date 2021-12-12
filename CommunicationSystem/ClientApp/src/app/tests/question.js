"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Question = exports.QuestionType = void 0;
exports.QuestionType = [
    { "value": 0, "name": "Выбор 1 правильного варианта" },
    { "value": 1, "name": "Выбор нескольких правильных вариантов" },
    { "value": 2, "name": "Ввод ответа с проверкой" },
    { "value": 3, "name": "С открытым ответом" }
];
var Question = /** @class */ (function () {
    function Question(id, options, text, questionType, image, studentAnswers) {
        if (id === void 0) { id = 0; }
        if (options === void 0) { options = []; }
        if (questionType === void 0) { questionType = 0; }
        if (studentAnswers === void 0) { studentAnswers = []; }
        this.id = id;
        this.options = options;
        this.text = text;
        this.questionType = questionType;
        this.image = image;
        this.studentAnswers = studentAnswers;
    }
    return Question;
}());
exports.Question = Question;
//# sourceMappingURL=question.js.map