SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW dbo.FirstName
AS
SELECT     FirstName, COUNT(*) AS count
FROM         dbo.People
GROUP BY FirstName
GO
