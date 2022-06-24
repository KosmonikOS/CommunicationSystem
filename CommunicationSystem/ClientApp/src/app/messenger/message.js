"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Message = void 0;
var Message = /** @class */ (function () {
    function Message(id, isMine, toGroup, date, content, type, nickName, accountImage) {
        if (id === void 0) { id = 0; }
        if (isMine === void 0) { isMine = false; }
        if (toGroup === void 0) { toGroup = false; }
        if (content === void 0) { content = ""; }
        if (nickName === void 0) { nickName = ""; }
        if (accountImage === void 0) { accountImage = ""; }
        this.id = id;
        this.isMine = isMine;
        this.toGroup = toGroup;
        this.date = date;
        this.content = content;
        this.type = type;
        this.nickName = nickName;
        this.accountImage = accountImage;
    }
    return Message;
}());
exports.Message = Message;
//# sourceMappingURL=message.js.map