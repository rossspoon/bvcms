SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION dbo.NextTranChangeDate
(
  @pid int
 ,@oid int
 ,@tid int
 ,@typeid int
)
RETURNS datetime
AS
	BEGIN
	  DECLARE @dt datetime 
		  select top 1 @dt = TransactionDate
			from dbo.EnrollmentTransaction
		   where TransactionTypeId >= 3
		     and @typeid <= 3
			 and PeopleId = @pid
			 and OrganizationId = @oid
			 and TransactionId > @tid
	   order by TransactionId
	RETURN @dt
	END

GO
