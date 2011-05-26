SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
create function dbo.SundayForDate(@dt DATETIME) 
returns		datetime
as
begin
	declare	 @stdt	datetime
	declare	 @fdt	datetime
	select @fdt = convert(datetime,-53690+((1+5)%7))
	select @stdt = dateadd(dd,(datediff(dd,@fdt,@dt)/7)*7,@fdt)
	return @stdt
	
END
GO
