SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE FUNCTION [dbo].[HeadOfHouseHoldSpouseId] 
(
	@family_id int
)

RETURNS int
AS
BEGIN
	DECLARE @Result int

    SELECT @Result = 
           dbo.SpouseId(dbo.HeadOfHouseholdId(@family_id))

	-- Return the result of the function
	RETURN @Result

END



GO
