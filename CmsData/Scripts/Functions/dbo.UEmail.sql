SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[UEmail] (@pid int)
RETURNS varchar(100)
AS
BEGIN
	-- Declare the return variable here
	declare @email varchar(100)
	
	SELECT  @email = EmailAddress
	FROM         dbo.People
	WHERE     PeopleId = @pid

	return @email

END
GO
