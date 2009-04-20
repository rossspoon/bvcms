SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION SchoolGrade (@pid INT)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @g INT

SELECT TOP 1 @g = GradeRangeStart FROM dbo.Organizations o
WHERE EXISTS(
		SELECT NULL 
		FROM dbo.OrganizationMembers om 
		WHERE om.OrganizationId = o.OrganizationId
		AND om.PeopleId = @pid AND om.MemberTypeId = 220
		)
AND o.GradeRangeStart = o.GradeRangeEnd AND o.GradeRangeStart IS NOT NULL AND o.GradeRangeStart <> 0


	-- Return the result of the function
	RETURN @g

END

GO
