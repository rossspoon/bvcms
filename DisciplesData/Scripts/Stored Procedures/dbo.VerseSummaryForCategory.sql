SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[VerseSummaryForCategory] 
	@cat INT,
	@startRow INT,
	@NumRows INT
AS

BEGIN

select count(*) from VerseSummary;

	With Verses as (
		SELECT ROW_NUMBER() OVER (ORDER BY book, chapter, versenum) as Row, 
			id, Reference, ShortText, CategoryCount, Book, Chapter, VerseNum,
			Cast(case when EXISTS(
			select * from dbo.VerseCategoryXref 
			where versecategoryid = @cat and verseid = vs.id) 
			then 1 else 0 end as bit) as InCategory
		FROM VerseSummary vs
	)
	
	Select id, Reference, ShortText, CategoryCount, Book, Chapter, VerseNum, InCategory
	FROM Verses
	WHERE Row between 
	@startRow and @startRow+@NumRows-1
	
END
GO
