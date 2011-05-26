SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].Churches
AS
SELECT c FROM (
SELECT OtherNewChurch c FROM dbo.People
UNION 
SELECT OtherPreviousChurch c FROM dbo.People
) AS t
GROUP BY c

GO
