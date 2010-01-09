SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE disc.GetUsersToUploadFor (@uploader varchar(20))
AS
BEGIN

select postsfor from dbo.UploadAuthenticationXref x 
where postinguser = @uploader

END
GO
