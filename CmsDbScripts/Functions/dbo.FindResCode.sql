SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].FindResCode
(
	@zipcode VARCHAR(20)
)
RETURNS int
AS
BEGIN
	DECLARE @Result int
	DECLARE @z5 VARCHAR(10)

	IF (@zipcode IS NOT NULL AND LEN(@zipcode) >= 5)
	BEGIN
		SELECT @z5 = SUBSTRING(@zipcode, 1, 5)
		SELECT @Result = MetroMarginalCode FROM dbo.Zips WHERE ZipCode = @z5
		IF @Result IS NULL
			SELECT @Result = 30
	END
	-- Return the result of the function
	RETURN @Result

END
GO
