SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[UserList]
AS
SELECT Username, UserId, p.Name, p.Name2, IsApproved, MustChangePassword, IsLockedOut, p.EmailAddress,
                          u.LastActivityDate, u.PeopleId, dbo.UserRoleList(UserId) Roles
FROM dbo.Users u 
JOIN dbo.People p ON u.PeopleId = p.PeopleId
GO
