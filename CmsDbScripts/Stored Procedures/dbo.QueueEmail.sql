SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[QueueEmail]
	(@id INT, @CmsHost varchar(50), @Host varchar(50))
AS
BEGIN

	DECLARE @conversation UNIQUEIDENTIFIER
	EXEC CMSEmailQueue.dbo.CreateNewConversation @conversationID = @conversation OUTPUT;
	DECLARE @msg VARCHAR(200) = 'START|' + @Host + '|' + @CmsHost + '|' + CONVERT(VARCHAR(10), @id)
	EXEC CMSEmailQueue.dbo.SendMessage @conversation, @msg
	DECLARE @pid int
	DECLARE c1 CURSOR READ_ONLY FOR SELECT PeopleId FROM dbo.EmailQueueTo WHERE Id = @id AND Sent IS NULL
	OPEN c1
	FETCH NEXT FROM c1 INTO @pid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @msg = 'SEND|' + @Host + '|' + @CmsHost + '|' + CONVERT(VARCHAR(10), @id) + '|' + CONVERT(VARCHAR(10), @pid)	
		EXEC CMSEmailQueue.dbo.SendMessage @conversation, @msg
		FETCH NEXT FROM c1 INTO @pid
	END
	CLOSE c1
	DEALLOCATE c1
	SET @msg = 'END|' + @Host + '|' + @CmsHost + '|' + CONVERT(VARCHAR(10), @id) + '|' + CONVERT(VARCHAR(50), @conversation)
	EXEC CMSEmailQueue.dbo.SendMessage @conversation, @msg

END

GO
