SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE FUNCTION [dbo].[OrganizationMemberCount](@oid int) 
RETURNS int
AS
BEGIN
	DECLARE @c int
	SELECT @c = count(*) from dbo.OrganizationMembers 
	where OrganizationId = @oid
	AND (Pending = 0 OR Pending IS NULL)
	AND MemberTypeId <> 230
	RETURN @c
END

GO
