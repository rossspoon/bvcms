
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
		DELETE FROM dbo.OrganizationMembers WHERE PeopleId = @pid
		DELETE FROM dbo.BadET WHERE PeopleId = @pid
		DELETE FROM dbo.EnrollmentTransaction WHERE PeopleId = @pid
		
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
		DELETE FROM dbo.UserCanEmailFor WHERE CanEmailFor IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		UPDATE dbo.VolunteerForm SET UploaderId = NULL WHERE UploaderId IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		DELETE FROM dbo.Users WHERE PeopleId = @pid
		
		DELETE FROM dbo.TagOrg WHERE Id IN (SELECT Id FROM dbo.Tag WHERE PeopleId = @pid)
		DELETE FROM dbo.TagPerson WHERE id IN (SELECT Id FROM dbo.Tag WHERE PeopleId = @pid)
		DELETE FROM dbo.TagShare WHERE TagId IN (SELECT Id FROM dbo.Tag WHERE PeopleId = @pid)
		DELETE FROM dbo.TagShare WHERE PeopleId = @pid
		DELETE FROM dbo.TagTag WHERE Id IN (SELECT Id FROM dbo.Tag WHERE PeopleId = @pid)
		DELETE FROM dbo.TagTag WHERE ParentTagId IN (SELECT Id FROM dbo.Tag WHERE PeopleId = @pid)
		DELETE FROM dbo.Tag WHERE PeopleId = @pid
		
		SELECT @fid = FamilyId, @pic = PictureId FROM dbo.People WHERE PeopleId = @pid
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
