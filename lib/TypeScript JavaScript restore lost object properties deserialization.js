"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.enhanceUser = void 0;
var User = /** @class */ (function () {
    function User() {
    }
    Object.defineProperty(User.prototype, "eligibleForPromotion", {
        get: function () {
            return this.userType == 'Editor';
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(User.prototype, "summary", {
        get: function () {
            var me = this;
            return "User ".concat(me.userName, ", ").concat(me.userType, ", from ").concat(me.location, ")");
        },
        enumerable: false,
        configurable: true
    });
    return User;
}());
/**
 * 	When a user object arrives in the system from a web api or a sparsely defined JSON file
 * 	it is stripped of everything except those primitive storage properties provided in the JSON.
 * 	This enhancement method will new-up a full class and integrate the raw data
 * 	while restoring functions, getters, setters etc found in the full class definition
 * @param {string} rawUser - a sparsely defined user
 * @returns {string} - a fully defined user
 */
function enhanceUser(rawUser) {
    var u = new User();
    u.userName = rawUser.userName;
    u.userType = rawUser.userType;
    u.location = rawUser.location;
    return u;
}
exports.enhanceUser = enhanceUser;
function test() {
    // note the toString get property makes this impossible
    // 
    //@ts-ignore
    var sparseUser = {
        userName: 'Alice',
        userType: 'Editor',
        location: 'Chicago'
    };
    console.log("Calling sparseUser.summary. Expecting 'undefined'");
    try {
        console.log(sparseUser.summary);
        console.log();
        console.log("Calling sparseUser.eligibleForPromotion. Expecting 'undefined'");
        console.log(sparseUser.eligibleForPromotion);
        console.log();
    }
    catch (e) {
        console.error(e);
    }
    var fullUser = enhanceUser(sparseUser);
    console.log('Calling fullUser.summary. Expecting success');
    console.log(fullUser.summary);
    console.log();
    console.log('Calling fullUser.eligibleForPromotion. Expecting success');
    console.log(fullUser.eligibleForPromotion);
    console.log();
}
test();
