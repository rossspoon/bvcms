

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE FUNCTION [dbo].[InBFClass]
	(
	@pid int
	)
RETURNS bit
AS
	BEGIN
	declare @mem BIT
	
	IF dbo.BibleFellowshipClassId(@pid) IS NOT NULL
		select @mem = 1

	RETURN isnull(@mem, 0)

	END





GO
