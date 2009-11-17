SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[PurgeOrganization](@oid INT)
AS
BEGIN
	BEGIN TRY 
		BEGIN TRANSACTION 
		DECLARE @fid INT, @pic INT
		DELETE FROM dbo.OrganizationMembers WHERE OrganizationId = @oid
		DELETE FROM dbo.BadET WHERE OrgId = @oid
		DELETE FROM dbo.EnrollmentTransaction WHERE OrganizationId = @oid
		DELETE FROM dbo.Attend WHERE OrganizationId = @oid
		DELETE FROM dbo.DivOrg WHERE OrgId = @oid
		DELETE FROM dbo.Meetings WHERE OrganizationId = @oid
		DELETE FROM dbo.Organizations WHERE OrganizationId = @oid
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
