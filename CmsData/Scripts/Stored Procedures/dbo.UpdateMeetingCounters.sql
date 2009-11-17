SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[UpdateMeetingCounters](@mid INT)
AS
BEGIN
	DECLARE @gf BIT, @numPresent INT, @numNewVisit INT, @numMembers INT, @numVstMembers INT, @numRepeatVst INT
	SELECT @gf = GroupMeetingFlag FROM dbo.Meetings WHERE MeetingId = @mid
	IF @gf = 0
	BEGIN
		SELECT @numPresent = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1
		SELECT @numNewVisit = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId = 60
		SELECT @numMembers = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId IN (10,20,30)
		SELECT @numVstMembers = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId = 40
		SELECT @numRepeatVst = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId = 50
		UPDATE dbo.Meetings SET
			NumMembers = @numMembers,
			NumPresent = @numPresent,
			NumNewVisit = @numNewVisit,
			NumVstMembers = @numVstMembers,
			NumRepeatVst = @numRepeatVst
			WHERE MeetingId = @mid
	END
	ELSE
	BEGIN
		SELECT @numNewVisit = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId = 60
		SELECT @numRepeatVst = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId = 50
		SELECT @numVstMembers = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId = 40
		UPDATE dbo.Meetings SET
			NumNewVisit = @numNewVisit,
			NumVstMembers = @numVstMembers,
			NumRepeatVst = @numRepeatVst
			WHERE MeetingId = @mid
	END
END
GO
