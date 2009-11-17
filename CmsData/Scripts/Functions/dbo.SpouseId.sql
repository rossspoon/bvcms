SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		Michael Mayers
-- Create date: 
-- Description:	Determines Spouse people id
-- =============================================
CREATE FUNCTION SpouseId 
(
	-- Add the parameters for the function here
	@peopleid int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int

	-- Add the T-SQL statements to compute the return value here
	SELECT TOP 1 @Result = s.PeopleId 
      FROM ( SELECT p1.FamilyId,
                    p1.MaritalStatusId,
                    p1.PositionInFamilyId
               FROM dbo.People p1
              WHERE p1.PeopleId =  @peopleid
                AND p1.MaritalStatusId = 20
                AND p1.DeceasedDate IS NULL
                AND p1.FirstName <> 'Duplicate'
           ) p
     INNER JOIN dbo.Families f
             ON f.FamilyId = p.FamilyId
     INNER JOIN dbo.People s
             ON s.FamilyId = f.FamilyId
     WHERE s.PeopleId <> @peopleid 
       AND p.MaritalStatusId = s.MaritalStatusId
       AND p.PositionInFamilyId = s.PositionInFamilyId
       AND s.DeceasedDate IS NULL
       AND s.FirstName <> 'Duplicate'

	-- Return the result of the function
	RETURN @Result

END

GO
