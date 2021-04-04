//DateManager requires dateformat and @types/dateformat
//npm i -S dateformat
//npm i -S @types/dateformat
//http://stevenlevithan.com/assets/misc/date.format.js

import dateFormat = require("dateformat");

export class DateManager {
    static nowToUsEstIsoDateTimeString() : string {
        return DateManager.dateToUsEstIsoDateTimeString(new Date())
    }

    static dateToUsEstIsoDateTimeString(date : Date) : string {
        //azure presents dates as in a UTC timezone (which is bizarre cuz no such thing)
        //convert to EST
        const estTimeStr = date.toLocaleString("en-US", {timeZone: "America/New_York"});
        const estTime = new Date(estTimeStr);
        //eg 2019-04-25 15:23:57.884
        return dateFormat(date, "yyyy-mm-dd HH:MM:ss.l")
    }
}