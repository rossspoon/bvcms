

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE FUNCTION [dbo].[InBFClass]
	(
	@pid int
	)
RETURNS bit
AS
	BEGIN
	declare @mem BIT, @bfid INT
	SELECT TOP 1 @bfid = Id FROM dbo.Program WHERE BFProgram = 1

	select @mem = 1
	FROM dbo.OrganizationMembers  AS om
	join dbo.Organizations o on o.OrganizationId = om.OrganizationId
	join dbo.DivOrg AS do ON o.OrganizationId = do.OrgId
	join dbo.Division d ON do.DivId = d.Id
	where d.ProgId = @bfid and om.PeopleId = @pid

	RETURN isnull(@mem, 0)

	END






GO
