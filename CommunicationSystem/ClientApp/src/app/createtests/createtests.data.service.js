"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CreatetestsDataService = void 0;
var CreatetestsDataService = /** @class */ (function () {
    function CreatetestsDataService(http) {
        this.http = http;
        this.url = "/api/createtests/";
    }
    CreatetestsDataService.prototype.getTests = function (id) {
        return this.http.get(this.url + id);
    };
    CreatetestsDataService.prototype.getSubjects = function () {
        return this.http.get("/api/subjects");
    };
    return CreatetestsDataService;
}());
exports.CreatetestsDataService = CreatetestsDataService;
//# sourceMappingURL=createtests.data.service.js.map