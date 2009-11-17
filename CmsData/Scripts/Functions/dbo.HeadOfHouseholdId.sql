SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

-- =============================================
-- Author:		Kenny
-- Create date: 4/17/2008
-- Description:	Finds the Head of Household given a family ID
-- =============================================
CREATE FUNCTION [dbo].[HeadOfHouseholdId] 
(
	@family_id int
)
RETURNS int
AS
BEGIN
	DECLARE @Result int

    SELECT top 1 @Result = 
        isnull(case (sum(case PositionInFamilyId when 10 then 1 else 0 end))
		          when 2 then isnull( min(case PositionInFamilyId when 10 then
									        case GenderId when 1 then PeopleId else null end
								          else
									        null
								          end),
							          min(case PositionInFamilyId when 10 then
                                            case GenderId when 2 then PeopleId else null end
								          else
                                            null
                                          end))
		          when 1 then isnull( min(case PositionInFamilyId when 10 then
									        case GenderId when 1 then PeopleId else null end
								          else
									        null
								          end),
							          isnull(min(case PositionInFamilyId when 10 then
											        case GenderId when 2 then PeopleId else null end
										         else
											        null
										         end),
								             min(case PositionInFamilyId when 10 then
											        case GenderId when 0 then PeopleId else null end
										         else
											        null
										         end)))
		        end, dbo.GetEldestFamilyMember(@family_id))
      FROM dbo.People
     WHERE FamilyId = @family_id
       AND DeceasedDate IS NULL
       AND FirstName <> 'Duplicate'

	-- Return the result of the function
	RETURN @Result

END



GO
