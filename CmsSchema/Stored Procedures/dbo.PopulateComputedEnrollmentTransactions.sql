SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE PopulateComputedEnrollmentTransactions
AS
BEGIN
	UPDATE dbo.EnrollmentTransaction
	SET NextTranChangeDate = dbo.NextTranChangeDate(PeopleId, OrganizationId, TransactionId, TransactionTypeId),
		EnrollmentTransactionId = dbo.EnrollmentTransactionId(PeopleId, OrganizationId, TransactionId, TransactionTypeId)
END



GO
