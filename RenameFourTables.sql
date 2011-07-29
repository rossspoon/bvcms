/*
This script needs to be run on the database schema prior to the 7/17/11 ChangeSet
it does the following renames:
	EXEC sp_rename N'[lookup].[DiscoveryClassStatus]',N'NewMemberClassStatus',N'OBJECT'
	EXEC sp_rename N'[lookup].[NewContactReason]',N'ContactReason',N'OBJECT'
	EXEC sp_rename N'[lookup].[NewContactType]',N'ContactType',N'OBJECT'
	EXEC sp_rename N'[dbo].[NewContact]',N'Contact',N'OBJECT'
*/
/*
Script created by SQL Prompt version 5.1.6.35 from Red Gate Software Ltd at 7/16/2011 9:50:09 PM
Run this script on CMS_Bellevue to perform the Smart Rename refactoring.

Please back up your database before running this script.
*/
-- Summary for the smart rename:
--
-- Action:
-- Drop foreign key [FK_People_DiscoveryClassStatus] from table [dbo].[People]
-- Refresh view [dbo].[VerseSummary]
-- Rename table [lookup].[NewMemberClassStatus]
-- Refresh view [dbo].[BadETView]
-- Refresh view [dbo].[City]
-- Refresh view [dbo].[FirstName]
-- Refresh view [dbo].[FirstName2]
-- Refresh view [dbo].[FirstNick]
-- Refresh view [dbo].[LastName]
-- Refresh view [dbo].[Nick]
-- Refresh view [dbo].[BlogCategoriesView]
-- Refresh view [dbo].[ContributionsView]
-- Refresh view [dbo].[Churches]
-- Refresh view [dbo].[PodcastSummary]
-- Refresh view [dbo].[VBSInfo]
-- Refresh view [dbo].[VerseCategoriesView]
-- Refresh view [dbo].[UserList]
-- Add foreign key to [dbo].[People]
--
-- No warnings
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping foreign keys from [dbo].[People]'
GO
ALTER TABLE [dbo].[People] DROP
CONSTRAINT [FK_People_DiscoveryClassStatus]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[VerseSummary]'
GO
EXEC sp_refreshview N'[dbo].[VerseSummary]'
EXEC sp_rename N'[lookup].[DiscoveryClassStatus]',N'NewMemberClassStatus',N'OBJECT'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[BadETView]'
GO
EXEC sp_refreshview N'[dbo].[BadETView]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[City]'
GO
EXEC sp_refreshview N'[dbo].[City]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[FirstName]'
GO
EXEC sp_refreshview N'[dbo].[FirstName]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[FirstName2]'
GO
EXEC sp_refreshview N'[dbo].[FirstName2]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[FirstNick]'
GO
EXEC sp_refreshview N'[dbo].[FirstNick]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[LastName]'
GO
EXEC sp_refreshview N'[dbo].[LastName]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[Nick]'
GO
EXEC sp_refreshview N'[dbo].[Nick]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[BlogCategoriesView]'
GO
EXEC sp_refreshview N'[dbo].[BlogCategoriesView]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[ContributionsView]'
GO
EXEC sp_refreshview N'[dbo].[ContributionsView]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[Churches]'
GO
EXEC sp_refreshview N'[dbo].[Churches]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[PodcastSummary]'
GO
EXEC sp_refreshview N'[dbo].[PodcastSummary]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[VBSInfo]'
GO
EXEC sp_refreshview N'[dbo].[VBSInfo]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[VerseCategoriesView]'
GO
EXEC sp_refreshview N'[dbo].[VerseCategoriesView]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Refreshing [dbo].[UserList]'
GO
EXEC sp_refreshview N'[dbo].[UserList]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[People]'
GO
ALTER TABLE [dbo].[People] ADD
CONSTRAINT [FK_People_DiscoveryClassStatus] FOREIGN KEY ([DiscoveryClassStatusId]) REFERENCES [lookup].[NewMemberClassStatus] ([Id])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO

