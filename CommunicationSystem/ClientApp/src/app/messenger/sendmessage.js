"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SendMessage = void 0;
var SendMessage = /** @class */ (function () {
    function SendMessage(from, content, type, to, toGroup, isGroup, id) {
        if (content === void 0) { content = ""; }
        if (type === void 0) { type = 0; }
        if (toGroup === void 0) { toGroup = ""; }
        if (id === void 0) { id = 0; }
        this.from = from;
        this.content = content;
        this.type = type;
        this.to = to;
        this.toGroup = toGroup;
        this.isGroup = isGroup;
        this.id = id;
    }
    return SendMessage;
}());
exports.SendMessage = SendMessage;
//# sourceMappingURL=sendmessage.js.map