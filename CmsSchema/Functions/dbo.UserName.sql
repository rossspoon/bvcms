


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].UserName (@pid int)
RETURNS varchar(100)
AS
	BEGIN
	declare @name varchar(100)
	
SELECT  @name = [LastName]+', '+(case when [Nickname]<>'' then [nickname] else [FirstName] end)
FROM         dbo.People
WHERE     PeopleId = @pid

	return @name
	END



GO
