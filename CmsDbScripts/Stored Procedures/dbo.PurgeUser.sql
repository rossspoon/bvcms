SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[PurgeUser](@pid INT)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE dbo.UserRole
	FROM dbo.UserRole ur
	JOIN dbo.Users u ON u.UserId = ur.UserId
	WHERE u.PeopleId = @pid
	
	DELETE disc.UserGroupRole
	FROM disc.UserGroupRole ug
	JOIN dbo.Users u ON ug.UserId = u.UserId
	WHERE u.PeopleId = @pid
	
	delete disc.VerseCategoryXref
	from disc.VerseCategoryXref vcx
	join disc.VerseCategory vc on vc.id = vcx.VerseCategoryId
	JOIN disc.Verse v ON vcx.VerseId = v.id
	join dbo.Users u on u.UserId = v.CreatedBy
	where u.PeopleId = @pid
	
	delete disc.VerseCategoryXref
	from disc.VerseCategoryXref vcx
	join disc.VerseCategory vc on vc.id = vcx.VerseCategoryId
	JOIN disc.Verse v ON vcx.VerseId = v.id
	join dbo.Users u on u.UserId = vc.CreatedBy
	where u.PeopleId = @pid
	
	delete disc.VerseCategory
	FROM disc.VerseCategory vc
	join dbo.Users u on u.UserId = vc.CreatedBy
	where u.PeopleId = @pid
	
	DELETE disc.Verse
	FROM disc.Verse v
	JOIN dbo.Users u ON v.CreatedBy = u.UserId
	WHERE u.PeopleId = @pid
	
	DELETE dbo.ActivityLog
	FROM dbo.ActivityLog a
	JOIN dbo.Users u ON a.UserId = u.UserId
	WHERE u.PeopleId = @pid
	
	delete disc.BlogCategoryXref
	from disc.BlogCategoryXref bcx
	join disc.BlogPost p on p.Id = bcx.BlogPostId
	join disc.Blog b on b.Id = p.BlogId
	join dbo.Users u on u.UserId = b.OwnerId
	where u.PeopleId = @pid
	
	delete disc.BlogCategoryXref
	from disc.BlogCategoryXref bcx
	join disc.BlogPost p on p.Id = bcx.BlogPostId
	join dbo.Users u ON p.PosterId = u.UserId
	where u.PeopleId = @pid
	
	delete disc.BlogComment
	from disc.BlogComment bc
	join dbo.Users u on u.UserId = bc.PosterId
	where u.PeopleId = @pid
	
	delete disc.BlogComment
	from disc.BlogComment bc
	JOIN disc.BlogPost p ON bc.BlogPostId = p.Id
	JOIN disc.Blog b ON p.BlogId = b.Id
	join dbo.Users u on u.UserId = b.OwnerId
	where u.PeopleId = @pid
	
	delete disc.BlogComment
	from disc.BlogComment bc
	JOIN disc.BlogPost p ON bc.BlogPostId = p.Id
	join dbo.Users u on u.UserId = p.PosterId
	where u.PeopleId = @pid
	
	delete disc.BlogPost
	FROM disc.BlogPost p
	JOIN disc.Blog b ON p.BlogId = b.Id
	join dbo.Users u on u.UserId = b.OwnerId
	where u.PeopleId = @pid
	
	delete disc.BlogNotify
	from disc.BlogNotify n
	join disc.Blog b ON n.BlogId = b.Id
	join dbo.Users u on u.UserId = b.OwnerId
	where u.PeopleId = @pid
	
	delete disc.Blog
	FROM disc.Blog b
	join dbo.Users u on u.UserId = b.OwnerId
	where u.PeopleId = @pid
	
	DELETE disc.BlogPost
	FROM disc.BlogPost p
	join dbo.Users u on u.UserId = p.PosterId
	where u.PeopleId = @pid
	
	DELETE disc.ForumUserRead
	FROM disc.ForumUserRead ur
	JOIN disc.ForumEntry fe ON ur.ForumEntryId = fe.Id
	JOIN disc.Forum f ON fe.ForumId = f.Id
	JOIN dbo.Users u ON f.CreatedBy = u.UserId
	WHERE u.PeopleId = @pid
	
	DELETE disc.ForumUserRead
	FROM disc.ForumUserRead ur
	JOIN disc.ForumEntry fe ON ur.ForumEntryId = fe.Id
	JOIN dbo.Users u ON fe.CreatedBy = u.UserId
	WHERE u.PeopleId = @pid
	
	DELETE disc.ForumUserRead
	FROM disc.ForumUserRead ur
	JOIN dbo.Users u ON ur.UserId = u.UserId
	WHERE u.PeopleId = @pid
	
	DELETE disc.ForumEntry
	FROM disc.ForumEntry fe
	JOIN disc.Forum f ON fe.ForumId = f.Id
	JOIN dbo.Users u ON f.CreatedBy = u.UserId
	WHERE u.PeopleId = @pid
	
	DELETE disc.Forum
	FROM disc.Forum f
	JOIN dbo.Users u ON f.CreatedBy = u.UserId
	WHERE u.PeopleId = @pid
	
	DELETE disc.TemporaryToken
	FROM disc.TemporaryToken tt
	JOIN dbo.Users u ON tt.CreatedBy = u.UserId
	WHERE u.PeopleId = @pid
	
	delete dbo.Preferences
	from dbo.Preferences p
	join dbo.Users u on u.UserId = p.UserId
	where u.PeopleId = @pid
	
	delete disc.PageVisit
	from disc.PageVisit pv
	join dbo.Users u on u.UserId = pv.UserId
	where u.PeopleId = @pid
	
	delete dbo.UserCanEmailFor
	from dbo.UserCanEmailFor um
	join dbo.Users u on u.UserId = um.UserId
	where u.PeopleId = @pid
	
	delete dbo.UserCanEmailFor
	from dbo.UserCanEmailFor um
	join dbo.Users u ON um.CanEmailFor = u.UserId
	where u.PeopleId = @pid
	
	DELETE dbo.VolunteerForm
	FROM dbo.VolunteerForm vf
	JOIN dbo.Users u ON vf.UploaderId = u.UserId
	WHERE u.PeopleId = @pid
	
	DELETE dbo.Coupons
	FROM dbo.Coupons c
	JOIN dbo.Users u ON c.UserId = u.UserId
	WHERE u.PeopleId = @pid
	
	DELETE disc.PageContent
	FROM disc.PageContent pc
	JOIN dbo.Users u ON pc.CreatedById = u.UserId
	WHERE u.PeopleId = @pid
	
	DELETE disc.PageContent
	FROM disc.PageContent pc
	JOIN dbo.Users u ON pc.ModifiedById = u.UserId
	WHERE u.PeopleId = @pid
	
	DELETE dbo.Users
	WHERE PeopleId = @pid
	
END
GO
