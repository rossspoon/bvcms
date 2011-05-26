SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[PrimaryResCode]( @pid int )
RETURNS int
AS
	BEGIN
declare @rescodeid int
select @rescodeid =
	case AddressTypeId
		when 10 then f.ResCodeId
		when 20 then f.AltResCodeId
		when 30 then p.ResCodeId
		when 40 then p.AltResCodeId
	end
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

if @rescodeid is null
	select @rescodeid = 40

	RETURN @rescodeid
	END

GO
