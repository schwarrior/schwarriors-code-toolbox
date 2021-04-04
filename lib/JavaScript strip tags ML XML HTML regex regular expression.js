main = () => {
	console.log("Csharp strip tags ML XML HTML regex regular expression")
	console.log()
		
	const inString = "<?xml><a>hello<b atrib=\"0\">world</b></a>"
	console.log(inString)
	console.log(removeTags(inString))
}

removeTags = (mlInString) => {
	var result = mlInString.replace(/<\/?[^>]+(>|$)/g, " ")
    return result.trim()
}

main()