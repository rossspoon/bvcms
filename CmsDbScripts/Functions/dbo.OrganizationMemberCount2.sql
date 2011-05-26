SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE FUNCTION [dbo].[OrganizationMemberCount2](@oid int) 
RETURNS int
AS
BEGIN
	DECLARE @c int
	SELECT @c = count(*) from dbo.OrganizationMembers 
	where OrganizationId = @oid AND (Pending IS NULL OR Pending = 0)
	RETURN @c
END

GO
