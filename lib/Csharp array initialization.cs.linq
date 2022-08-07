<Query Kind="Statements" />

var arA = new string[2]; // creates array of length 2, having default string value of null for each member
var arB = new string[] { "A", "B" }; 

// the followig forms don't allow implicit variable typing with "var"
string[] arC = { "A" , "B" }; 
string[] arD = new[] { "A", "B" }; 