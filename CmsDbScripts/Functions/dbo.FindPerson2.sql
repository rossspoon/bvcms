SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE FUNCTION [dbo].[FindPerson2](@first VARCHAR(25), @last VARCHAR(50), @m INT, @d INT, @y INT, @email VARCHAR(60), @phone VARCHAR(15))
RETURNS @t TABLE ( PeopleId INT)
AS
BEGIN
	DECLARE @fname VARCHAR(50) = REPLACE(@first,' ', '')
	
	INSERT INTO @t SELECT PeopleId FROM dbo.People p
	JOIN dbo.Families f ON p.FamilyId = f.FamilyId
	WHERE
	(
		FirstName2 LIKE (@fname + '%')
		OR @fname LIKE (FirstName + '%')
		OR @fname = NickName
	)
	AND (@last = LastName OR @last = MaidenName OR @last = MiddleName)
	AND
	(
		p.EmailAddress = @email
		OR p.EmailAddress2 = @email
		OR f.HomePhone = @phone
		OR CellPhone = @phone
		OR (BirthDay = @d AND BirthMonth = @m AND BirthYear = @y)
	)
	AND
	(
		@d IS NULL OR BirthDay IS NULL
		OR (BirthDay = @d AND BirthMonth = @m AND (@y IS NULL OR ABS(BirthYear - @y) <= 1))
	)
	RETURN
END
GO
