SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE disc.ForumThread (@ThreadId int, @UserId nvarchar(50)) 
AS
	SELECT        e.Id, e.Title, e.ReplyToId, e.DisplayOrder, e.DisplayDepth, e.CreatedOn, 
		CAST(CASE WHEN r.userid IS NULL THEN 0 ELSE 1 END AS bit) AS IsRead, 
	                         e.CreatedBy, e.ForumId
	FROM            dbo.ForumEntry AS e LEFT OUTER JOIN
	                         dbo.ForumUserRead AS r ON e.Id = r.ForumEntryId AND (r.UserId = @UserId)
	WHERE        (e.ThreadId = @ThreadId) 
	ORDER BY e.DisplayOrder

	RETURN
GO
