import * as env from 'dotenv';
import { DateFormatter } from './date-formatter.js';

const LogLevel = {
    Trace: 0,
    Debug: 1,
    Information: 2,
    Warning: 3,
    Error: 4,
    Critical: 5,
    None: 6
}

function LogThis(message, level = LogLevel.Information){
    const logDate = DateFormatter.getSqlDate();
    const levelName = Object.keys(LogLevel)[level];
    const levelNamePadded = levelName.padStart('Information'.length, ' ');
    var fullMsg = '';
	if (level === LogLevel.Trace) {
		fullMsg = `${logDate}: ${levelNamePadded}: ${message}`;
	}
	const filterLogLevel = getFilterLogLevel();
    switch(level) {
        case LogLevel.Critical:
			if (filterLogLevel > LogLevel.Critical) { return; }
			console.error(fullMsg);
			break;
        case LogLevel.Error:
			if (filterLogLevel > LogLevel.Error) { return; }
            console.error(fullMsg);
            break;
        case LogLevel.Warning:
			if (filterLogLevel > LogLevel.Warning) { return; }
            console.warn(fullMsg);
            break;
		case LogLevel.Information:
			if (filterLogLevel > LogLevel.Information) { return; }
			console.log(fullMsg);
			break;
		case LogLevel.Debug:
			if (filterLogLevel > LogLevel.Debug) { return; }
			console.log(fullMsg);
			break;
		case LogLevel.Trace:
			if (filterLogLevel > LogLevel.Trace) { return; }
			console.dir(message);
			break;	
		case LogLevel.None:	
		default:
            return;	
    }
}

var _filterLogLevel = -1;

function getFilterLogLevel(){
	if ( _filterLogLevel > -1 ) {
		return _filterLogLevel;
	} else {
		env.config();
		const envLogLevel = process.env['logLevel'];
		_filterLogLevel = envLogLevel ?? 2;
		return _filterLogLevel;
	}
}

export {
	LogLevel,
	LogThis
}