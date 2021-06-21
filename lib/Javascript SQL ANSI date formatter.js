'use strict';

//Get the date in YYYY-MM-DD HH:MM:SS format
//Run with no params to get for now
//Or pass a JS Date Obj to be converted
const getSqlDateObj =  function(jsDateObj = null) {
    let d = new Date();
    if (jsDateObj && isDateObj(jsDateObj)) {
        d = jsDateObj;
    }
    const dSql = d.getFullYear() +
        '-' + pad(d.getMonth() + 1) +
        '-' + pad(d.getDate()) +
        ' ' + pad(d.getHours()) +
        ':' + pad(d.getMinutes()) +
        ':' + pad(d.getSeconds());
    return dSql;
};

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

const test = function() {
    const jsOldDateStr = "Fri Jun 11 2021 14:41:31 GMT-0400 (Eastern Daylight Time)";
    const jsOldDateObj = new Date(jsOldDateStr);
    console.log(`isDateObj(): ${isDateObj()}`);
    console.log(`isDateObj(${jsOldDateStr}): ${isDateObj(jsOldDateStr)}`);
    console.log(`isDateObj(new Date(${jsOldDateObj})): ${isDateObj(jsOldDateObj)}`);
    
    console.log(`getSqlDateObj(new Date(${jsOldDateObj})): ${getSqlDateObj(jsOldDateObj)}`);
    console.log(`getSqlDateObj(): ${getSqlDateObj()}`);
}

// test()

module.exports = getSqlDateObj;