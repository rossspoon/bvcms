

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

-- =============================================
-- Author:		Kenny
-- Create date: 4/17/2008
-- Description:	Finds the Head of Household given a family ID
-- =============================================
CREATE FUNCTION [dbo].[HeadOfHouseholdId] ( @family_id INT )
RETURNS INT
AS 
    BEGIN
        DECLARE @Result INT

        SELECT TOP 1
                @Result = ISNULL(CASE ( SUM(CASE PositionInFamilyId
                                              WHEN 10 THEN 1
                                              ELSE 0
                                            END) )
                                   WHEN 2 -- couple
                                   -- use primary male
                                        THEN ISNULL(MIN(CASE PositionInFamilyId
                                                          WHEN 10
                                                          THEN CASE GenderId
                                                              WHEN 1
                                                              THEN PeopleId
                                                              ELSE NULL
                                                              END
                                                          ELSE NULL
                                                        END),
                                                   -- primary female
                                                    MIN(CASE PositionInFamilyId
                                                          WHEN 10
                                                          THEN CASE GenderId
                                                              WHEN 2
                                                              THEN PeopleId
                                                              ELSE NULL
                                                              END
                                                          ELSE NULL
                                                        END))
                                   WHEN 1 -- single
                                   -- primary male
                                        THEN ISNULL(MIN(CASE PositionInFamilyId
                                                          WHEN 10
                                                          THEN CASE GenderId
                                                              WHEN 1
                                                              THEN PeopleId
                                                              ELSE NULL
                                                              END
                                                          ELSE NULL
                                                        END),
                                                   -- primary female
                                                    ISNULL(MIN(CASE PositionInFamilyId
                                                              WHEN 10
                                                              THEN CASE GenderId
                                                              WHEN 2
                                                              THEN PeopleId
                                                              ELSE NULL
                                                              END
                                                              ELSE NULL
                                                              END),
                                                          -- primary unknown
                                                           MIN(CASE PositionInFamilyId
                                                              WHEN 10
                                                              THEN CASE GenderId
                                                              WHEN 0
                                                              THEN PeopleId
                                                              ELSE NULL
                                                              END
                                                              ELSE NULL
                                                              END)))
                                      -- eldest
                                 END, dbo.GetEldestFamilyMember(@family_id))
        FROM    dbo.People
        WHERE   FamilyId = @family_id
                AND DeceasedDate IS NULL
                AND FirstName <> 'Duplicate'

	-- Return the result of the function
        RETURN @Result

    END



GO
