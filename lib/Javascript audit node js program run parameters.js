// import * as os from 'node:os';
// import * as fs from 'node:fs';

const os = require('os');
const fs = require('fs');

function logRunParameters() {   
	const startCmd = arrayToString(process.argv);
	console.log(startCmd);
	const nodeVersion = process.version;
	console.log(`Node.js ${nodeVersion}`);
    try
    {
        const pckgRaw = fs.readFileSync('./package.json');
        const pckg = JSON.parse(pckgRaw);
        const npmName = pckg.name;
        const npmVersion = pckg.version;
        console.log(`${npmName} v${npmVersion}`);
    }
    catch
    {
        console.log('No NPM package found.');
    }
	const envName = process.env['ENVIRONMENT'];
    if (envName) {console.log(`Running in environment: ${envName}`);}
	const host = os.hostname();
	console.log(`Running on ${host}`);
	const logPath = process.env['LOG'];
    if (envName) {console.log(`Logging to ${logPath}`);}
}

function arrayToString(inArray, delimitter = ' ') {
	let s = '';
	if (!Array.isArray(inArray)) {
		return inArray??s.toString();
	}
	for (let i = 0; i < inArray.length; i++) {
		if (i > 0) { s = s + delimitter; }
		s = s + inArray[i].toString();
	}
	return s;
}

// test
logRunParameters();

// export {
// 	logRunParameters
// }