SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[SchoolGrade] (@pid INT)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @g INT

	SELECT TOP 1 @g = o.GradeAgeStart 
	FROM dbo.OrganizationMembers AS om 
		JOIN dbo.Organizations AS o ON om.OrganizationId = o.OrganizationId 
	WHERE o.IsBibleFellowshipOrg = 1
		AND om.PeopleId = @pid 
		AND om.MemberTypeId = 220 
		AND GradeAgeStart <> 0

	-- Return the result of the function
	RETURN @g

END
GO
