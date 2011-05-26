SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION dbo.PrimaryZip ( @pid int )
RETURNS varchar(11)
AS
	BEGIN
declare @zip varchar(11)
select @zip =
	case AddressTypeId
			when 10 then f.ZipCode
			when 20 then f.AltZipCode
			when 30 then p.ZipCode
			when 40 then p.AltZipCode
	end
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

	RETURN @zip
	END

GO
