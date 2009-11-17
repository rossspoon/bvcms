SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[DeleteTagForUser](@tag varchar, @user varchar)
AS
	/* SET NOCOUNT ON */
	declare @id int
	select @id = id from tag where name = @tag and @user = owner
	
	delete from tagperson where id = @id
	delete from tagshare where tagid = @id
	delete from tag where id = @id
	
	RETURN
GO
