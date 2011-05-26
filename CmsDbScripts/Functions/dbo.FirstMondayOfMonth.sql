SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[FirstMondayOfMonth] (@inputDate DATETIME)RETURNS DATETIME BEGIN     RETURN DATEADD(wk, DATEDIFF(wk, 0, dateadd(dd, 6 - datepart(day, @inputDate), @inputDate)), 0)  END
GO
