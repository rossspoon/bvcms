SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE dbo.DeleteTagForUser(@tag varchar, @user varchar)
AS
	/* SET NOCOUNT ON */
	declare @id int
	select @id = id from tag where name = @tag and @user = owner
	
	delete from tagorg where id = @id
	delete from tagtag where id = @id
	delete from tagperson where id = @id
	delete from tagshared where id = @id
	delete from tag where id = @id
	
	RETURN

GO
