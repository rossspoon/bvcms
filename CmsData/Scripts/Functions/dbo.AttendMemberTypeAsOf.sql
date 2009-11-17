SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[AttendMemberTypeAsOf]
(	
	@from DATETIME,
	@to DATETIME,
	@progid INT,
	@divid INT,
	@orgid INT,
	@not BIT,
	@ids VARCHAR(4000)
)
RETURNS @t TABLE ( PeopleId INT )
AS
BEGIN
	INSERT INTO @t (PeopleId) SELECT p.PeopleId FROM dbo.People p
	WHERE EXISTS (
		SELECT NULL FROM dbo.Attend a
		WHERE a.PeopleId = p.PeopleId
		AND (@not = 1 OR a.MemberTypeId IN (SELECT id FROM CsvTable(@ids)))
		AND (@not = 0 OR a.MemberTypeId NOT IN (SELECT id FROM CsvTable(@ids)))
		AND a.AttendanceFlag = 1
		AND a.MeetingDate >= @from
		AND a.MeetingDate < @to
		AND (a.OrganizationId = @orgid OR @orgid = 0)
		AND (EXISTS(SELECT NULL FROM DivOrg d1
				WHERE d1.OrgId = a.OrganizationId
				AND d1.DivId = @divid) OR @divid = 0)
		AND (EXISTS(SELECT NULL FROM DivOrg d2
				WHERE d2.OrgId = a.OrganizationId
				AND EXISTS(SELECT NULL FROM Division d
						WHERE d2.DivId = d.Id
						AND d.ProgId = @progid)) OR @progid = 0)
		)
	RETURN
END	
GO
