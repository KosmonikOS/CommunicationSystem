"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Member = void 0;
var Member = /** @class */ (function () {
    function Member(userId, state, nickName, accountImage, isSelected, isLinked) {
        if (state === void 0) { state = 3; }
        if (isLinked === void 0) { isLinked = false; }
        this.userId = userId;
        this.state = state;
        this.nickName = nickName;
        this.accountImage = accountImage;
        this.isSelected = isSelected;
        this.isLinked = isLinked;
    }
    return Member;
}());
exports.Member = Member;
//# sourceMappingURL=member.js.map