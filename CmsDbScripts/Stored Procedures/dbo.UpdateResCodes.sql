SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].UpdateResCodes 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-----------------------------------------------------------------
	
UPDATE dbo.People
SET ResCodeId = dbo.FindResCode(ZipCode),
AltResCodeId = dbo.FindResCode(AltZipCode)

UPDATE dbo.Families
SET ResCodeId = dbo.FindResCode(ZipCode),
AltZipCode = dbo.FindResCode(AltZipCode)


END
GO
