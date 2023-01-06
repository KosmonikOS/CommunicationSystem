"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Guid = void 0;
var guid_typescript_1 = require("guid-typescript");
var Guid = /** @class */ (function () {
    function Guid() {
    }
    Guid.IsEmpty = function (guid) {
        return guid == this.Empty;
    };
    Guid.NewGuid = function () {
        return guid_typescript_1.Guid.create().toString();
    };
    Guid.Empty = "00000000-0000-0000-0000-000000000000";
    return Guid;
}());
exports.Guid = Guid;
//# sourceMappingURL=guid.js.map