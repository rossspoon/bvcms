SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE dbo.ForumNewEntry 
(
	@forumid int, 
	@replytoid int, 
	@title nvarchar(50), 
	@entry text, 
	@created datetime, 
	@createdby nvarchar(50)
)
AS

	declare @DisplayOrder int, @DisplayDepth int, @ThreadId int, @newid int
	
	set @DisplayOrder = 1
	set @DisplayDepth = 0
	
BEGIN TRANSACTION
BEGIN TRY
	if @replytoid is not null
	BEGIN
		SELECT @DisplayOrder=DisplayOrder, @DisplayDepth=DisplayDepth, @ThreadId = ThreadId, @forumid = ForumId
		FROM dbo.ForumEntry 
		WHERE Id=@replytoid
		
		
		-- Find the next entry at the same level or closer to root
		SELECT @DisplayOrder = Min(DisplayOrder)
		FROM dbo.ForumEntry
		WHERE DisplayOrder > @DisplayOrder -- after this entry
		AND DisplayDepth <= @DisplayDepth -- beyond the children
		AND ThreadID = @ThreadID
		
	    IF @DisplayOrder IS NOT NULL
		BEGIN
			-- Move the existing Entries down
			UPDATE dbo.ForumEntry
			SET DisplayOrder = DisplayOrder + 1
			WHERE ThreadID = @ThreadID
			AND DisplayOrder >= @DisplayOrder
		END
		ELSE -- There are no BlogComments at this level or above
		BEGIN
    		-- Find the highest sort order for this parent
    		SELECT @DisplayOrder = MAX(DisplayOrder) + 1
    		FROM dbo.ForumEntry
    		WHERE ThreadID = @ThreadID
		END 
	END
	
	INSERT into dbo.ForumEntry (Title, Entry, ReplyToId, ForumId, ThreadId, DisplayOrder, DisplayDepth, CreatedOn, CreatedBy)
	values (@title, @entry, @replytoid, @forumid, @ThreadId, @DisplayOrder, @DisplayDepth+1, @created, @createdby)
	
	set @newid = scope_identity()
	
	if @ThreadId is null
	begin
		update dbo.ForumEntry set ThreadId = @newid where id = @newid
	end
	
END TRY
BEGIN CATCH
	ROLLBACK

	DECLARE @ErrorMessage NVARCHAR(400);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();
    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	RETURN
END CATCH

COMMIT

	Select * from dbo.ForumEntry where Id = @newid
GO
EXEC sp_addextendedproperty N'ReturnType', N'ForumEntry', 'SCHEMA', N'dbo', 'PROCEDURE', N'ForumNewEntry', NULL, NULL
GO
