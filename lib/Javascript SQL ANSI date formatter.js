export class DateFormatter {

    static getFormattedDateString = (jsDateObj = null) => {
		const dtStr = this.getFormattedDateTimeString(jsDateObj)
		const dtParts = dtStr.split(' ')
		const dStr = dtParts[0]
		return dStr
	}

	static getFormattedDateTimeString = (jsDateObj = null) => {
		let d = new Date();
		if (jsDateObj && this.isDateObj(jsDateObj)) {
			d = jsDateObj;
		}
		let hours = d.getHours()
		let ampm = 'AM'
		if (hours > 12 ) {
			hours = hours % 12
			ampm = "PM"
		}
		const dtStr = 
			(d.getMonth() + 1).toString() + '/' + d.getDate() + '/'+ d.getFullYear() + ' '
			+ hours + ':' + d.getMinutes() + ' ' + ampm
		return dtStr
	}
    
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
