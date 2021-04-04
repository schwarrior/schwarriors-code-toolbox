select 
    ROW_NUMBER() OVER (ORDER BY TABLE_SCHEMA, TABLE_NAME) as RowNumber,
    TABLE_SCHEMA, 
    TABLE_NAME
from 
    Information_Schema.Tables
order by
    TABLE_SCHEMA, 
    TABLE_NAME