
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[PurgePerson](@pid INT)
AS
BEGIN
	BEGIN TRY 
		BEGIN TRANSACTION 
		DECLARE @fid INT, @pic INT
		DELETE FROM dbo.OrgMemMemTags WHERE PeopleId = @pid
		DELETE FROM dbo.OrganizationMembers WHERE PeopleId = @pid
		DELETE FROM dbo.BadET WHERE PeopleId = @pid
		DELETE FROM dbo.EnrollmentTransaction WHERE PeopleId = @pid
		DELETE FROM dbo.MOBSReg WHERE PeopleId = @pid
		DELETE FROM dbo.CardIdentifiers WHERE PeopleId = @pid
		DELETE FROM dbo.CheckInTimes WHERE PeopleId = @pid
		DELETE FROM disc.PendingNotifications WHERE PeopleId = @pid
		DELETE FROM disc.PrayerSlot WHERE PeopleId = @pid
		
		DELETE disc.VerseCategoryXref
		FROM disc.VerseCategoryXref x 
		JOIN disc.VerseCategory c ON x.VerseCategoryId = c.id
		JOIN dbo.Users u ON c.CreatedBy = u.UserId
		WHERE u.PeopleId = @pid
		
		DELETE disc.VerseCategory
		FROM disc.VerseCategory c
		JOIN dbo.Users u ON c.CreatedBy = u.UserId
		WHERE u.PeopleId = @pid
		
		DELETE disc.BlogCategoryXref
		FROM disc.BlogCategoryXref x
		JOIN disc.BlogPost bp ON x.BlogPostId = bp.Id
		JOIN dbo.Users u ON bp.PosterId = u.UserId
		WHERE u.PeopleId = @pid
		
		DELETE disc.BlogPost
		FROM disc.BlogPost bp
		JOIN dbo.Users u ON bp.PosterId = u.UserId
		WHERE u.PeopleId = @pid

		DECLARE @t TABLE(id int)
		INSERT INTO @t(id) SELECT MeetingId FROM dbo.Attend a WHERE a.PeopleId = @pid
		
		DELETE FROM dbo.Attend WHERE PeopleId = @pid
		
		DECLARE cur CURSOR FOR SELECT id FROM @t
		OPEN cur
		DECLARE @mid int
		FETCH NEXT FROM cur INTO @mid
		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXECUTE dbo.UpdateMeetingCounters @mid
			FETCH NEXT FROM cur INTO @mid
		END
		CLOSE cur
		DEALLOCATE cur
		
		UPDATE dbo.Contribution SET PeopleId = NULL WHERE PeopleId = @pid
		DELETE FROM dbo.VolunteerForm WHERE PeopleId = @pid
		DELETE FROM dbo.Volunteer WHERE PeopleId = @pid
		DELETE FROM dbo.Contactees WHERE PeopleId = @pid
		DELETE FROM dbo.Contactors WHERE PeopleId = @pid
		DELETE FROM dbo.TagPerson WHERE PeopleId = @pid
		DELETE FROM dbo.Task WHERE WhoId = @pid
		DELETE FROM dbo.Task WHERE OwnerId = @pid
		DELETE FROM dbo.Task WHERE CoOwnerId = @pid
		DELETE FROM dbo.TaskListOwners WHERE PeopleId = @pid
		
		DELETE FROM dbo.Preferences WHERE UserId IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		DELETE FROM dbo.ActivityLog WHERE UserId IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		DELETE FROM dbo.UserRole WHERE UserId IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		DELETE FROM dbo.UserCanEmailFor WHERE UserId IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		DELETE FROM dbo.UserCanEmailFor WHERE CanEmailFor IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		UPDATE dbo.VolunteerForm SET UploaderId = NULL WHERE UploaderId IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		
		DELETE disc.UserGroupRole
		FROM disc.UserGroupRole ugr
		JOIN dbo.Users u ON ugr.UserId = u.UserId
		WHERE u.PeopleId = @pid
		DELETE disc.PageVisit
		FROM disc.PageVisit pv
		JOIN dbo.Users u ON pv.UserId = u.UserId
		WHERE u.PeopleId = @pid
		DELETE FROM dbo.Users WHERE PeopleId = @pid
		
		DELETE FROM dbo.TagPerson WHERE id IN (SELECT Id FROM dbo.Tag WHERE PeopleId = @pid)
		DELETE FROM dbo.TagShare WHERE TagId IN (SELECT Id FROM dbo.Tag WHERE PeopleId = @pid)
		DELETE FROM dbo.TagShare WHERE PeopleId = @pid
		DELETE FROM dbo.Tag WHERE PeopleId = @pid
		
		DELETE FROM dbo.SoulMate WHERE HerId = @pid OR HimId = @pid
		DELETE FROM dbo.LoveRespect WHERE HerId = @pid OR HimId = @pid
		DELETE FROM dbo.RecReg WHERE PeopleId = @pid
		DELETE FROM dbo.VBSApp WHERE PeopleId = @pid
		
		DELETE dbo.VolInterestInterestCodes
		FROM dbo.VolInterestInterestCodes vc
		WHERE vc.PeopleId = @pid
		
		SELECT @fid = FamilyId, @pic = PictureId FROM dbo.People WHERE PeopleId = @pid
		
		UPDATE dbo.Families
		SET HeadOfHouseholdId = NULL, HeadOfHouseholdSpouseId = NULL
		WHERE FamilyId = @fid AND HeadOfHouseholdId = @pid
		OR FamilyId = @fid AND HeadOfHouseholdSpouseId = @pid
		
		DELETE FROM dbo.People WHERE PeopleId = @pid
		IF (SELECT COUNT(*) FROM dbo.People WHERE FamilyId = @fid) = 0
		BEGIN
			DELETE FROM dbo.RelatedFamilies WHERE FamilyId = @fid OR RelatedFamilyId = @fid
			DELETE FROM dbo.Families WHERE FamilyId = @fid			
		END
		DELETE FROM dbo.Picture WHERE PictureId = @pic
		COMMIT
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH 
 
END
GO
