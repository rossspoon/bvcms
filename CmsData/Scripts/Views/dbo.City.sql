SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE VIEW dbo.City
AS
SELECT PrimaryCity AS City, PrimaryState AS State, PrimaryZip AS Zip, COUNT(*) AS [count] FROM dbo.People GROUP BY PrimaryCity, PrimaryState, PrimaryZip

GO
