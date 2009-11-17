CREATE TABLE [dbo].[EnrollmentTransaction]
(
[TransactionId] [int] NOT NULL IDENTITY(1, 1),
[TransactionStatus] [bit] NOT NULL CONSTRAINT [DF_ENROLLMENT_TRANSACTION_TBL_TRANSACTION_STATUS] DEFAULT ((0)),
[CreatedBy] [int] NULL,
[CreatedDate] [datetime] NULL,
[TransactionDate] [datetime] NOT NULL,
[TransactionTypeId] [int] NOT NULL,
[OrganizationId] [int] NOT NULL,
[OrganizationName] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PeopleId] [int] NOT NULL,
[MemberTypeId] [int] NOT NULL,
[EnrollmentDate] [datetime] NULL,
[AttendancePercentage] [real] NULL,
[NextTranChangeDate] [datetime] NULL,
[EnrollmentTransactionId] [int] NULL,
[Pending] [bit] NULL
) ON [PRIMARY]
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[insEnrollmentTransaction] 
   ON  [dbo].[EnrollmentTransaction] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @tid INT, @trandt DATETIME, @typeid INT, @orgid INT, @pid INT

	DECLARE c CURSOR FORWARD_ONLY FOR
	SELECT TransactionId, TransactionDate, TransactionTypeId, OrganizationId, PeopleId 
	FROM inserted 
	WHERE TransactionTypeId > 2

	OPEN c
	FETCH NEXT FROM c INTO @tid, @trandt, @typeid, @orgid, @pid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC dbo.LinkEnrollmentTransaction @tid, @trandt, @typeid, @orgid, @pid
		FETCH NEXT FROM c INTO @tid, @trandt, @typeid, @orgid, @pid
	END
	CLOSE c
	DEALLOCATE c
END
GO
ALTER TABLE [dbo].[EnrollmentTransaction] ADD CONSTRAINT [ENROLLMENT_TRANSACTION_PK] PRIMARY KEY NONCLUSTERED  ([TransactionId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ENROLLMENT_TRANSACTION_ORG_IX] ON [dbo].[EnrollmentTransaction] ([OrganizationId], [PeopleId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ENROLLMENT_TRANS_ORG_TC_TS_IX] ON [dbo].[EnrollmentTransaction] ([OrganizationId], [TransactionTypeId], [TransactionStatus]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ENROLLMENT_TRANSACTION_PPL_IX] ON [dbo].[EnrollmentTransaction] ([PeopleId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ENROLLMENT_TRANSACTION_TBL] ON [dbo].[EnrollmentTransaction] ([TransactionDate]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>] ON [dbo].[EnrollmentTransaction] ([TransactionDate], [TransactionTypeId]) INCLUDE ([OrganizationId], [PeopleId], [TransactionId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EnrollmentTransaction] WITH NOCHECK ADD CONSTRAINT [ENROLLMENT_TRANSACTION_ORG_FK] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[EnrollmentTransaction] WITH NOCHECK ADD CONSTRAINT [ENROLLMENT_TRANSACTION_PPL_FK] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[EnrollmentTransaction] WITH NOCHECK ADD CONSTRAINT [FK_ENROLLMENT_TRANSACTION_TBL_MemberType] FOREIGN KEY ([MemberTypeId]) REFERENCES [lookup].[MemberType] ([Id])
GO
