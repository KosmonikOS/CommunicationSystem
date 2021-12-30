"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.UserLastMessage = void 0;
var UserLastMessage = /** @class */ (function () {
    function UserLastMessage(id, nickName, accountImage, email, messageId, from, to, content, notViewed, date, userActivity) {
        this.id = id;
        this.nickName = nickName;
        this.accountImage = accountImage;
        this.email = email;
        this.messageId = messageId;
        this.from = from;
        this.to = to;
        this.content = content;
        this.notViewed = notViewed;
        this.date = date;
        this.userActivity = userActivity;
    }
    return UserLastMessage;
}());
exports.UserLastMessage = UserLastMessage;
//# sourceMappingURL=userlastmessage.js.map