
// In JavaScript, a primitive (primitive value, primitive data type) 
// is data that is not an object and has no methods. 
// There are 7 primitive data types: 
// string, number, bigint, boolean, undefined, symbol, and null.

// All primitives are immutable

// Watch out of uppercase type names
// Except for null and undefined, all primitives have object equivants that wrap primitive values
// String, Number, BigInt, Boolean, Symbol
// valueOf() returns the primitive value

function isPrimitive(test) {
    return test !== Object(test);
}

function isObject(test) {
    return test === Object(test);
}
