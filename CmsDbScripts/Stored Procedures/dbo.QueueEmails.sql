SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[QueueEmails](@scheduled BIT, @CmsHost varchar(50), @Host varchar(50))
AS
BEGIN

DECLARE @id INT
DECLARE @msg VARCHAR(200)
DECLARE c1 CURSOR READ_ONLY
FOR
SELECT Id FROM dbo.EmailQueue
WHERE Sent IS NULL AND (
	(@scheduled = 0 AND SendWhen IS NULL)
	OR (@scheduled = 1 AND SendWhen IS NOT NULL AND SendWhen < GETDATE())
)

OPEN c1

FETCH NEXT FROM c1 INTO @id

WHILE @@FETCH_STATUS = 0
BEGIN

	
	SET @msg = @id + '|' + @CmsHost + '|' + @Host
	EXEC dbo.QueueEmail @id, @CmsHost, @Host

	FETCH NEXT FROM c1
	INTO @id

END

CLOSE c1
DEALLOCATE c1

END
GO
