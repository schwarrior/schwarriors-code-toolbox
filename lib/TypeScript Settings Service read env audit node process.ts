import * as os from 'node:os';
import * as fs from 'node:fs';
import * as env from 'dotenv';

// Remark: none of the above references are compatable with a web app / angular

export interface Settings {
	ENVIRONMENT: string | null,
	LOG: string | null,
	AzureTableAcctName: string | null,
	AzureTableAcctKey: string | null
}

export class SettingsService {

	settings: Settings = {
		ENVIRONMENT: null,
		LOG: null,
		AzureTableAcctName: null,
		AzureTableAcctKey: null
	};

	constructor() {
		env.config();
		for(const expectedKey in this.settings) {
			const value = process.env[expectedKey];
			if (!value) {
				console.warn(`No value found for expected .env key ${expectedKey}`);
				continue;
			} 
			(this.settings as any)[expectedKey] = process.env[expectedKey];
		}
	}

	getSqlDbConfig() : object {
		throw new Error('SQL DB config not set up for this project');
		const dbConfig = {
			user: (this.settings as any)['dbUser'],
			password: (this.settings as any)['dbPassword'],
			server: (this.settings as any)['dbServer'],
			database: (this.settings as any)['dbDatabase'],
			connectionTimeout: 30000, //30 seconds
			requestTimeout: 1800000, //30 minutes
			options: {enableArithAbort: true, trustServerCertificate: true}
		}
		return dbConfig;
	}
	
	getAzureTableStoreConnString = () : string => {
		throw new Error('Azure Table Store not set up for this project');
		const acctName = (this.settings as any)['AzureTableStoreAcctName'];
		const acctKey = (this.settings as any)['AzureTableStoreAcctKey'];
		const connStr = `DefaultEndpointsProtocol=https;AccountName=${acctName};AccountKey=${acctKey};EndpointSuffix=core.windows.net`;
		return connStr;
	}
	
	getProgramAudit = (withConsoleLog = true) : string[] => {
		const auditLines = this.getGeneralNodeAppAuditLines();
		const appSpecificAuditLines = this.getAppSpecificAuditLines();
		auditLines.push(...appSpecificAuditLines);
		if (!withConsoleLog) {return auditLines;}
		auditLines.forEach(auditLine => {
			console.log(auditLine)
		});
		return auditLines;
	}
	
	getGeneralNodeAppAuditLines = () : string[] => {
		const auditLines = new Array<string>();
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

	getAppSpecificAuditLines = () : string[] => {
		const auditLines = new Array<string>();
		for(const expectedKey in this.settings) {
			auditLines.push(this.getEnvSettingAuditLine(expectedKey));
		}
		return auditLines;
	}

	getEnvSettingAuditLine = (settingName: string) : string => {
		const value = (this.settings as any)[settingName];
		if (!value) {
			return `.env ${settingName} value not found`;
		}
		return `.env ${settingName}: ${value}`;
	} 

	stringToArray = (inStr: string) : string[] => {
		const outAr = new Array<string>();
		if (inStr && inStr.length > 0) {
			const inStrParts = inStr.split(',');
			for(let strPart of inStrParts) {
				outAr.push(strPart)
			}
		}
		return outAr
	}
	
	arrayToString = (inArray: any[], delimitter = ' '): string => {
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
