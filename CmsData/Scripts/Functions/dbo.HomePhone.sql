SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION dbo.HomePhone(@pid int)
RETURNS varchar(11)
AS
	BEGIN
	declare @homephone varchar(11)
	select @homephone = f.HomePhone from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

	RETURN @homephone
	END

GO
