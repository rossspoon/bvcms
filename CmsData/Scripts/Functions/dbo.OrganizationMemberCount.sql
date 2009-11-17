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
	RETURN @c
END

GO
