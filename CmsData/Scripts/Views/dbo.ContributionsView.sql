SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE VIEW [dbo].[ContributionsView]
AS
SELECT
	f.FundId, 
	ContributionTypeId AS TypeId, 
	ContributionDate AS CDate, 
	ContributionAmount AS Amount, 
	ContributionStatusId AS StatusId,
	PledgeFlag AS Pledged,
	Age,
	Age / 10 AS AgeRange,
	f.FundDescription AS Fund,
	cs.Description AS [Status],
	ct.Description AS [Type],
	p.Name
FROM dbo.Contribution c
JOIN dbo.People p ON c.PeopleId = p.PeopleId
JOIN lookup.ContributionStatus cs ON c.ContributionStatusId = cs.Id
JOIN lookup.ContributionType ct ON c.ContributionTypeId = ct.Id
JOIN dbo.ContributionFund f ON c.FundId = f.FundId

GO