/*
Script created by SQL Prompt version 5.1.6.35 from Red Gate Software Ltd at 7/16/2011 9:52:50 PM
Run this script on CMS_Bellevue to perform the Smart Rename refactoring.

Please back up your database before running this script.
*/
-- Summary for the smart rename:
--
-- Action:
-- Drop foreign key [FK_NewContacts_ContactReasons] from table [dbo].[NewContact]
-- Rename table [lookup].[ContactReason]
-- Add foreign key to [dbo].[NewContact]
--
-- No warnings
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping foreign keys from [dbo].[NewContact]'
GO
ALTER TABLE [dbo].[NewContact] DROP
CONSTRAINT [FK_NewContacts_ContactReasons]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[lookup].[NewContactReason]',N'ContactReason',N'OBJECT'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[NewContact]'
GO
ALTER TABLE [dbo].[NewContact] ADD
CONSTRAINT [FK_NewContacts_ContactReasons] FOREIGN KEY ([ContactReasonId]) REFERENCES [lookup].[ContactReason] ([Id])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO
/*
Script created by SQL Prompt version 5.1.6.35 from Red Gate Software Ltd at 7/16/2011 9:53:09 PM
Run this script on CMS_Bellevue to perform the Smart Rename refactoring.

Please back up your database before running this script.
*/
-- Summary for the smart rename:
--
-- Action:
-- Drop foreign key [FK_Contacts_ContactTypes] from table [dbo].[NewContact]
-- Rename table [lookup].[ContactType]
-- Add foreign key to [dbo].[NewContact]
--
-- No warnings
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping foreign keys from [dbo].[NewContact]'
GO
ALTER TABLE [dbo].[NewContact] DROP
CONSTRAINT [FK_Contacts_ContactTypes]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[lookup].[NewContactType]',N'ContactType',N'OBJECT'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[NewContact]'
GO
ALTER TABLE [dbo].[NewContact] ADD
CONSTRAINT [FK_Contacts_ContactTypes] FOREIGN KEY ([ContactTypeId]) REFERENCES [lookup].[ContactType] ([Id])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
/*
Script created by SQL Prompt version 5.1.6.35 from Red Gate Software Ltd at 7/16/2011 9:53:44 PM
Run this script on CMS_Bellevue to perform the Smart Rename refactoring.

Please back up your database before running this script.
*/
-- Summary for the smart rename:
--
-- Action:
-- Drop foreign key [contactees__contact] from table [dbo].[Contactees]
-- Drop foreign key [contactsMakers__contact] from table [dbo].[Contactors]
-- Drop foreign key [TasksCompleted__CompletedContact] from table [dbo].[Task]
-- Drop foreign key [TasksAssigned__SourceContact] from table [dbo].[Task]
-- Drop foreign key [FK_NewContacts_ContactReasons] from table [dbo].[NewContact]
-- Drop foreign key [FK_Contacts_ContactTypes] from table [dbo].[NewContact]
-- Drop foreign key [FK_Contacts_Ministries] from table [dbo].[NewContact]
-- Rename table [dbo].[Contact]
-- Alter function [dbo].[LastContact]
-- Alter function [dbo].[DaysSinceContact]
-- Alter procedure [dbo].[PurgeAllButOrg]
-- Add foreign key to [dbo].[Contactees]
-- Add foreign key to [dbo].[Contactors]
-- Add foreign key to [dbo].[Task]
-- Add foreign key to [dbo].[Task]
-- Add foreign key to [dbo].[Contact]
-- Add foreign key to [dbo].[Contact]
-- Add foreign key to [dbo].[Contact]
--
-- No warnings
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping foreign keys from [dbo].[Contactees]'
GO
ALTER TABLE [dbo].[Contactees] DROP
CONSTRAINT [contactees__contact]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Contactors]'
GO
ALTER TABLE [dbo].[Contactors] DROP
CONSTRAINT [contactsMakers__contact]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Task]'
GO
ALTER TABLE [dbo].[Task] DROP
CONSTRAINT [TasksCompleted__CompletedContact],
CONSTRAINT [TasksAssigned__SourceContact]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[NewContact]'
GO
ALTER TABLE [dbo].[NewContact] DROP
CONSTRAINT [FK_NewContacts_ContactReasons],
CONSTRAINT [FK_Contacts_ContactTypes],
CONSTRAINT [FK_Contacts_Ministries]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[NewContact]',N'Contact',N'OBJECT'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[LastContact]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
ALTER FUNCTION dbo.LastContact(@pid INT)
RETURNS DATETIME
AS
BEGIN
	DECLARE @dt DATETIME

	SELECT @dt = MAX(c.ContactDate) FROM dbo.Contact c
	JOIN dbo.Contactees ce ON c.ContactId = ce.ContactId
	WHERE ce.PeopleId = @pid
	IF @dt IS NULL
		SELECT @dt = DATEADD(DAY,-5000,GETDATE())

	RETURN @dt

END



GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[DaysSinceContact]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
ALTER FUNCTION dbo.DaysSinceContact(@pid INT)
RETURNS int
AS
BEGIN
	DECLARE @days int

	SELECT @days = MIN(DATEDIFF(D,c.ContactDate,GETDATE())) FROM dbo.Contact c
	JOIN dbo.Contactees ce ON c.ContactId = ce.ContactId
	WHERE ce.PeopleId = @pid

	RETURN @days

