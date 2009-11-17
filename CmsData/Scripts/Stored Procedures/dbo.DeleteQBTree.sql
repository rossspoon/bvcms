SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE DeleteQBTree(@qid INT)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

		DECLARE cur CURSOR FOR SELECT QueryId FROM dbo.QBClauses(@qid) ORDER BY LEVEL DESC
		OPEN cur
		DECLARE @id int
		FETCH NEXT FROM cur INTO @id
		WHILE @@FETCH_STATUS = 0
		BEGIN
			DELETE dbo.QueryBuilderClauses
			WHERE QueryId = @id
			FETCH NEXT FROM cur INTO @id
		END
		CLOSE cur
		DEALLOCATE cur

		
END
GO
