

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

-- =============================================
-- Author:		Kenny
-- Create date: 4/17/2008
-- Description:	Finds the Head of Household Spouse ID given a family ID
-- =============================================
CREATE FUNCTION [dbo].[HeadOfHouseHoldSpouseId] 
(
	@family_id int
)

RETURNS int
AS
BEGIN
	DECLARE @Result int

    SELECT top 1 @Result = 
           dbo.SpouseId(PeopleId)
      FROM dbo.People
     WHERE FamilyId = @family_id

	-- Return the result of the function
	RETURN @Result

END




GO
