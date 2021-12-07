"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Option = void 0;
var Option = /** @class */ (function () {
    function Option(id, text, isRightOption) {
        if (id === void 0) { id = 0; }
        if (isRightOption === void 0) { isRightOption = false; }
        this.id = id;
        this.text = text;
        this.isRightOption = isRightOption;
    }
    return Option;
}());
exports.Option = Option;
//# sourceMappingURL=option.js.map