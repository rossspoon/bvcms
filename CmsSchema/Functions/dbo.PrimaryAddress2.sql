
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION dbo.PrimaryAddress2 ( @pid int )
RETURNS varchar(60)
AS
	BEGIN
declare @addr varchar(60)
select @addr =
	case AddressTypeId
			when 10 then f.AddressLineTwo
			when 20 then f.AltAddressLineTwo
			when 30 then p.AddressLineTwo
			when 40 then p.AltAddressLineTwo
	end
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

	RETURN @addr
	END


GO
