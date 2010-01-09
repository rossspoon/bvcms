SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE VIEW [dbo].[UserList]
AS
SELECT     Username, UserId, p.Name, p.Name2, IsApproved, MustChangePassword, IsLockedOut, p.EmailAddress,
                          (SELECT     MAX(v.VisitTime) AS Expr1
                            FROM          disc.PageVisit AS v
                            WHERE      (UserId = u.UserId)) AS LastVisit, u.PeopleId
FROM         dbo.Users u JOIN dbo.People p ON u.PeopleId = p.PeopleId

GO
