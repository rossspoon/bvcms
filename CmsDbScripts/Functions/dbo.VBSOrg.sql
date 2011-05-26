SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[VBSOrg](@pid INT) 
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @org INT

	-- Add the T-SQL statements to compute the return value here
	DECLARE @vbs INT
	SELECT @vbs = CONVERT(INT, Setting) FROM dbo.Setting WHERE Id = 'VBS'
	
	SELECT @org = om.OrganizationId
	FROM dbo.OrganizationMembers om
	JOIN dbo.People p ON om.PeopleId = p.PeopleId
	JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
	WHERE EXISTS(SELECT NULL FROM dbo.DivOrg do 
		JOIN dbo.Division d ON do.DivId = d.Id
		WHERE d.ProgId = @vbs
		AND do.OrgId = om.OrganizationId)
	AND o.OrganizationStatusId = 30
	AND p.PeopleId = @pid

	-- Return the result of the function
	RETURN @org

END
GO
