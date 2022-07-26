-- create a random number

DECLARE @MIN Int = 100000;
DECLARE @MAX Int = 999999
SELECT FLOOR(RAND()*(@MAX-@MIN+1)+@MIN);