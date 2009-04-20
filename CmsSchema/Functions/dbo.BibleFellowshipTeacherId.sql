

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE FUNCTION [dbo].[BibleFellowshipTeacherId]
	(
	@pid int
	)
RETURNS int
AS
	BEGIN
	declare @id varchar(100),	@oid int
	
	select @oid = dbo.BibleFellowshipClassId(@pid)

SELECT  TOP 1 @id = p.PeopleId
FROM         dbo.OrganizationMembers AS om INNER JOIN
             dbo.Organizations AS o ON om.OrganizationId = o.OrganizationId INNER JOIN
             dbo.People AS p ON om.PeopleId = p.PeopleId
WHERE     (om.MemberTypeId = o.LeaderMemberTypeId) AND (om.OrganizationId = @oid)
ORDER BY EnrollmentDate

	return @id
	END


GO
