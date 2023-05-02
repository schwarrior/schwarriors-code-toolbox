Excel Date Decimal Decoding
===========================

# Introduction
Developing a custom Node.js module
mi-xls-to-json https://measinc.visualstudio.com/StudentInfo/_git/StudentInfo.Utility.ExcelToJson, 
that converts Excel files into JSON files.
Using the NPM package XLXS https://www.npmjs.com/package/xlsx,
cells with date values convert unpredictably into several forms, often a long decimal number.
Apparently this is a limitation of the free "community" version of XLSX;
there is a commercial version that will handle date formatting.
However, if we understand better, perhaps the free version of XLXS is sufficient. 
We can succeed in the ultimate goal of converting all date and/or time data from Excel into the ANSI string format that SQL Server uses as its default.

# Conclusion

The decimal form of date is interpreted as follows:
- the integer part of the date number is the number of days since 1/1/1900.
- the decimal part of the date number is the fraction of the day elapsed thus translates to time.

This decimal date number is Excel's internal format for storing dates and times. It is not the same as the numeric represenation of dates and times used in JavaScript. ECMA numeric date & time is the number of milliseconds elapsed from midnight of January 1, 1970. 

When the XLSX module translates Excel dates, if a cell format has been applied (explicitly or implicitly), it will attempt to convert to a string that mirrors Excel display format, with mixed results.
To reliably receive a number date, provide the option `raw: false` to the `sheet_to_json` method.

```javaScript
const rows = xlsx.utils.sheet_to_json(workbook.Sheets[sheetName], {raw: false});
```

# Analysis			

```
+-------------+----------------------------+-----------------------+
|    Value    |       Format String        |    Formatted Value    |
+-------------+----------------------------+-----------------------+
| 44310.58333 | [$-en-US]m/d/yy h:mm AM/PM | 4/24/2021  2:00:00 PM |
| 44310.58333 | yyyy-mm-dd h:mm:ss         | 2021-04-24 14:00:00   |
| 44310.58333 | @                          | 44310.5833333333      |
+-------------+----------------------------+-----------------------+

Date Time	44310.58333
Date	44310
Time	0.583333333
Ratio Numerator	0.583333333	14
Ratio Denominator	1	24
Hour of the Day	14
Years since 1900	121.3972603
Years floor	121
Day of Year	0.397260274
Ratio Numerator	0.397260274	145
Ratio Denominator	1	365
Day of Year	145

```
