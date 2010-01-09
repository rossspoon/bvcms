SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION disc.VerseSummaryForCategory2
	(
	@catid int
	)
RETURNS TABLE
AS
	RETURN SELECT *, disc.VerseInCategory(id, @catid) As InCategory FROM dbo.VerseSummary
GO
