SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[FindPerson](@first VARCHAR(25), @last VARCHAR(50), @dob DATETIME, @email VARCHAR(60), @phone VARCHAR(15))
RETURNS @t TABLE ( PeopleId INT)
AS
BEGIN
	DECLARE @fname VARCHAR(50) = REPLACE(@first,' ', '')
	DECLARE @m INT = DATEPART(m, @dob)
	DECLARE @d INT = DATEPART(d, @dob)
	DECLARE @y INT = DATEPART(yy, @dob)
	
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
		@dob IS NULL OR BirthDay IS NULL
		OR (BirthDay = @d AND BirthMonth = @m AND ABS(BirthYear - @y) <= 1)
	)
		
    RETURN
END
GO
