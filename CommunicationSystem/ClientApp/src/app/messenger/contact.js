"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Contact = void 0;
var guid_1 = require("../infrastructure/guid");
var Contact = /** @class */ (function () {
    function Contact(toId, toGroup, isGroup, nickName, accountImage, lastMessage, lastMessageDate, lastMessageType, notViewedMessages, newMessages, lastActivity) {
        if (toGroup === void 0) { toGroup = guid_1.Guid.Empty; }
        if (notViewedMessages === void 0) { notViewedMessages = 0; }
        if (newMessages === void 0) { newMessages = 0; }
        this.toId = toId;
        this.toGroup = toGroup;
        this.isGroup = isGroup;
        this.nickName = nickName;
        this.accountImage = accountImage;
        this.lastMessage = lastMessage;
        this.lastMessageDate = lastMessageDate;
        this.lastMessageType = lastMessageType;
        this.notViewedMessages = notViewedMessages;
        this.newMessages = newMessages;
        this.lastActivity = lastActivity;
    }
    return Contact;
}());
exports.Contact = Contact;
//# sourceMappingURL=contact.js.map