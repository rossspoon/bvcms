SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION dbo.PrimaryCity ( @pid int )
RETURNS varchar(50)
AS
	BEGIN
declare @city varchar(50)
select @city =
	case AddressTypeId
			when 10 then f.CityName
			when 20 then f.AltCityName
			when 30 then p.CityName
			when 40 then p.AltCityName
	end
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

	RETURN @city
	END

GO
