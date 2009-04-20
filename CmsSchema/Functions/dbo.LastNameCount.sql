SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[LastNameCount](@last VARCHAR(20))
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @r INT
	
	SELECT @r = [count] FROM dbo._LastName WHERE LastName = @last
	RETURN @r

END

GO
