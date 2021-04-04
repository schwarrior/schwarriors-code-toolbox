declare @mydate datetime = getdate() --'1-2-2022 3:04:05.678 AM' '1-2-2022 3:04:05.678 AM' 

select
	cast(YEAR(@mydate) as varchar(4))
	+ right('0' + cast(MONTH(@mydate) as varchar(2)),2)
	+ right('0' + cast(DAY(@mydate) as varchar(2)),2)
	+ right('0' + cast( DATEPART(hh,@mydate) as varchar(2)),2)
	+ right('0' + cast( DATEPART(mi,@mydate) as varchar(2)),2)
	+ right('0' + cast( DATEPART(ss,@mydate) as varchar(2)),2)
	--+ right('00' + cast( DATEPART(ms,@mydate) as varchar(3)),3)
