SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE UpdateAllAttendStr
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		DECLARE cur CURSOR FOR SELECT OrganizationId, PeopleId FROM dbo.OrganizationMembers
		OPEN cur
		DECLARE @oid INT, @pid INT, @n INT
		SET @n = 0
		FETCH NEXT FROM cur INTO @oid, @pid
		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXECUTE dbo.UpdateAttendStr @oid, @pid
			SET @n = @n + 1
			IF (@n % 1000) = 0
				RAISERROR ('%d', 0, 1, @n) WITH NOWAIT
			FETCH NEXT FROM cur INTO @oid, @pid
		END
		CLOSE cur
		DEALLOCATE cur
END


GO
