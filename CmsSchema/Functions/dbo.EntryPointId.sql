
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[EntryPointId] ( @pid INT )
RETURNS INT 
AS
BEGIN
	DECLARE @ret INT 

	SELECT TOP 1 @ret = e.Id FROM 
	dbo.Attend a 
	JOIN dbo.Meetings m ON a.MeetingId = m.MeetingId
	JOIN dbo.Organizations o ON m.OrganizationId = o.OrganizationId
	LEFT OUTER JOIN lookup.EntryPoint e ON o.EntryPointId = e.Id
	WHERE a.PeopleId = @pid
	ORDER BY a.MeetingDate
	
	RETURN @ret
END




GO
