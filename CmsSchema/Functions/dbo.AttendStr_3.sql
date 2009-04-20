SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[AttendStr_3](@orgid int, @pid int)
RETURNS varchar(100)
BEGIN
	DECLARE @a nvarchar(200) -- attendance string
	set @a = ''
			
	DECLARE @mindt DATETIME, @dt DATETIME 

	SELECT @mindt = MIN(MeetingDate), @dt = MAX(MeetingDate) FROM dbo.Attend WHERE OrganizationId = @orgid
	SELECT @dt = DATEADD(yy, -1, @dt)
	IF (@dt > @mindt)
		SELECT @mindt = @dt
	
	FROM dbo.Attend
	WHERE OrganizationId = @orgid AND PeopleId = @pid

	SELECT @a = 
	CASE a.AttendanceTypeId
	WHEN 70 THEN 'I'
	WHEN 90 THEN 'G'
	WHEN 80 THEN 'O'
	WHEN 110 THEN '*'
	WHEN 0 THEN '.'
	ELSE CASE a.AttendanceFlag
		WHEN NULL THEN '*'
		WHEN 1 THEN 'P'
		WHEN 0 THEN '.'
		END
	END + @a
	FROM dbo.Attend a
	WHERE a.MeetingDate >= @dt AND a.PeopleId = @pid AND a.OrganizationId = @orgid
	ORDER BY MeetingDate DESC

	RETURN @a
END


GO