END



GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[PurgeAllButOrg]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE dbo.PurgeAllButOrg(@orgid INT)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

DECLARE @people TABLE ( id INT )

INSERT INTO @people
SELECT PeopleId
FROM dbo.People p
WHERE NOT EXISTS(
	SELECT NULL FROM dbo.OrganizationMembers om
	JOIN dbo.People pp ON om.PeopleId = pp.PeopleId
	WHERE OrganizationId = @orgid AND pp.FamilyId = p.FamilyId
	)
	AND NOT EXISTS(
	SELECT NULL FROM dbo.EnrollmentTransaction et
	JOIN dbo.People pp ON et.PeopleId = pp.PeopleId
	WHERE OrganizationId = @orgid AND pp.FamilyId = p.FamilyId
	)
	
DELETE dbo.SoulMate
	
DELETE dbo.LoveRespect

DELETE dbo.OrgMemMemTags
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.OrganizationMembers
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.BadET
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.EnrollmentTransaction
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.MOBSReg
WHERE PeopleId IN (SELECT id FROM @people)
	
DELETE dbo.Attend
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.BundleDetail
FROM dbo.BundleDetail bd
JOIN dbo.Contribution c ON bd.ContributionId = c.ContributionId
WHERE PeopleId IN (SELECT id FROM @people)
	
DELETE dbo.Contribution
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.VolunteerForm
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.Volunteer
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.Contactees
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.Contactors
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.TagPerson
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.Task
WHERE WhoId IN (SELECT id FROM @people)

DELETE dbo.Task
WHERE OwnerId IN (SELECT id FROM @people)

DELETE dbo.Task
WHERE CoOwnerId IN (SELECT id FROM @people)

DELETE dbo.TaskListOwners
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.Preferences
WHERE UserId IN (SELECT UserId FROM Users
WHERE PeopleId IN (SELECT id FROM @people))

DELETE dbo.ActivityLog
WHERE UserId IN (SELECT UserId FROM Users
WHERE PeopleId IN (SELECT id FROM @people))

DELETE dbo.UserRole
WHERE UserId IN (SELECT UserId FROM Users
WHERE PeopleId IN (SELECT id FROM @people))

DELETE dbo.UserCanEmailFor
WHERE UserId IN (SELECT UserId FROM Users
WHERE PeopleId IN (SELECT id FROM @people))

DELETE dbo.UserCanEmailFor
WHERE CanEmailFor IN (SELECT UserId FROM Users
WHERE PeopleId IN (SELECT id FROM @people))

DELETE dbo.VolunteerForm
WHERE UploaderId IN (SELECT UserId FROM Users
WHERE PeopleId IN (SELECT id FROM @people))

DELETE dbo.Users
WHERE UserId IN (SELECT UserId FROM Users
WHERE PeopleId IN (SELECT id FROM @people))
	
DELETE dbo.TagPerson
DELETE dbo.TagShare
DELETE dbo.Tag

DELETE dbo.RecReg
WHERE PeopleId IN (SELECT id FROM @people)
	
DELETE dbo.VBSApp
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.VolInterestInterestCodes

DELETE dbo.VolInterestCodes

UPDATE dbo.Families
SET HeadOfHouseholdId = NULL, HeadOfHouseholdSpouseId = NULL

UPDATE dbo.People
SET PictureId = NULL 
WHERE PeopleId IN (SELECT id FROM @people)

DELETE dbo.Picture
FROM dbo.Picture pic
WHERE NOT EXISTS(SELECT NULL FROM dbo.People WHERE PictureId = pic.PictureId)

---------------------------------------
DELETE dbo.People
WHERE PeopleId IN (SELECT id FROM @people)
---------------------------------------

DELETE dbo.RelatedFamilies
FROM dbo.RelatedFamilies r
JOIN dbo.Families f ON r.FamilyId = f.FamilyId
WHERE (SELECT COUNT(*) FROM dbo.People WHERE FamilyId = f.FamilyId) = 0

DELETE dbo.RelatedFamilies
FROM dbo.RelatedFamilies r
JOIN dbo.Families f ON r.RelatedFamilyId = f.FamilyId
WHERE (SELECT COUNT(*) FROM dbo.People WHERE FamilyId = f.FamilyId) = 0

DELETE dbo.Families 
FROM dbo.Families f
WHERE (SELECT COUNT(*) FROM dbo.People WHERE FamilyId = f.FamilyId) = 0
	
----------------------------------------------------------------------------
DELETE dbo.Contact
FROM dbo.Contact nc
WHERE NOT EXISTS(SELECT NULL FROM dbo.Contactees WHERE ContactId = nc.ContactId)
AND NOT EXISTS(SELECT NULL FROM dbo.Contactors WHERE ContactId = nc.ContactId)

