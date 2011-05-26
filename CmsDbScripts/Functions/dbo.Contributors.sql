SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[Contributors](@fd DATETIME, @td DATETIME, @pid INT, @spid INT, @fid INT, @noaddrok BIT)
RETURNS TABLE 
AS
RETURN 
(
	SELECT	
			p.PrimaryAddress,
			p.PrimaryAddress2,
			p.PrimaryCity,
			p.PrimaryState,
			p.PrimaryZip,
			p.FamilyId,
			
			p.PeopleId, 
			p.LastName,
			p.FirstName + ' ' + p.LastName AS Name, 
			p.TitleCode AS Title, 
			p.SuffixCode AS Suffix,
			CASE 
				WHEN 2 IN (ISNULL(p.ContributionOptionsId, 0), ISNULL(sp.ContributionOptionsId,0)) 
						AND ISNULL(p.ContributionOptionsId,0) <> 9 THEN 2 
				WHEN ISNULL(p.ContributionOptionsId,0) = 2 
						AND (sp.PeopleId IS NULL OR ISNULL(sp.ContributionOptionsId, 0) = 9) THEN 1
				WHEN p.ContributionOptionsId IS NULL THEN 0
				ELSE p.ContributionOptionsId
				END AS ContributionOptionsId,
			p.DeceasedDate,
			p.Age,
			p.PositionInFamilyId,
			CASE 
				WHEN f.HeadOfHouseholdId = p.PeopleId THEN 1 
				WHEN f.HeadOfHouseholdSpouseId = p.PeopleId THEN 2 
				ELSE 3 
				END AS hohFlag,
		    ISNULL((SELECT SUM(ContributionAmount)
				FROM Contribution c 
				WHERE c.PeopleId = p.PeopleId
				AND c.ContributionStatusId = 0
				AND c.ContributionTypeId NOT IN (6,7)
				AND c.ContributionDate >= @fd
				AND c.ContributionDate <= @td), 0) AS Amount,
			
			sp.Name AS SpouseName,
			sp.TitleCode AS SpouseTitle,
			p.SpouseId,
			CASE 
				WHEN 2 IN (ISNULL(p.ContributionOptionsId,0), ISNULL(sp.ContributionOptionsId,0)) 
						AND p.ContributionOptionsId <> 9 THEN 2 
				WHEN ISNULL(p.ContributionOptionsId,0) = 2 
						AND (sp.PeopleId IS NULL OR ISNULL(sp.ContributionOptionsId,0) = 9) THEN 1
				WHEN sp.ContributionOptionsId IS NULL THEN 0
				ELSE sp.ContributionOptionsId
				END AS SpouseContributionOptionsId,
		    ISNULL((SELECT SUM(ISNULL(ContributionAmount,0))
				FROM Contribution c 
				WHERE c.PeopleId = p.SpouseId
				AND c.ContributionStatusId = 0
				AND c.ContributionTypeId NOT IN (6,7)
				AND c.ContributionDate >= @fd
				AND c.ContributionDate <= @td),0) AS SpouseAmount,
			p.CampusId,
			(SELECT LastName FROM dbo.People WHERE PeopleId = f.HeadOfHouseholdId) AS HouseName
			
	from People p
	JOIN dbo.Families f ON p.FamilyId = f.FamilyId
	LEFT OUTER JOIN dbo.People sp ON p.SpouseId = sp.PeopleId
	
	WHERE 
	(@noaddrok = 1 OR p.PrimaryAddress <> '')
	AND p.PrimaryBadAddrFlag = 0
	AND p.DoNotMailFlag = 0
	AND ISNULL(p.ContributionOptionsId, 0) <> 9	
	AND (@pid = 0 OR @pid = p.PeopleId OR @spid = p.PeopleId)
	AND (@fid = 0 OR @fid = p.FamilyId)
)
GO
