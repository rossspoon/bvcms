SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateAttendStr] @orgid INT, @pid INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-----------------------------------------------------------------
    DECLARE @yearago DATETIME
    DECLARE @lastmeet DATETIME 
    DECLARE @tct INT 
    DECLARE @act INT
    DECLARE @pct REAL
		
    SELECT @lastmeet = MAX(MeetingDate) FROM dbo.Meetings
    WHERE OrganizationId = @orgid
    
    SELECT @yearago = DATEADD(year,-1,@lastmeet)
    
	SELECT @tct = COUNT(*) FROM dbo.Attend
     WHERE PeopleId = @pid
       AND OrganizationId = @orgid
       AND EffAttendFlag IS NOT NULL
       AND MeetingDate >= @yearago
       
    SELECT @act = COUNT(*) FROM dbo.Attend
     WHERE PeopleId = @pid
       AND OrganizationId = @orgid
       AND EffAttendFlag = 1
       AND MeetingDate >= @yearago
       
       
	if @tct = 0
			select @pct = 0
		else
			SELECT @pct = @act * 100.0 / @tct
			
	-------------------------------------------------------
	DECLARE @a nvarchar(200) -- attendance string
	set @a = ''
			
	DECLARE @mindt DATETIME, @dt DATETIME 

	SELECT @mindt = MIN(MeetingDate), @dt = MAX(MeetingDate) FROM dbo.Meetings WHERE OrganizationId = @orgid
	SELECT @dt = DATEADD(yy, -1, @dt)
	IF (@dt > @mindt)
		SELECT @mindt = @dt
		FROM dbo.Attend
		WHERE OrganizationId = @orgid AND PeopleId = @pid

	SELECT @a = 
		CASE 
		WHEN a.EffAttendFlag IS NULL THEN
			CASE a.AttendanceTypeId
			WHEN 20 THEN 'V'
			WHEN 70 THEN 'I'
			WHEN 90 THEN 'G'
			WHEN 80 THEN 'O'
			WHEN 110 THEN '*'
			ELSE '*'
			END
		WHEN a.EffAttendFlag = 1 THEN 'P'
		ELSE '.'
		END + @a
	FROM dbo.Attend a
	WHERE a.MeetingDate >= @dt AND a.PeopleId = @pid AND a.OrganizationId = @orgid
	ORDER BY MeetingDate DESC
	
	----------------------------------------------------------------
	DECLARE @lastattend DATETIME
	SELECT @lastattend = MAX(a.MeetingDate) FROM dbo.Attend a
	WHERE a.AttendanceFlag = 1 AND a.OrganizationId = @orgid AND a.PeopleId = @pid

	
	--------------------------------------------	
	
		
	UPDATE dbo.OrganizationMembers SET
		AttendPct = @pct,
		AttendStr = @a,
		LastAttended = @lastattend
	WHERE OrganizationId = @orgid AND PeopleId = @pid

END

GO
