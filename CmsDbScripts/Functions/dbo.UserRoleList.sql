SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[UserRoleList](@uid int)
RETURNS VARCHAR(500)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @roles VARCHAR(500)
	select @roles = coalesce(@roles + '|', '|') + r.RoleName FROM dbo.UserRole ur JOIN dbo.Roles r ON ur.RoleId = r.RoleId
	WHERE ur.UserId = @uid

	RETURN @roles + '|'

END
GO
