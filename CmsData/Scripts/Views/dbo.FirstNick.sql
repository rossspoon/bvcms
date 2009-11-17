SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE VIEW dbo.FirstNick
AS
SELECT FirstName, NickName, COUNT(*) AS [count] FROM dbo.People GROUP BY FirstName, NickName

GO
