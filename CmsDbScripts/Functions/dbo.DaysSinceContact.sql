SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[DaysSinceContact](@pid INT)
RETURNS int
AS
BEGIN
	DECLARE @days int

	SELECT @days = MIN(DATEDIFF(D,c.ContactDate,GETDATE())) FROM dbo.NewContact c
	JOIN dbo.Contactees ce ON c.ContactId = ce.ContactId
	WHERE ce.PeopleId = @pid

	RETURN @days

END



GO
