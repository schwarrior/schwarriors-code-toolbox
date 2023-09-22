export class DateFormatter {

	/**
	 * Get the date
	 * In format m/d/yyyy
	 * @param {Date | null} jsDateObj Date to convert. Omit or send null for now
	 * @return {string} formatted date
	 */
	static getFormattedDateString = (jsDateObj = null) => {
		const dtStr = this.getFormattedDateTimeString(jsDateObj)
		const dtParts = dtStr.split(' ')
		const dStr = dtParts[0]
		return dStr
	}
	
	/**
	 * Get the date and time
	 * In format m/d/yyyy h:mm pm
	 * @param {Date | null} jsDateObj Date to convert. Omit or send null for now
	 * @return {string} formatted date and time
	 */
	static getFormattedDateTimeString = (jsDateObj = null) => {
		let d = new Date();
		if (jsDateObj && this.isDateObj(jsDateObj)) {
			d = jsDateObj;
		}
		let hrs = d.getHours();
		let ampm = 'AM';
		if (hrs > 12 ) {
			hrs = hrs % 12;
			ampm = "PM";
		}
		const mins = this.pad(d.getMinutes());
		const dtStr = 
			(d.getMonth() + 1).toString() + '/' + d.getDate() + '/'+ d.getFullYear() + ' '
			+ hrs + ':' + mins + ' ' + ampm;
		return dtStr;
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
