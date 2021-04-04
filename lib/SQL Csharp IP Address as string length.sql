create table [Send]
(
	SendId int not null primary key identity,
	SenderIpAddress varchar(45) not null 
	--'000.000.000.000' 15 chars ipv4 in dotted decimal 
	--or '0000:0000:0000:0000:0000:0000:0000:0000' 39 chars in ipv6 hexideciaml
	--or '0000:0000:0000:0000:0000:0000:127.127.127.127' v4/v6 hybrid at 45 chars 
	--https://stackoverflow.com/questions/166132/maximum-length-of-the-textual-representation-of-an-ipv6-address
)
