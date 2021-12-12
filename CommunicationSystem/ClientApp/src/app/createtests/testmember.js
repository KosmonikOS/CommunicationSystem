"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TestMember = void 0;
var TestMember = /** @class */ (function () {
    function TestMember(testId, userId, name, grade, isSelected, isCompleted, mark, id) {
        if (userId === void 0) { userId = 0; }
        if (isCompleted === void 0) { isCompleted = false; }
        if (mark === void 0) { mark = 0; }
        if (id === void 0) { id = 0; }
        this.testId = testId;
        this.userId = userId;
        this.name = name;
        this.grade = grade;
        this.isSelected = isSelected;
        this.isCompleted = isCompleted;
        this.mark = mark;
        this.id = id;
    }
    ;
    return TestMember;
}());
exports.TestMember = TestMember;
//# sourceMappingURL=testmember.js.map