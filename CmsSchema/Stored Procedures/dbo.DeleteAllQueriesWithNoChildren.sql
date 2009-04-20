SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[DeleteAllQueriesWithNoChildren]
AS
BEGIN
	SET NOCOUNT ON;

delete from QueryBuilderClauses where queryid in (select q.queryid
FROM       QueryBuilderClauses q
where not exists (select null from QueryBuilderClauses where groupid = q.queryid))

END

GO
