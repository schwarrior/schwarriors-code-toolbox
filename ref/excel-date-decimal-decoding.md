Excel Date Decimal Decoding
===========================

# Introduction
Working on a project
mi-xls-to-json
https://measinc.visualstudio.com/StudentInfo/_git/StudentInfo.Utility.ExcelToJson
A Node.js program converts Excel files into JSON files
Using the NPM package
https://www.npmjs.com/package/xlsx
Cell with date values become long decimal numbers
Apparently this is a limitation of the free "community" version of XLSX
There is a commercial version that will handle date formatting
This lab seeks to understand what these decimal date representations
Perhaps I can decode them back into ANSI (SQL Server) format

# Conclusion

The integer part of the date number is the number of days since 1/1/1900			
The decimal part of the date number is the fraction of the day elapsed. Translates to time.
However, it turns out that the xmlx package will format date columns the way they look on the sheet.
The secret is to provide the option raw: false to the sheet_to_json method.

```javaScript
const rows = xlsx.utils.sheet_to_json(workbook.Sheets[sheetName], {raw: false});
```

# Analysis			

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
