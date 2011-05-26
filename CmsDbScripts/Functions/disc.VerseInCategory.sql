SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION disc.VerseInCategory
	(
	@vid int,
	@catid int
	)
RETURNS bit
AS
	BEGIN
	RETURN 	Cast(case when EXISTS(
			select * from dbo.VerseCategoryXref 
			where versecategoryid = @catid and verseid = @vid) 
			then 1 else 0 end as bit)
	END
GO
