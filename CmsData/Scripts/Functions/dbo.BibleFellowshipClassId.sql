SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO



CREATE FUNCTION [dbo].[BibleFellowshipClassId]
	(
	@pid int
	)
RETURNS int
AS
	BEGIN
	declare @oid INT, @bfid INT
	SELECT TOP 1 @bfid = Id FROM dbo.Program WHERE BFProgram = 1

	select top 1 @oid = om.OrganizationId from dbo.OrganizationMembers AS om INNER JOIN
		dbo.Organizations AS o ON om.OrganizationId = o.OrganizationId inner JOIN
		dbo.DivOrg do ON o.OrganizationId = do.OrgId INNER JOIN
		dbo.Division d ON do.DivId = d.Id
		where d.ProgId = @bfid and om.PeopleId = @pid

	return @oid
	END
GO
