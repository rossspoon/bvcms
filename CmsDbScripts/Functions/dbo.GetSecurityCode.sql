SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetSecurityCode]()
RETURNS CHAR(3)
AS
BEGIN
	DECLARE @code CHAR(3)
	EXEC NextSecurityCode @code OUTPUT
	RETURN @code
END
GO
