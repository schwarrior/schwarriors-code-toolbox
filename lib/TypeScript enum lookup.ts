enum colors { red, green, blue };

// Will be converted essentially to this:

var colors = { red: 0, green: 1, blue: 2,
               [0]: "red", [1]: "green", [2]: "blue" }

// Because of this, the following will be true:

colors.red === 0
colors[colors.red] === "red"
colors["red"] === 0

// This creates a easy way to get the name of an enumerated as follows:

var color: colors = colors.red;
console.log("The color selected is " + colors[color]);

// It also creates a nice way to convert a string to an enumerated value.

var colorName: string = "green";
var color: colors = colors.red;
if (colorName in colors) color = colors[colorName];