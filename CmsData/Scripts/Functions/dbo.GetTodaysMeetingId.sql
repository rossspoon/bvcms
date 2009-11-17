SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[GetTodaysMeetingId]
    (
      @orgid INT ,
      @thisday INT
    )
RETURNS INT 
AS 
    BEGIN
        DECLARE 
			@DefaultHour DATETIME,
            @DefaultDay INT,
            @prevMidnight DATETIME,
            @ninetyMinutesAgo DATETIME,
            @nextMidnight DATETIME
            
        IF @thisday IS NULL
			SELECT @thisday = DATEPART(dw, GETDATE()) - 1
			
		DECLARE @plusdays INT = @thisday - (DATEPART(dw, GETDATE())-1) + 7
		IF @plusdays > 6
			SELECT @plusdays = @plusdays - 7
		SELECT @prevMidnight = dateadd(dd,0, datediff(dd,0,GETDATE())) + @plusdays
        SELECT @nextMidnight = @prevMidnight + 1
        SELECT @ninetyMinutesAgo = DATEADD(mi, -90, GETDATE())
        
        SELECT  @DefaultHour = MeetingTime,
                @DefaultDay = SchedDay
        FROM    dbo.Organizations
        WHERE   OrganizationId = @orgid
        
        DECLARE @meetingid INT, @meetingdate DATETIME
        
        SELECT TOP 1 @meetingid = MeetingId FROM dbo.Meetings
        WHERE OrganizationId = @orgid
        AND MeetingDate >= @ninetyMinutesAgo
        AND MeetingDate < @nextMidnight
        ORDER BY MeetingDate
        
        IF @meetingid IS NULL
			SELECT TOP 1 @meetingid = MeetingId FROM dbo.Meetings
			WHERE OrganizationId = @orgid
			AND MeetingDate >= @prevMidnight
			AND MeetingDate < @nextMidnight
			ORDER BY MeetingDate
			
		RETURN @meetingid

    END
GO
