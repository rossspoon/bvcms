SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[VerseSummaryForCategory2]
	(
	@catid int
	)
RETURNS TABLE
AS
	RETURN SELECT *, dbo.VerseInCategory(id, @catid) As InCategory FROM dbo.VerseSummary
GO
