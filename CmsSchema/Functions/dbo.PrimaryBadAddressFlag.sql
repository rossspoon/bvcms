
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[PrimaryBadAddressFlag]( @pid int )
RETURNS int
AS
	BEGIN
declare @flag bit
select @flag =
	case AddressTypeId
		when 10 then f.BadAddressFlag
		when 20 then f.AltBadAddressFlag
		when 30 then p.BadAddressFlag
		when 40 then p.AltBadAddressFlag
	end
	
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

if (@flag is null)
	select @flag = 0

	RETURN @flag
	END


GO
