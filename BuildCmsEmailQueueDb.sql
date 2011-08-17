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
PRINT N'Creating schemata'
GO
PRINT N'Creating [dbo].[GetInitiatorHandle]'
GO
CREATE FUNCTION [dbo].[GetInitiatorHandle] ( @conversationID AS UNIQUEIDENTIFIER )
RETURNS UNIQUEIDENTIFIER
AS
	BEGIN
		DECLARE @conversationHandle AS UNIQUEIDENTIFIER
		
		SELECT @conversationHandle = conversation_handle
		FROM sys.conversation_endpoints
		WHERE conversation_id = @conversationID
		AND is_initiator=1
		
		RETURN @conversationHandle
	END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[SendMessage]'
GO
CREATE PROCEDURE [dbo].[SendMessage](@conversation UNIQUEIDENTIFIER, @msg VARCHAR(200))
AS
BEGIN
	DECLARE @h UNIQUEIDENTIFIER
	SELECT @h = dbo.GetInitiatorHandle(@conversation);
	SEND ON CONVERSATION @h 
	MESSAGE TYPE [DEFAULT] (@msg)
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[InsertMessage]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertMessage](@msg VARCHAR(200))
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @dialog UNIQUEIDENTIFIER
	DECLARE @message NVARCHAR(50)
	BEGIN DIALOG CONVERSATION @dialog
	FROM SERVICE EmailSendService
	TO SERVICE 'EmailReceiveService'
	ON CONTRACT EmailContract
	WITH Encryption = OFF;
	
	SEND ON CONVERSATION @dialog
	MESSAGE TYPE EmailRequest (@msg)
	
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[QueueScheduledEmails]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[QueueScheduledEmails]
AS
BEGIN

DECLARE @Id INT
DECLARE @Host VARCHAR(50)
DECLARE @CmsHost VARCHAR(50)
DECLARE @msg VARCHAR(200)

DECLARE c1 CURSOR READ_ONLY
FOR
SELECT Id, Host, CmsHost FROM BlogData.dbo.ScheduledEmailAll

OPEN c1

FETCH NEXT FROM c1 INTO @Id, @Host, @CmsHost

WHILE @@FETCH_STATUS = 0
BEGIN

	
	SET @msg = CONVERT(VARCHAR(10), @id) + '|' + @CmsHost + '|' + @Host
	EXEC dbo.InsertMessage @msg

	FETCH NEXT FROM c1
	INTO @Id, @Host, @CmsHost

END

CLOSE c1
DEALLOCATE c1

END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[GetTargetHandle]'
GO
CREATE FUNCTION [dbo].[GetTargetHandle] ( @conversationID AS UNIQUEIDENTIFIER )
RETURNS UNIQUEIDENTIFIER
AS
	BEGIN
		DECLARE @conversationHandle AS UNIQUEIDENTIFIER
		
		SELECT @conversationHandle = conversation_handle
		FROM sys.conversation_endpoints
		WHERE conversation_id = @conversationID
		AND is_initiator=0
		
		RETURN @conversationHandle
	END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[EndConversation]'
GO
CREATE PROCEDURE [dbo].[EndConversation] ( @conversationID AS UNIQUEIDENTIFIER )
AS
BEGIN
	DECLARE @initiatorHandle AS UNIQUEIDENTIFIER
	DECLARE @targetHandle AS UNIQUEIDENTIFIER
	
	SET @initiatorHandle = dbo.GetInitiatorHandle(@conversationID)
	SET @targetHandle = dbo.GetTargetHandle(@conversationID)
	
	IF @initiatorHandle IS NOT NULL
	BEGIN
		END CONVERSATION @initiatorHandle WITH CLEANUP
		PRINT 'ended Initiator'
	END
	
	IF @targetHandle IS NOT NULL
	BEGIN
		END CONVERSATION @targetHandle WITH CLEANUP
		PRINT 'ended Target'
	END

	RETURN
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[SelectFromQueue]'
GO

CREATE VIEW [dbo].[SelectFromQueue]
	AS
	SELECT CONVERT(VARCHAR(max), message_body) AS message FROM EmailQueue
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[SelectConversations]'
GO
CREATE VIEW [dbo].[SelectConversations]
	AS
SELECT * FROM sys.conversation_endpoints
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[CreateNewPriorityConversation]'
GO
CREATE PROCEDURE [dbo].[CreateNewPriorityConversation] ( @conversationID AS UNIQUEIDENTIFIER OUTPUT )
AS
BEGIN
	DECLARE @dialogHandle AS UNIQUEIDENTIFIER

	--	Create a dialog
	BEGIN DIALOG CONVERSATION @dialogHandle
	FROM SERVICE EmailService
	TO SERVICE 'EmailService'
	ON CONTRACT PriorityEmailContract
	WITH
	ENCRYPTION = OFF
	
	SELECT @conversationId = conversation_id
	FROM sys.conversation_endpoints
	WHERE conversation_handle = @dialogHandle
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[CreateNewConversation]'
GO
CREATE PROCEDURE [dbo].[CreateNewConversation] ( @conversationID AS UNIQUEIDENTIFIER OUTPUT )
AS
BEGIN
	DECLARE @dialogHandle AS UNIQUEIDENTIFIER

	--	Create a dialog
	BEGIN DIALOG CONVERSATION @dialogHandle
	FROM SERVICE [EmailService]
	TO SERVICE 'EmailService'
	ON CONTRACT NormalEmailContract
	WITH
	ENCRYPTION = OFF
	
	SELECT @conversationId = conversation_id
	FROM sys.conversation_endpoints
	WHERE conversation_handle = @dialogHandle

END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[CleanUpAllConversations]'
GO
CREATE PROCEDURE [dbo].[CleanUpAllConversations]
AS
	WHILE EXISTS(SELECT * FROM sys.conversation_endpoints)
	BEGIN
		DECLARE @conversationHandle AS UNIQUEIDENTIFIER

		SELECT TOP(1) @conversationHandle=conversation_handle FROM sys.conversation_endpoints

		END CONVERSATION @conversationHandle WITH CLEANUP
	END
	RETURN
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[ShowConversations]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ShowConversations]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM sys.conversation_endpoints
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating contracts'
GO
CREATE CONTRACT [NormalEmailContract]
AUTHORIZATION [dbo] ( 
[DEFAULT] SENT BY INITIATOR
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
CREATE CONTRACT [PriorityEmailContract]
AUTHORIZATION [dbo] ( 
[DEFAULT] SENT BY INITIATOR
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating queues'
GO
CREATE QUEUE [dbo].[EmailQueue] 
WITH STATUS=ON, 
RETENTION=OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating services'
GO
CREATE SERVICE [EmailService]
AUTHORIZATION [dbo]
ON QUEUE [dbo].[EmailQueue]
(
[NormalEmailContract],
[PriorityEmailContract]
)
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
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO
SET DATEFORMAT YMD
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)
COMMIT TRANSACTION
GO
