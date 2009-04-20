
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION dbo.AttendDesc(@id int) 
RETURNS VARCHAR(100)
AS
BEGIN
	DECLARE @ret VARCHAR(100)
	SELECT @ret =  Description FROM lookup.AttendType WHERE id = @id
	RETURN @ret
END


GO
