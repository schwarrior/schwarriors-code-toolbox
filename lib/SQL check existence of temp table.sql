if (object_id('tempdb..#workingset') is not null)
drop table #workingset;

select * 
into #workingset
from information_schema.tables 

select * from #workingset