'use strict';

//Get the date and time in YYYY-MM-DD HH:MM:SS format
//Run with no params to get for now
//Or pass a JS Date Obj to be converted
const getSqlDateTime =  function(jsDateObj = null, daysAdd = null) {
    let d = new Date();
    if (jsDateObj && isDateObj(jsDateObj)) {
        d = jsDateObj;
    }
    if (daysAdd) {
        d = addDays(d, daysAdd);
    }
    const dSql = d.getFullYear() +
        '-' + pad(d.getMonth() + 1) +
        '-' + pad(d.getDate()) +
        ' ' + pad(d.getHours()) +
        ':' + pad(d.getMinutes()) +
        ':' + pad(d.getSeconds());
    return dSql;
};

//Get the date only in YYYY-MM-DD format
//Run with no params to get for now
//Or pass a JS Date Obj to be converted
const getSqlDate =  function(jsDateObj = null, daysAdd = null) {
    const sqlD = getSqlDateTime(jsDateObj, daysAdd);
    const sqlDParts = sqlD.split(' ');
    const dateOnly = sqlDParts[0];
    return dateOnly;
}

const pad = function(number) {
    if (number < 10) {
        return '0' + number;
    }
    return number;
}

const isDateObj = function(jsDate) {
    const isD = jsDate instanceof Date && !isNaN(jsDate.valueOf());
    return isD;

}

const addDays = function(jsDateObj, days) {
    var paranoidDateObj = new Date(jsDateObj.valueOf());
    paranoidDateObj.setDate(paranoidDateObj.getDate() + days);
    return paranoidDateObj;
}



const test = function() {
    const jsOldDateStr = "Fri Jun 11 2021 14:41:31 GMT-0400 (Eastern Daylight Time)";
    const jsOldDateObj = new Date(jsOldDateStr);
    console.log(`isDateObj(): ${isDateObj()}`);
    console.log(`isDateObj(${jsOldDateStr}): ${isDateObj(jsOldDateStr)}`);
    console.log(`isDateObj(new Date(${jsOldDateObj})): ${isDateObj(jsOldDateObj)}`);
    
    console.log(`getSqlDateTime(new Date(${jsOldDateObj})): ${getSqlDateTime(jsOldDateObj)}`);
    console.log(`getSqlDateTime(): ${getSqlDateTime()}`);

    console.log(`getSqlDateTime(null, 365): ${getSqlDateTime(null, 365)}`);

    console.log(`getSqlDate(null, -500): ${getSqlDate(null, -500)}`);
}

// test()

module.exports = {
    getSqlDate,
    getSqlDateTime
} ;
