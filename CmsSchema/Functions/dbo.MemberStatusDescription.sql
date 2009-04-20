
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[MemberStatusDescription](@pid int)
RETURNS varchar(50)
AS
	BEGIN
	declare @desc varchar(50)
	select @desc = m.description from lookup.memberstatus m
	join dbo.People p on p.MemberStatusId = m.id
	where p.PeopleId = @pid
	return @desc
	END


GO
