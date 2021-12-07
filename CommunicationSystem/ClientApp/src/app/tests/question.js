"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Question = void 0;
var Question = /** @class */ (function () {
    function Question(id, options, text) {
        if (id === void 0) { id = 0; }
        if (options === void 0) { options = []; }
        this.id = id;
        this.options = options;
        this.text = text;
    }
    return Question;
}());
exports.Question = Question;
//# sourceMappingURL=question.js.map