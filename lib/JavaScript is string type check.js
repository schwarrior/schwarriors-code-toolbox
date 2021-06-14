function isString(val) {
    const isStr = (typeof val === 'string' || val instanceof String)
    return isStr
}

// alt typescript version
// private isString = (test: any): boolean => {
//     const isStr = (typeof test === 'string' || test instanceof String)
//     return isStr
// }