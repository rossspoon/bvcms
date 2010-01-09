SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- set the from userid here

CREATE PROCEDURE [disc].[DeleteUser](@frid int)
AS
BEGIN

DELETE disc.BlogCategoryXref
FROM disc.BlogPost bp JOIN disc.BlogCategoryXref bcx
ON bp.id = bcx.BlogPostId
WHERE @frid = PosterId

DELETE disc.BlogComment
WHERE @frid = PosterId

DELETE disc.BlogComment
FROM disc.BlogPost bp JOIN disc.BlogComment bcx
ON bp.id = bcx.BlogPostId
WHERE @frid = bp.PosterId

DELETE disc.BlogNotify
WHERE @frid = UserId

DELETE disc.BlogPost
WHERE @frid = PosterId

DELETE disc.Blog 
WHERE OwnerId = @frid

DELETE disc.Forum
WHERE @frid = CreatedBy

DELETE disc.ForumNotify
WHERE @frid = UserId

DELETE disc.ForumUserRead
WHERE @frid = UserId

DELETE disc.PageContent 
WHERE @frid = CreatedById
DELETE disc.PageContent
WHERE @frid = ModifiedById

DELETE disc.PageVisit
WHERE @frid = UserId

DELETE disc.ParaContent
WHERE @frid = CreatedById

DELETE disc.PendingNotifications
WHERE @frid = UserId

DELETE disc.PodCast
WHERE @frid = UserId

DELETE disc.PrayerSlot
WHERE @frid = UserId

DELETE disc.TemporaryToken
WHERE @frid = CreatedBy

DELETE disc.UserGroupRole
WHERE @frid = UserId

DELETE dbo.UserRole
WHERE @frid = UserId

DELETE disc.Verse
WHERE @frid = CreatedBy

DELETE disc.VerseCategoryXref
FROM disc.VerseCategory vc JOIN disc.VerseCategoryXref vcx
ON vc.id = vcx.VerseCategoryId
WHERE @frid = CreatedBy

DELETE disc.VerseCategory
WHERE @frid = CreatedBy

DELETE dbo.Users WHERE UserId = @frid
END
GO
