SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[CsvTable](@csv VARCHAR(4000))
RETURNS 
@tbl TABLE (id int NOT NULL)
AS
BEGIN
	declare @pos int
	declare @val varchar(1000)

	set @csv = @csv + ','

WHILE PATINDEX('%,%',@csv) > 0
BEGIN

	SELECT @pos = PATINDEX('%,%', @csv)
	SELECT @val = left(@csv, @pos - 1)

	INSERT INTO @tbl SELECT @val
	SELECT @csv = STUFF(@csv, 1, @pos, '')
END

	RETURN 
END

GO
