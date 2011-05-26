SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[AttendMeetingInfo]
( @MeetingId INT, @PeopleId INT)
AS
BEGIN
	DECLARE @orgid INT
			,@meetingdate DATETIME
			,@meetdt DATE
			,@tm TIME
			,@dt DATETIME
			,@regularhour BIT
			,@membertypeid INT
			,@schedid INT
			,@attendedelsewhere INT
			,@allowoverlap BIT

	SELECT @regularhour = CASE WHEN EXISTS(
		SELECT null
			FROM dbo.Meetings m
			JOIN dbo.Organizations o ON m.OrganizationId = o.OrganizationId
			WHERE m.MeetingId = @MeetingId
				AND CONVERT(TIME, m.MeetingDate) = CONVERT(TIME, o.MeetingTime)
				AND SchedDay = (DATEPART(weekday, m.MeetingDate) - 1))
		THEN 1 ELSE 0 END

	SELECT
		@orgid = m.OrganizationId,
		@schedid = o.ScheduleId,
		@dt = DATEADD(DAY, o.RollSheetVisitorWks * -7, m.MeetingDate),
		@meetingdate = m.MeetingDate,
		@allowoverlap = o.AllowAttendOverlap,
		@membertypeid = (SELECT om.MemberTypeId
					FROM dbo.OrganizationMembers om
					WHERE om.OrganizationId = o.OrganizationId
					AND om.PeopleId = @PeopleId)

	FROM dbo.Meetings m
	JOIN dbo.Organizations o ON m.OrganizationId = o.OrganizationId
	WHERE m.MeetingId = @MeetingId

	DECLARE @name VARCHAR(50), @bfclassid INT

	SELECT @name = p.[Name], @bfclassid = BibleFellowshipClassId
	FROM dbo.People p
	WHERE PeopleId = @PeopleId

	SELECT
		@meetdt = CONVERT(DATE, m.MeetingDate),
		@tm = CONVERT(TIME, m.MeetingDate)
	FROM dbo.Meetings m
	WHERE m.MeetingId = @MeetingId
		
	IF @dt IS NULL
		SELECT @dt = DATEADD(DAY, 3 * -7, @meetdt)

	DECLARE	@isrecentvisitor BIT
			,@isoffsite BIT
			,@otherattend INT
			,@bfcattend INT
			,@bfcid INT
			--,@isinservice BIT
			--,@issamehour bit

	SELECT @isrecentvisitor = CASE WHEN exists(
				SELECT NULL FROM Attend
				WHERE PeopleId = @PeopleId
				AND AttendanceFlag = 1
				AND MeetingDate >= @dt
				AND MeetingDate <= @meetdt
				AND OrganizationId = @orgid
				AND AttendanceTypeId IN (50, 60)) -- new and recent
			THEN 1 ELSE 0 END

	--SELECT @isinservice = CASE WHEN exists(
	--			SELECT NULL FROM dbo.OrganizationMembers om
	--			JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
	--			WHERE om.PeopleId = @PeopleId
	--			AND om.OrganizationId <> @orgid
	--			AND om.MemberTypeId = 500 -- inservice member
	--			AND o.ScheduleId = @schedid)
	--		THEN 1 ELSE 0 END
			
	SELECT @isoffsite = CASE WHEN exists(
				SELECT NULL FROM dbo.OrganizationMembers om
				JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
				WHERE om.PeopleId = @PeopleId
				AND om.OrganizationId <> @orgid
				AND o.AttendClassificationId = 2 -- offsite
				AND o.FirstMeetingDate <= @meetdt
				AND @meetdt <= o.LastMeetingDate)
			THEN 1 ELSE 0 END

	SELECT TOP 1 @otherattend = ae.AttendId
	FROM Attend ae
	JOIN dbo.Organizations o ON ae.OrganizationId = o.OrganizationId
	WHERE ae.PeopleId = @PeopleId
	AND ae.MeetingDate = @meetingdate
	AND ae.OrganizationId <> @orgid
	
	-- BFC class membership
	SELECT @bfcid = om.OrganizationId FROM dbo.OrganizationMembers om
	JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
	WHERE om.PeopleId = @PeopleId 
	AND om.OrganizationId <> @orgid
	AND o.ScheduleId = @schedid AND @regularhour = 1
	AND (om.OrganizationId = @bfclassid OR om.MemberTypeId = 500) -- regular or InSvc

	-- BFC Attendance at same time
	SELECT @bfcattend = a.AttendId FROM dbo.Attend a
	JOIN dbo.OrganizationMembers om ON a.OrganizationId = om.OrganizationId AND a.PeopleId = om.PeopleId
	JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
	WHERE o.ScheduleId = @schedid AND @regularhour = 1
	AND a.MeetingDate = @meetingdate
	AND om.OrganizationId <> @orgid
	AND om.OrganizationId = @bfcid
	AND a.PeopleId = @PeopleId

	-- attended elsewhere at same time
	SELECT TOP 1 @attendedelsewhere = ae.AttendId
	FROM Attend ae
	JOIN dbo.Organizations o ON ae.OrganizationId = o.OrganizationId
	WHERE ae.PeopleId = @PeopleId
	AND ae.AttendanceFlag = 1
	AND ae.MeetingDate = @meetingdate
	AND ae.OrganizationId NOT IN (@orgid, @bfcid)
	AND o.AllowAttendOverlap <> 1
	AND @allowoverlap <> 1

-- The returned records:
			
	SELECT
		 @attendedelsewhere AttendedElsewhere
		,@membertypeid MemberTypeId
		,@isoffsite IsOffSite
		,@isrecentvisitor IsRecentVisitor
		,@name Name
		,@bfclassid BFClassId
	
	-- Attend if any
	SELECT * FROM dbo.Attend
	WHERE MeetingId = @MeetingId AND PeopleId = @PeopleId
		
	-- the meeting
	SELECT * FROM dbo.Meetings
	WHERE MeetingId = @MeetingId
	
	-- Related VIP Attendance
	SELECT v.*
	FROM Attend v
	JOIN dbo.OrganizationMembers om ON v.OrganizationId = om.OrganizationId AND v.PeopleId = om.PeopleId
	WHERE v.PeopleId = @PeopleId
	AND v.MeetingDate = @meetingdate
	AND v.OrganizationId <> @orgid
	AND om.MemberTypeId = 700 -- vip
	AND @orgid = @bfclassid
	
	-- BFC class membership 
	SELECT * FROM dbo.OrganizationMembers
	WHERE OrganizationId = @bfcid
	AND PeopleId = @PeopleId
	
	-- BFC Attendance at same time
	SELECT * FROM dbo.Attend
	WHERE AttendId = @bfcattend

	-- BFC Meeting at same time
	SELECT m.* FROM dbo.Meetings m
	JOIN dbo.Attend a ON m.MeetingId = a.MeetingId
	WHERE AttendId = @bfcattend
	
END
GO
