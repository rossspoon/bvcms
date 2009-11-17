SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		Kenny
-- Create date: 4/17/2008
-- Description:	Finds the eldest member given the family ID
-- =============================================
CREATE FUNCTION [dbo].[GetEldestFamilyMember]( @fid int )
RETURNS int
AS
BEGIN
    DECLARE @Result int

    select @Result = PeopleId
      from dbo.People
     where FamilyId = @fid
       and dbo.Age(PeopleId) = (select max(dbo.Age(PeopleId))
                    from dbo.People
                   where FamilyId = @fid)
                   
    IF @Result IS NULL
		SELECT TOP 1 @Result = PeopleId FROM dbo.People WHERE FamilyId = @fid
     
	RETURN @Result
END
GO
