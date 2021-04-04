IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GenerateRows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GenerateRows]
GO

create function [dbo].[GenerateRows]
(@rangeBegin int, 
@rangeEnd int)
returns @Range table(Number int)
AS
begin
	--declare @Range table(Number int);
	
	WITH Num2( n ) AS ( SELECT 1 UNION SELECT 0 ),
	Num4( n ) AS ( SELECT 1 FROM Num2 n1 CROSS JOIN Num2 n2 ),
	Num16( n ) AS ( SELECT 1 FROM Num4 n1 CROSS JOIN Num4 n2 ),
	Num256( n ) AS ( SELECT 1 FROM Num16 n1 CROSS JOIN Num16 n2 ),
	Num65K( n ) AS ( SELECT 1 FROM Num256 n1 CROSS JOIN Num256 n2 )--,
	--Num1M ( n ) AS ( SELECT 1 FROM Num65K n1 CROSS JOIN Num16 n2 )
	Insert @Range(Number)
	SELECT n + @rangeBegin - 1 as Number
	--INTO drop table Test.dbo.SN
	FROM ( SELECT ROW_NUMBER() OVER (ORDER BY n)
	FROM Num65K ) D ( n )
	WHERE n <= (@rangeEnd - @rangeBegin+1); 

	return

end
GO

GRANT SELECT ON [dbo].[GenerateRows] TO [db_datareader] AS [dbo]
GO
