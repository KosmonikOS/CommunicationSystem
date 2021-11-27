"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SubjectDataService = void 0;
var SubjectDataService = /** @class */ (function () {
    function SubjectDataService(http) {
        this.http = http;
        this.url = "/api/subjects/";
    }
    ;
    SubjectDataService.prototype.getSubjects = function () {
        return this.http.get(this.url);
    };
    SubjectDataService.prototype.postSubject = function (subject) {
        return this.http.post(this.url, subject);
    };
    SubjectDataService.prototype.deleteSubject = function (id) {
        return this.http.delete(this.url + id);
    };
    return SubjectDataService;
}());
exports.SubjectDataService = SubjectDataService;
//# sourceMappingURL=subject.data.service.js.map