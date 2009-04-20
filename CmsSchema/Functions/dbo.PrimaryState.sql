
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION dbo.PrimaryState ( @pid int )
RETURNS varchar(5)
AS
	BEGIN
declare @st varchar(5)
select @st =
	case AddressTypeId
			when 10 then f.StateCode
			when 20 then f.AltStateCode
			when 30 then p.StateCode
			when 40 then p.AltStateCode
	end
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

	RETURN @st
	END


GO