DELETE dbo.Meetings
FROM dbo.Meetings m
WHERE NOT EXISTS(SELECT NULL FROM dbo.Attend WHERE MeetingId = m.MeetingId)

DELETE dbo.DivOrg
FROM dbo.DivOrg od
JOIN dbo.Organizations o ON od.OrgId = o.OrganizationId
WHERE NOT EXISTS(SELECT NULL FROM dbo.OrganizationMembers WHERE OrganizationId = o.OrganizationId)
AND NOT EXISTS(SELECT NULL FROM dbo.EnrollmentTransaction WHERE OrganizationId = o.OrganizationId)

DELETE dbo.MemberTags
FROM dbo.MemberTags mt
JOIN dbo.Organizations o ON mt.OrgId = o.OrganizationId
WHERE NOT EXISTS(SELECT NULL FROM dbo.OrganizationMembers WHERE OrganizationId = o.OrganizationId)

DELETE dbo.Organizations
FROM dbo.Organizations o
WHERE NOT EXISTS(SELECT NULL FROM dbo.OrganizationMembers WHERE OrganizationId = o.OrganizationId)
AND NOT EXISTS(SELECT NULL FROM dbo.EnrollmentTransaction WHERE OrganizationId = o.OrganizationId)
AND NOT EXISTS(SELECT NULL FROM dbo.Attend WHERE OrganizationId = o.OrganizationId)

DELETE dbo.Promotion

DELETE dbo.Division
FROM dbo.Division d
WHERE NOT EXISTS(SELECT NULL FROM dbo.DivOrg WHERE DivId = d.Id)
AND NOT EXISTS(SELECT NULL FROM dbo.Organizations WHERE DivisionId = d.Id)
AND NOT EXISTS(SELECT NULL FROM dbo.RecLeague WHERE DivId = d.Id)

DELETE dbo.Program
FROM dbo.Program p
WHERE NOT EXISTS(SELECT NULL FROM dbo.Division WHERE ProgId = p.Id)

DELETE dbo.TaskList
FROM dbo.TaskList t
WHERE NOT EXISTS(SELECT NULL FROM  dbo.Users WHERE UserId = t.CreatedBy)

DELETE Tag
FROM dbo.Tag t
WHERE PeopleId IS NULL
AND NOT EXISTS(SELECT NULL FROM dbo.TagPerson WHERE Id = t.Id)

DELETE dbo.BundleDetail
FROM dbo.BundleDetail bd
WHERE EXISTS(SELECT NULL FROM dbo.Contribution WHERE ContributionId = bd.ContributionId AND PeopleId IS NULL)

DELETE FROM dbo.Contribution WHERE PeopleId IS NULL

DELETE dbo.BundleHeader
FROM dbo.BundleHeader h
WHERE NOT EXISTS(SELECT NULL FROM dbo.BundleDetail WHERE BundleHeaderId = h.BundleHeaderId)

DELETE FROM dbo.AuditValues
DELETE FROM dbo.Audits

EXEC dbo.DeleteAllQueriesWithNoChildren
EXEC dbo.DeleteAllQueriesWithNoChildren
EXEC dbo.DeleteAllQueriesWithNoChildren
EXEC dbo.DeleteAllQueriesWithNoChildren

END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Contactees]'
GO
ALTER TABLE [dbo].[Contactees] ADD
CONSTRAINT [contactees__contact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact] ([ContactId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Contactors]'
GO
ALTER TABLE [dbo].[Contactors] ADD
CONSTRAINT [contactsMakers__contact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact] ([ContactId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Task]'
GO
ALTER TABLE [dbo].[Task] ADD
CONSTRAINT [TasksCompleted__CompletedContact] FOREIGN KEY ([CompletedContactId]) REFERENCES [dbo].[Contact] ([ContactId]),
CONSTRAINT [TasksAssigned__SourceContact] FOREIGN KEY ([SourceContactId]) REFERENCES [dbo].[Contact] ([ContactId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Contact]'
GO
ALTER TABLE [dbo].[Contact] ADD
CONSTRAINT [FK_NewContacts_ContactReasons] FOREIGN KEY ([ContactReasonId]) REFERENCES [lookup].[ContactReason] ([Id]),
CONSTRAINT [FK_Contacts_ContactTypes] FOREIGN KEY ([ContactTypeId]) REFERENCES [lookup].[ContactType] ([Id]),
CONSTRAINT [FK_Contacts_Ministries] FOREIGN KEY ([MinistryId]) REFERENCES [dbo].[Ministries] ([MinistryId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO
GO