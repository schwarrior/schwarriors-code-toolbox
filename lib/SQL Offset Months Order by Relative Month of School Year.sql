declare @SchoolYearBegin int = 2021
declare @Months table ([Month] int not null primary key)
insert @Months([Month]) values (1), (2), (3), (4), (5), (6), (7), (8), (9), (10), (11), (12)
select 
	[Month] as CalendarMonth
	,case when [Month] - 8 < 0 then @SchoolYearBegin + 1 else @SchoolYearBegin end as [Calendar Year]
	,case when [Month] - 8 < 0 then [Month] + 5 else [Month] - 7 end as [School-Year Month]
	,@SchoolYearBegin as [School-Year Year]
from 
	@Months
order by
	case when [Month] - 8 < 0 then @SchoolYearBegin + 1 else @SchoolYearBegin end
	,[Month]