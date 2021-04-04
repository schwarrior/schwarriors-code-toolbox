
declare @car table(make varchar(15), [model] varchar(15), [year] int);

insert @car(make,[model],[year])
values
 ('Audi','R8',2012)
,('Audi','A3',2013)
,('Kia','Sorento',2014)
,('BMW','M3',2014)
,('BMW','M3',2015)	

Select
	make,[model],[year],
	count(*)
From
	@car
Group by rollup(make,[model],[year])

--result
/*
make            model           year        
--------------- --------------- ----------- -----------
Audi            A3              2013        1
Audi            A3              NULL        1
Audi            R8              2012        1
Audi            R8              NULL        1
Audi            NULL            NULL        2
BMW             M3              2014        1
BMW             M3              2015        1
BMW             M3              NULL        2
BMW             NULL            NULL        2
Kia             Sorento         2014        1
Kia             Sorento         NULL        1
Kia             NULL            NULL        1
NULL            NULL            NULL        5
*/