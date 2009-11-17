SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION QBClauses(@qid INT)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	WITH QB (QueryId, GroupId, SavedBy, Description, Level)
	AS
	(
	-- Anchor member definition
		SELECT c.QueryId, c.GroupId, c.SavedBy, c.Description, 0 AS Level
		FROM dbo.QueryBuilderClauses c
		WHERE c.QueryId = @qid AND c.GroupId IS NULL
		UNION ALL
	-- Recursive member definition
		SELECT c.QueryId, c.GroupId, c.SavedBy, c.Description, cc.Level + 1
		FROM dbo.QueryBuilderClauses c
		INNER JOIN QB AS cc
			ON c.GroupId = cc.QueryId
	)
	SELECT QueryId, GroupId, SavedBy, Description, [Level] FROM QB
)
GO
