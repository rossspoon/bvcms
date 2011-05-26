SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[TopGivers](@top INT, @sdate DATETIME, @edate DATETIME)
AS
BEGIN

	SELECT TOP (@top) c.PeopleId, Name, SUM(ContributionAmount) FROM dbo.People p
	JOIN dbo.Contribution c ON p.PeopleId = c.PeopleId
	WHERE c.ContributionDate >= @sdate
	AND c.ContributionDate <= @edate
	GROUP BY c.PeopleId, Name
	ORDER BY SUM(ContributionAmount) DESC

END
GO
EXEC sp_addextendedproperty N'ReturnType', N'TopGiver', 'SCHEMA', N'dbo', 'PROCEDURE', N'TopGivers', NULL, NULL
GO
