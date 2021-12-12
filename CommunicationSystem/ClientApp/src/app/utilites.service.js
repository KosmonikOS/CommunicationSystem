"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.UtilitesService = void 0;
var UtilitesService = /** @class */ (function () {
    function UtilitesService(http) {
        this.http = http;
        this.url = "utilites/";
    }
    UtilitesService.prototype.putImage = function (image) {
        var formData = new FormData();
        formData.append("ImageToSave", image);
        return this.http.put(this.url + "saveimage", formData);
    };
    return UtilitesService;
}());
exports.UtilitesService = UtilitesService;
//# sourceMappingURL=utilites.service.js.map