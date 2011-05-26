SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[FindPerson3](@first VARCHAR(25), @last VARCHAR(50), @dob DATETIME, @email VARCHAR(60), 
			@phone1 VARCHAR(15),
			@phone2 VARCHAR(15),
			@phone3 VARCHAR(15))
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
		OR f.HomePhone = @phone1
		OR CellPhone = @phone1
		OR WorkPhone = @phone1
		OR f.HomePhone = @phone2		
		OR CellPhone = @phone2
		OR WorkPhone = @phone2
		OR f.HomePhone = @phone3				
		OR CellPhone = @phone3
		OR WorkPhone = @phone3
		OR (BirthDay = @d AND BirthMonth = @m AND BirthYear = @y)
		--OR (NULLIF(f.HomePhone, '') IS NULL 
		--	AND NULLIF(CellPhone, '') IS NULL 
		--	AND NULLIF(WorkPhone, '') IS NULL 
		--	AND NULLIF(EmailAddress, '') IS NULL
		--	AND NULLIF(@email, '') IS NULL
		--	AND NULLIF(@phone1, '') IS NULL
		--	AND NULLIF(@phone2, '') IS NULL
		--	AND NULLIF(@phone3, '') IS NULL
		--	)
	)
	AND
	(
		@dob IS NULL OR BirthDay IS NULL
		OR (BirthDay = @d AND BirthMonth = @m AND ABS(BirthYear - @y) <= 1)
	)
		
    RETURN
END
GO
