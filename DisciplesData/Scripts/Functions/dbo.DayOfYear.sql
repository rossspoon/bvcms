SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE   FUNCTION [dbo].[DayOfYear] ( @Date datetime ) 
RETURNS int
 WITH SCHEMABINDING
AS BEGIN
	declare @d1 as datetime
	set @d1 = '1/1/' + CAST(YEAR(@Date) AS CHAR(4))
	return (select DATEDIFF (day,@d1,@Date)+1)
END
GO
