SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE FUNCTION [dbo].[EnrollmentTransactionId2]
(
  @pid int
 ,@oid int
 ,@tid int
 ,@ttid int
)
RETURNS int
AS
	BEGIN
	  DECLARE @TransactionId int
	  SELECT @TransactionId = NULL
	  if @ttid >= 3
		  select top 1 @TransactionId = et.TransactionId
			from  dbo.EnrollmentTransaction et
		   where et.TransactionTypeId <= 2
			 and et.PeopleId = @pid
			 and et.OrganizationId = @oid
			 and et.TransactionId < @tid
			 AND et.TransactionStatus = 0
	   order by et.TransactionId desc
	RETURN @TransactionId
	END
GO
