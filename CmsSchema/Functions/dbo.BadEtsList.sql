SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION BadEtsList(@flag INT) 
RETURNS TABLE 
AS
RETURN 
(
SELECT et.id, et.Flag, et2.PeopleId, et2.OrganizationId, et2.TransactionId, et2.OrganizationName, p.Name2, et.Status, et2.TransactionDate, et2.TransactionTypeId, 
                      et2.TransactionStatus
FROM dbo.People AS p 
INNER JOIN dbo.EnrollmentTransaction AS et2 ON p.PeopleId = et2.PeopleId 
LEFT OUTER JOIN dbo.BadET AS et ON et2.TransactionId = et.TranId
WHERE EXISTS (SELECT NULL FROM dbo.BadET
              WHERE OrgId = et2.OrganizationId AND PeopleId = et2.PeopleId)
AND (@flag = 0 
OR EXISTS(SELECT NULL FROM dbo.BadET
			WHERE OrgId = et2.OrganizationId AND PeopleId = et2.PeopleId
			AND Flag = @flag))
)

GO
