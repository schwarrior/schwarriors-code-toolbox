import { LogThis, LogLevel } from './log-helper.js';

export class DateFormatter {

    //Get the date in YYYY-MM-DD HH:MM:SS format
    //Run with no params to get for now
    //Or pass a JS Date Obj to be converted
    static getSqlDate = (jsDateObj = null) => {
        let d = new Date();
        if (jsDateObj && DateFormatter.isDateObj(jsDateObj)) {
            d = jsDateObj;
        }
        const dSql = d.getFullYear() +
            '-' + DateFormatter.pad(d.getMonth() + 1) +
            '-' + DateFormatter.pad(d.getDate()) +
            ' ' + DateFormatter.pad(d.getHours()) +
            ':' + DateFormatter.pad(d.getMinutes()) +
            ':' + DateFormatter.pad(d.getSeconds());
        return dSql;
    };

    static pad = (number) => {
        if (number < 10) {
            return '0' + number;
        }
        return number;
    }

    static isDateObj = (jsDate) => {
        const isD = jsDate instanceof Date && !isNaN(jsDate.valueOf());
        return isD;
    }

    static test = () => {
        const jsOldDateStr = "Fri Jun 11 2021 14:41:31 GMT-0400 (Eastern Daylight Time)";
        const jsOldDateObj = new Date(jsOldDateStr);
        LogThis(`isDateObj(): ${DateFormatter.isDateObj()}`, LogLevel.Debug);
        LogThis(`isDateObj(${jsOldDateStr}): ${DateFormatter.isDateObj(jsOldDateStr)}`, LogLevel.Debug);
        LogThis(`isDateObj(new Date(${jsOldDateObj})): ${DateFormatter.isDateObj(jsOldDateObj)}`, LogLevel.Debug);

        LogThis(`getSqlDateObj(new Date(${jsOldDateObj})): ${DateFormatter.getSqlDate(jsOldDateObj)}`, LogLevel.Debug);
        LogThis(`getSqlDateObj(): ${DateFormatter.getSqlDate()}`);
    }

}

// DateFormatter.test();
