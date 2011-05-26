SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[GetTodaysMeetingHour]
    (
      @orgid INT ,
      @thisday INT
    )
RETURNS DATETIME
AS 
    BEGIN
        DECLARE 
			@DefaultHour DATETIME,
            @DefaultDay INT,
            @prevMidnight DATETIME
            
        IF @thisday IS NULL
			SELECT @thisday = DATEPART(dw, GETDATE()) - 1
			
		DECLARE @plusdays INT = @thisday - (DATEPART(dw, GETDATE())-1) + 7
		IF @plusdays > 6
			SELECT @plusdays = @plusdays - 7
		SELECT @prevMidnight = dateadd(dd,0, datediff(dd,0,GETDATE())) + @plusdays
        
        SELECT  @DefaultHour = MeetingTime,
                @DefaultDay = ISNULL(SchedDay, 0)
        FROM    dbo.Organizations
        WHERE   OrganizationId = @orgid
        
        DECLARE @meetingid INT, @meetingdate DATETIME
        
		DECLARE @DefaultTime DATETIME = DATEADD(dd, -DATEDIFF(dd, 0, @DefaultHour), @DefaultHour)
		
		/*
		SELECT @meetingid = dbo.GetTodaysMeetingId(@orgid, @thisday)
		
		IF @meetingid IS NOT NULL
			SELECT @meetingdate = MeetingDate FROM dbo.Meetings
			WHERE MeetingId = @meetingid
		ELSE */ 
		IF @DefaultDay = @thisday
			SELECT @meetingdate = @prevMidnight + @DefaultTime
					
        RETURN @meetingdate
    END
GO
