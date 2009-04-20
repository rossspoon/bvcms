
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[AttendMeetingInfo]
( @MeetingId INT, @PeopleId INT)
AS
BEGIN
	DECLARE @orgid INT,
			@meetdt DATE,
			@tm TIME,
			@dt DATETIME,
			@regularhour BIT,
			@attendedelsewhere BIT,
			@membertypeid INT,
			@schedid INT

	SELECT
		@orgid = m.OrganizationId,
		@schedid = o.ScheduleId,
		@dt = DATEADD(DAY, o.RollSheetVisitorWks * -7, m.MeetingDate),
		@attendedelsewhere = CASE WHEN exists(
				SELECT NULL FROM Attend ae
				JOIN dbo.Organizations oo 
				ON ae.OrganizationId = oo.OrganizationId
				WHERE ae.PeopleId = @PeopleId
				AND ae.AttendanceFlag = 1
				AND ae.MeetingDate = m.MeetingDate
				AND ae.OrganizationId <> m.OrganizationId
				AND oo.AllowAttendOverlap <> 1
				AND o.AllowAttendOverlap <> 1)
			THEN 1 ELSE 0 END,
		@membertypeid = (SELECT om.MemberTypeId
					FROM dbo.OrganizationMembers om
					WHERE om.OrganizationId = o.OrganizationId
					AND om.PeopleId = @PeopleId)

	FROM dbo.Meetings m
	JOIN dbo.Organizations o ON m.OrganizationId = o.OrganizationId
	WHERE m.MeetingId = @MeetingId

	IF @dt IS NULL
		SELECT @dt = DATEADD(DAY, 3 * -7, @meetdt)

	DECLARE @name VARCHAR(50), @bfclassid INT

	SELECT @name = p.[Name], @bfclassid = BibleFellowshipClassId
	FROM dbo.People p
	WHERE PeopleId = @PeopleId


	SELECT
		@meetdt = CONVERT(DATE, m.MeetingDate),
		@tm = CONVERT(TIME, m.MeetingDate)
	FROM dbo.Meetings m
	WHERE m.MeetingId = @MeetingId
		
		
	SELECT @regularhour = CASE WHEN EXISTS(
		SELECT null
			FROM dbo.Meetings m
			JOIN dbo.Organizations o ON m.OrganizationId = o.OrganizationId
			JOIN lookup.WeeklySchedule w ON o.ScheduleId = w.Id
			WHERE m.MeetingId = @MeetingId
				AND CONVERT(TIME, m.MeetingDate) = CONVERT(TIME, w.MeetingTime)
				AND w.[Day] = (DATEPART(weekday, m.MeetingDate) - 1))
		THEN 1 ELSE 0 END


	DECLARE	@isrecentvisitor BIT,
			@isinservice BIT,
			@isoffsite BIT,
			@issamehour bit

	SELECT @isrecentvisitor = CASE WHEN exists(
				SELECT NULL FROM Attend
				WHERE PeopleId = @PeopleId
				AND AttendanceFlag = 1
				AND MeetingDate >= @dt
				AND MeetingDate <= @meetdt
				AND OrganizationId = @orgid
				AND AttendanceTypeId IN (50, 60)) -- new and recent
			THEN 1 ELSE 0 END

	SELECT @isinservice = CASE WHEN exists(
				SELECT NULL FROM dbo.OrganizationMembers om
				JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
				WHERE om.PeopleId = @PeopleId
				AND om.OrganizationId <> @orgid
				AND om.MemberTypeId = 500 -- inservice member
				AND o.ScheduleId = @schedid)
			THEN 1 ELSE 0 END
			
	SELECT @isoffsite = CASE WHEN exists(
				SELECT NULL FROM dbo.OrganizationMembers om
				JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
				WHERE om.PeopleId = @PeopleId
				AND om.OrganizationId <> @orgid
				AND o.AttendClassificationId = 2 -- offsite
				AND o.FirstMeetingDate <= @meetdt
				AND @meetdt <= o.LastMeetingDate)
			THEN 1 ELSE 0 END

	SELECT @issamehour = CASE WHEN exists(
				SELECT NULL FROM dbo.OrganizationMembers om
				JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
				WHERE om.PeopleId = @PeopleId
				AND om.OrganizationId <> @orgid
				AND om.OrganizationId = @bfclassid
				AND o.ScheduleId = @schedid)
			THEN 1 ELSE 0 END
			
	SELECT	@attendedElsewhere AttendedElsewhere, 
			@membertypeid MemberTypeId, 
			@regularhour IsRegularHour, 
			@schedid ScheduleId,
			@issamehour IsSameHour,
			@isoffsite IsOffSite,
			@isrecentvisitor IsRecentVisitor,
			@name Name
	
	-- Attend if any
	SELECT * FROM dbo.Attend
	WHERE MeetingId = @MeetingId AND PeopleId = @PeopleId
		
	-- the meeting
	SELECT * FROM dbo.Meetings
	WHERE MeetingId = @MeetingId
	
	-- normal class membership
	SELECT * FROM dbo.OrganizationMembers
	WHERE PeopleId = @PeopleId AND OrganizationId = @bfclassid
	
	-- inservice class member
	SELECT * FROM dbo.OrganizationMembers om
	JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
	WHERE om.PeopleId = @PeopleId
	AND om.OrganizationId <> @orgid
	AND om.MemberTypeId = 500 -- inservice member
	AND o.ScheduleId = @schedid
	
		
END

GO
