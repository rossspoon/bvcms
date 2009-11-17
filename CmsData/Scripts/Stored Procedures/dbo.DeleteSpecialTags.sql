SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE DeleteSpecialTags(@pid INT = null)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DELETE dbo.TagPerson
FROM dbo.TagPerson tp
JOIN dbo.Tag t ON tp.Id = t.Id
WHERE t.TypeId IN (3,4,5) AND (t.PeopleId = @pid OR @pid IS NULL)

DELETE FROM dbo.Tag
WHERE TypeId IN (3,4,5) AND (PeopleId = @pid OR @pid IS NULL)

END
GO
