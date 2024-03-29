import * as env from 'dotenv';
import * as os from 'node:os';
import * as fs from 'node:fs'

export class SettingService {

	expectedEnvKeys = ['ENVIRONMENT','LOG','','AzureTableAcctName','AzureTableAcctKey'];

	settings = new Object();

	constructor() {
		env.config();
		for(const expectedKey in this.settings) {
			const value = process.env[expectedKey];
			if (!value) {
				console.warn(`No value found for expected .env key ${expectedKey}`);
				continue;
			} 
			this.settings[expectedKey] = process.env[expectedKey];
		}
	}

	getSqlDbConfig() {
		throw new Error('SQL DB config not set up for this project');
		const dbConfig = {
			user: this.settings['dbUser'],
			password: this.settings['dbPassword'],
			server: this.settings['dbServer'],
			database: this.settings['dbDatabase'],
			connectionTimeout: 30000, //30 seconds
			requestTimeout: 1800000, //30 minutes
			options: {enableArithAbort: true, trustServerCertificate: true}
		}
		return dbConfig;
	}
	
	getAzureTableStoreConnString = () => {
		throw new Error('Azure Table Store not set up for this project');
		const acctName = this.settings['AzureTableStoreAcctName'];
		const acctKey = this.settings['AzureTableStoreAcctKey'];
		const connStr = `DefaultEndpointsProtocol=https;AccountName=${acctName};AccountKey=${acctKey};EndpointSuffix=core.windows.net`;
		return connStr;
	}
	
	getProgramAudit = (withConsoleLog = true) => {
		const auditLines = this.getGeneralNodeAppAuditLines();
		const appSpecificAuditLines = this.getAppSpecificAuditLines();
		auditLines.push(...appSpecificAuditLines);
		if (!withConsoleLog) {return auditLines;}
		auditLines.forEach(auditLine => {
			console.log(auditLine)
		});
		return auditLines;
	}
	
	getGeneralNodeAppAuditLines = () => {
		const auditLines = [];
		const startCmd = this.arrayToString(process.argv);
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
		const pckg = JSON.parse(pckgRaw.toString());
		const npmName = pckg.name;
		auditLines.push(`NPM package name: ${npmName}`);
		const npmVersion = pckg.version;
		auditLines.push(`NPM package version: ${npmVersion}`);
		return auditLines;
	}

	getAppSpecificAuditLines = () => {
		const auditLines = [];
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
