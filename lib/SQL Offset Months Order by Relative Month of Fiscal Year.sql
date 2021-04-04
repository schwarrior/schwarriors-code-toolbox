declare @FiscalYearBegin int = 2021
declare @Months table ([Month] int not null primary key)
insert @Months([Month]) values (1), (2), (3), (4), (5), (6), (7), (8), (9), (10), (11), (12)
select 
	[Month] as CalendarMonth
	,case when [Month] - 4 < 0 then @FiscalYearBegin + 1 else @FiscalYearBegin end as [Calendar Year]
	--,case when [Month] - 8 < 0 then [Month] + 5 else [Month] - 7 end as [Fiscal-Year Month]
	,case when [Month] - 4 < 0 then [Month] + 9 else [Month] - 3 end as [Fiscal-Year Month]
	,@FiscalYearBegin as [Fiscal-Year Year]
from 
	@Months
order by
	case when [Month] - 4 < 0 then @FiscalYearBegin + 1 else @FiscalYearBegin end
	,[Month]