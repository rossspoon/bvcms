SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[DayAndTime] (@dt DATETIME)
RETURNS VARCHAR(20)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @daytime VARCHAR(20)

SELECT @daytime =
	CASE DATEPART(dw,@dt)
    WHEN 1 THEN 'Sunday'
    WHEN 2 THEN 'Monday'
    WHEN 3 THEN 'Tuesday'
    WHEN 4 THEN 'Wednesday'
    WHEN 5 THEN 'Thursday'
    WHEN 6 THEN 'Friday'
    WHEN 7 THEN 'Saturday'
    END + ' ' + SUBSTRING(CONVERT(varchar,@dt,0),13,7)
    
	RETURN @daytime

END


GO
