SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[GetToken](@guid uniqueidentifier) 
AS
	declare @expired bit
	set @expired = (select expired from dbo.TemporaryToken where @guid = id)
	update dbo.TemporaryToken set expired = 1 where id = @guid
	select CreatedOn, @expired as expired, CreatedBy from dbo.TemporaryToken where id = @guid
	RETURN
GO
