



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

	select top 1 @oid = om.OrganizationId from dbo.OrganizationMembers AS om 
	JOIN dbo.Organizations AS o ON om.OrganizationId = o.OrganizationId
	JOIN dbo.Division d ON o.DivisionId = d.Id
	where d.ProgId = @bfid and om.PeopleId = @pid
	AND ISNULL(om.Pending, 0) = 0

	IF @oid IS NULL
		select top 1 @oid = om.OrganizationId from dbo.OrganizationMembers AS om 
		JOIN dbo.Organizations AS o ON om.OrganizationId = o.OrganizationId
		JOIN dbo.DivOrg do ON o.OrganizationId = do.OrgId
		JOIN dbo.Division d ON do.DivId = d.Id
		where d.ProgId = @bfid and om.PeopleId = @pid
		AND ISNULL(om.Pending, 0) = 0

	return @oid
	END
GO
