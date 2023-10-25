class User {

	userName;
	userType;
	location;

	constructor(userName, userType, location) {
		this.userName = userName;
		this.userType = userType;
		this.location = location;
	}

	get eligibleForPromotion() {
		return this.userType == 'Editor'
	}

	toString() {
		const me = this;
		return `User ${me.userName}, ${me.userType}, from ${me.location}`;
	}

}

function test() {

	const user = new User('Alice', 'Editor', 'Chicago');

	// User.prototype.toString = function userToString() {
	// 	const me = this
	// 	return `User ${me.userName}, ${me.userType}, from ${me.location}`
	// };

	console.log("console.log with user as simple arg");
	console.log(user);

	console.log("User embedded in string interpolation");
	console.log(`${user}`);

	console.log("Calling user.toString()");
	console.log(user.toString());
}

test()
