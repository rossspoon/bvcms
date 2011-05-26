SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[DaysBetween12Attend](@pid INT, @progid INT, @divid INT, @orgid INT, @lookback int)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @days INT, @d1 DATETIME, @d2 DATETIME, @d0 DATETIME
	
	SELECT @d0 = DATEADD(d, -@lookback, GETDATE())

	-- Add the T-SQL statements to compute the return value here
	SELECT TOP 1 @d1 = MeetingDate FROM Attend a
	WHERE a.PeopleId = @pid
	AND a.AttendanceFlag = 1 AND a.AttendanceTypeId = 60
	ORDER BY a.MeetingDate
	
	IF (@d1 < @d0)
		SET @d1 = NULL -- does not match criteria
	
	SELECT TOP 1 @d2 = MeetingDate FROM Attend a
	WHERE a.PeopleId = @pid AND (@orgid = 0 OR @orgid = a.OrganizationId)
	AND (@divid = 0 OR EXISTS(SELECT NULL FROM dbo.DivOrg do WHERE do.OrgId = a.OrganizationId AND do.DivId = @divid))
	AND (@progid = 0 OR EXISTS(SELECT NULL FROM dbo.DivOrg do JOIN dbo.Division d ON do.DivId = d.Id JOIN dbo.ProgDiv pd ON d.Id = pd.DivId WHERE pd.ProgId = @progid))
	AND a.AttendanceFlag = 1
	AND DATEDIFF(d, @d1, a.MeetingDate) >= 1
	ORDER BY a.MeetingDate
	
	SELECT @days = DATEDIFF(d, @d1, @d2)

	-- Return the result of the function
	RETURN @days

END



GO
