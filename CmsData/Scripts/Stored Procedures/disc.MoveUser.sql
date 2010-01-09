SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE disc.MoveUser(@frid INT, @toid INT) 
AS
BEGIN
UPDATE dbo.Blog SET OwnerId = @toid 
WHERE OwnerId = @frid

UPDATE dbo.BlogComment SET PosterId = @toid 
WHERE @frid = PosterId

UPDATE dbo.BlogNotify SET UserId = @toid 
WHERE @frid = dbo.BlogNotify.UserId

UPDATE dbo.BlogPost SET PosterId = @toid 
WHERE @frid = PosterId

UPDATE dbo.Forum SET CreatedBy = @toid 
WHERE @frid = dbo.Forum.CreatedBy

UPDATE dbo.ForumNotify SET UserId = @toid 
WHERE @frid = dbo.ForumNotify.UserId

UPDATE dbo.ForumUserRead SET UserId = @toid 
WHERE @frid = dbo.ForumUserRead.UserId

UPDATE dbo.PageContent SET CreatedById = @toid
WHERE @frid = CreatedById
UPDATE dbo.PageContent SET ModifiedById = @toid
WHERE @frid = ModifiedById

UPDATE dbo.PageVisit SET UserId = @toid 
WHERE @frid = dbo.PageVisit.UserId

UPDATE dbo.ParaContent SET CreatedById = @toid 
WHERE @frid = CreatedById

UPDATE dbo.PendingNotifications SET UserId = @toid 
WHERE @frid = dbo.PendingNotifications.UserId

UPDATE dbo.PodCast SET UserId = @toid 
WHERE @frid = dbo.PodCast.UserId

UPDATE dbo.PrayerSlot SET UserId = @toid 
WHERE @frid = dbo.PrayerSlot.UserId

UPDATE dbo.TemporaryToken SET CreatedBy = @toid 
WHERE @frid = CreatedBy

UPDATE dbo.UserGroupRole SET UserId = @toid 
WHERE @frid = dbo.UserGroupRole.UserId

UPDATE dbo.Verse SET CreatedBy = @toid 
WHERE @frid = CreatedBy

UPDATE dbo.VerseCategory SET CreatedBy = @toid 
WHERE @frid = CreatedBy

DELETE dbo.Users WHERE UserId = @frid
END
GO
