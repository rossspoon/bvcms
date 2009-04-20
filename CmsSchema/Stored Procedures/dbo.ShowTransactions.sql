
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[ShowTransactions](@pid INT, @orgid INT)
AS
BEGIN
	SELECT
		TransactionId, 
		TransactionDate, 
		TransactionTypeId, 
		OrganizationId, 
		PeopleId, 
		NextTranChangeDate,
		ClothingSizeInfo,
		dbo.NextTranChangeDate(PeopleId, OrganizationId, TransactionId, TransactionTypeId) NextTranChangeDate0,
		EnrollmentTransactionId,
		dbo.EnrollmentTransactionId(PeopleId, OrganizationId, TransactionId, TransactionTypeId) EnrollmentTransactionId0,
		CreatedDate
	FROM dbo.EnrollmentTransaction
	WHERE PeopleId = @pid AND (OrganizationId = @orgid OR @orgid = 0)
	ORDER BY TransactionId

END



GO
