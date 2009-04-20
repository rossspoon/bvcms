
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION dbo.AttendPct_2(@orgid int, @pid int) 
RETURNS Real
AS
BEGIN


--DECLARE @orgid INT, @pid INT
--SELECT @orgid = 80092, @pid = 828612

    DECLARE @pct REAL
    DECLARE @yearago DATETIME
    DECLARE @lastmeet DATETIME 
    DECLARE @tct INT 
    DECLARE @act INT
    
    SELECT @lastmeet = MAX(MeetingDate) FROM dbo.Meetings
    WHERE OrganizationId = @orgid
    
    SELECT @yearago = DATEADD(year,-1,@lastmeet)
    
	SELECT @tct = COUNT(*) FROM dbo.Attend
     WHERE PeopleId = @pid
       AND OrganizationId = @orgid
       AND AttendanceFlag IS NOT NULL
       AND MeetingDate >= @yearago
       
    SELECT @act = COUNT(*) FROM dbo.Attend
     WHERE PeopleId = @pid
       AND OrganizationId = @orgid
       AND AttendanceFlag = 1
       AND MeetingDate >= @yearago
       
       
	if @tct = 0
			select @pct = 0
		else
			SELECT @pct = @act * 100.0 / @tct

	RETURN @pct
    
END


GO
