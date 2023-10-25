type UserType = 'Admin' | 'Editor' | 'Reader'

class User {

	userName: string
	userType: UserType
	location: string

	get eligibleForPromotion(): boolean {
		return this.userType == 'Editor'
	}

	get summary(): string {
		const me = this
		return `User ${me.userName}, ${me.userType}, from ${me.location})`
	} 

}

/**
 * 	When a user object arrives in the system from a web api or a sparsely defined JSON file
 * 	it is stripped of everything except those primitive storage properties provided in the JSON.
 * 	This enhancement method will new-up a full class and integrate the raw data
 * 	while restoring functions, getters, setters etc found in the full class definition
 * @param {string} rawUser - a sparsely defined user 
 * @returns {string} - a fully defined user
 */
export function enhanceUser(rawUser: User): User {

	const u = new User()
			
	u.userName		= rawUser.userName							
	u.userType		= rawUser.userType
	u.location		= rawUser.location
	
	return u
}

function test() {

	// note the toString get property makes this impossible
	// 
	//@ts-ignore
	const sparseUser: User = {
		userName: 'Alice',
		userType: 'Editor',
		location: 'Chicago'
	}

	console.log("Calling sparseUser.summary. Expecting 'undefined'")
	try {
		console.log(sparseUser.summary)
		console.log()
		console.log("Calling sparseUser.eligibleForPromotion. Expecting 'undefined'")
		console.log(sparseUser.eligibleForPromotion)
		console.log()
	} catch (e) {
		console.error(e)
	}
    
    var fullUser = enhanceUser(sparseUser)
	console.log('Calling fullUser.summary. Expecting success')
    console.log(fullUser.summary)
    console.log()
	console.log('Calling fullUser.eligibleForPromotion. Expecting success')
    console.log(fullUser.eligibleForPromotion)
    console.log()

}

test()