import * as env from 'dotenv';
import * as os from 'node:os';
import * as fs from 'node:fs'

export class SettingService {

	expectedEnvKeys = ['ENVIRONMENT','LOG','','AzureTableAcctName','AzureTableAcctKey'];

	settings = new Object();

	constructor() {
		env.config();
		const settingsCount = process.env.length;
		let adjExpectKeys = this.expectedEnvKeys;
		if (!settingsCount < 1) {
			console.warn('Environment settings file not found. Please create a .env file at root of project.');
			adjExpectKeys = [];
		}
		for(const expectedKey of adjExpectKeys) {
			const value = process.env[expectedKey];
			if (!value) {
				console.warn(`No value found for expected .env key ${expectedKey}`);
				continue;
			} 
			settings[expectedKey] = process.env[expectedKey];
		}
	}

	getSqlDbConfig() {
		throw new Error('SQL DB config not set up for this project');
		const dbConfig = {
			user: process.env['dbUser'],
			password: process.env['dbPassword'],
			server: process.env['dbServer'],
			database: process.env['dbDatabase'],
			connectionTimeout: 30000, //30 seconds
			requestTimeout: 1800000, //30 minutes
			options: {enableArithAbort: true, trustServerCertificate: true}
		}
		return dbConfig;
	}
	
	getAzureTableStoreConnString = () => {
		throw new Error('Azure Table Store not set up for this project');
		const acctName = settings['AzureTableStoreAcctName'];
		const acctKey = settings['AzureTableStoreAcctKey'];
		const connStr = `DefaultEndpointsProtocol=https;AccountName=${acctName};AccountKey=${acctKey};EndpointSuffix=core.windows.net`;
		return connStr;
	}
	
	getProgramAudit = (withConsoleLog = true) => {
		let auditLines = this.getGeneralNodeAppAuditLines();
		let appSpecificAuditLines = this.getAppSpecificAuditLines();
		auditLines.push(appSpecificAuditLines);
		if (!withConsoleLog) {return auditLines;}
		auditLines.forEach(auditLine => {
			console.log(auditLine)
		});
		return auditLines;
	}
	
	getGeneralNodeAppAuditLines = () => {
		auditLines = [];
		const startCmd = arrayToString(process.argv);
		auditLines.push(`Running: ${startCmd}`);
		const host = os.hostname();
		auditLines.push(`Running on: ${host}`);
		const nodeVersion = process.version;
		auditLines.push(`Node.js version: ${nodeVersion}`);
		const cwd = process.cwd;
		auditLines.push(`Current working dir: ${cwd}`);

		const pckgFound = fs.existsSync('./package.json');
		if (!pckgFound) {
			auditLines.push('No package.json file found in working dir');
			return auditLines;
		}
		const pckgRaw = fs.readFileSync('./package.json');
		const pckg = JSON.parse(pckgRaw);
		const npmName = pckg.name;
		auditLines.push(`NPM package name: ${npmName}`);
		const npmVersion = pckg.version;
		auditLines.push(`NPM package version${npmVersion}`);
		return auditLines;
	}

	getAppSpecificAuditLines = () => {
		auditLines = [];
		if (this.settings.length < 1) {
			auditLines.push('No .env file or empty .env file found in working dir');
			return auditLines;
		}
		for(const expectedKey of this.expectedEnvKeys) {
			auditLines.push(this.getEnvSettingAuditLine(expectedKey));
		}
		return auditLines;
	}

	getEnvSettingAuditLine = (settingName) => {
		const value = this.settings[settingName];
		if (!value) {
			return `.env ${settingName} value not found`;
		}
		return `.env ${settingName}: ${value}`;
	} 

	stringToArray = (inStr) => {
		const outAr = []
		if (inStr && inStr.length > 0) {
			const inStrParts = inStr.split(',');
			for(let strPart of inStrParts) {
				outAr.push(strPart)
			}
		}
		return outAr
	}
	
	arrayToString = (inArray, delimitter = ' ') => {
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
}
