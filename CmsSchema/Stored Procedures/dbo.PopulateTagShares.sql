SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PopulateTagShares]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO dbo.TagShare (
		TagId,
		PeopleId
	) 
	SELECT DISTINCT t.Id, u.PeopleId 
	FROM dbo.TagShared t
	JOIN Users u ON t.SharedUser = u.Username
	
	UPDATE dbo.Tag
	SET PeopleId = u.PeopleId
	FROM dbo.Tag t
	JOIN dbo.Users u ON t.Owner = u.Username
END

GO
