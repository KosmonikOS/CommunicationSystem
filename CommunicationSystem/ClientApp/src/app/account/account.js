"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Account = void 0;
var Account = /** @class */ (function () {
    function Account(id, password, email, nickName, firstName, middleName, lastName, grade, gradeLetter, role, roleName, accountImage, phone) {
        if (id === void 0) { id = 0; }
        if (role === void 0) { role = 1; }
        if (accountImage === void 0) { accountImage = "assets/user.png"; }
        this.id = id;
        this.password = password;
        this.email = email;
        this.nickName = nickName;
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
        this.grade = grade;
        this.gradeLetter = gradeLetter;
        this.role = role;
        this.roleName = roleName;
        this.accountImage = accountImage;
        this.phone = phone;
    }
    return Account;
}());
exports.Account = Account;
//# sourceMappingURL=account.js.map