SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW dbo.FirstName2
AS
SELECT     FirstName, GenderId, CA, COUNT(*) AS Expr1
FROM         (SELECT     FirstName, GenderId, CASE WHEN Age <= 18 THEN 'C' ELSE 'A' END AS CA
                       FROM          dbo.People) AS ttt
GROUP BY FirstName, GenderId, CA
GO
