SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE FUNCTION [dbo].[BibleFellowshipTeacher] (@pid int)
RETURNS varchar(100)
AS
	BEGIN
	declare @name varchar(100),	@oid int
	select @oid = dbo.BibleFellowshipClassId(@pid)
	SELECT  @name = dbo.OrganizationLeaderName(@oid)
	return @name
	END

GO
