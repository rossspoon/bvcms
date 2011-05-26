SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION disc.VersePos
	(
	@id int
	)
RETURNS int
AS
	BEGIN
		return (select Book * 100000 + Chapter * 100 + VerseNum 
		from Verse
		where id = @id)
	END
GO
