SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[Contributors](@fd DATETIME, @td DATETIME, @pid INT, @spid INT, @fid INT)
RETURNS TABLE 
AS
RETURN 
(
	select	p.PeopleId, 
			p.PositionInFamilyId, 
			p.Name, 
			f.HeadOfHouseholdId, 
			p.TitleCode AS Title, 
			p.SuffixCode AS Suffix,
			p.SpouseId,
			sp.[Name] AS SpouseName, 
			sp.TitleCode AS SpouseTitle,
			sp.ContributionOptionsId AS SpouseContributionOptionsId,
			p.ContributionOptionsId,
			p.PrimaryAddress,
			p.PrimaryAddress2,
			p.PrimaryCity,
			p.PrimaryState,
			p.PrimaryZip,
			p.DeceasedDate,
			p.FamilyId,
			p.Age,
			CASE WHEN f.HeadOfHouseholdId = p.PeopleId THEN 1 ELSE 0 END AS hohFlag
	from People p
	JOIN dbo.Families f ON p.FamilyId = f.FamilyId
	LEFT OUTER JOIN dbo.People sp ON p.SpouseId = sp.PeopleId
	WHERE EXISTS(
		SELECT NULL FROM Contribution c 
		WHERE c.PeopleId = p.PeopleId
		AND c.ContributionStatusId = 0
		AND c.ContributionTypeId NOT IN (6,7)
		AND c.ContributionDate >= @fd
		AND c.ContributionDate <= @td)
	AND p.ContributionOptionsId > 0
	AND p.PrimaryAddress <> ''
	AND p.PrimaryBadAddrFlag = 0
	AND (@pid = 0 OR @pid = p.PeopleId OR @spid = p.PeopleId)
	AND (@fid = 0 OR @fid = p.FamilyId)
)
GO
