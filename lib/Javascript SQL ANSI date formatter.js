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

}
