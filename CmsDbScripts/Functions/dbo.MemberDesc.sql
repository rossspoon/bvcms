SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION dbo.MemberDesc(@id int) 
RETURNS VARCHAR(100)
AS
BEGIN
	DECLARE @ret VARCHAR(100)
	SELECT @ret =  Description FROM lookup.MemberType WHERE id = @id
	RETURN @ret
END


GO
