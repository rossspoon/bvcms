SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[ChangedGiving](@dt1 DATETIME, @dt2 DATETIME, @lopct FLOAT, @hipct FLOAT)
AS
BEGIN

	SELECT PeopleId, Name, dbo.ContributionAmount2(PeopleId, @dt1, @dt2, NULL) FROM dbo.People
	WHERE dbo.ContributionChange(PeopleId, @dt1, @dt2) BETWEEN @lopct AND @hipct
	
END
GO
