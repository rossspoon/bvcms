SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION dbo.PrimaryAddress ( @pid int )
RETURNS varchar(60)
AS
	BEGIN
declare @addr varchar(60)
select @addr =
	case AddressTypeId
			when 10 then f.AddressLineOne
			when 20 then f.AltAddressLineOne
			when 30 then p.AddressLineOne
			when 40 then p.AltAddressLineOne
	end
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

	RETURN @addr
	END

GO
