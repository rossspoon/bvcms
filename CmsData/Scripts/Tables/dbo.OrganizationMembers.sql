CREATE TABLE [dbo].[OrganizationMembers]
(
[OrganizationId] [int] NOT NULL,
[PeopleId] [int] NOT NULL,
[CreatedBy] [int] NULL,
[CreatedDate] [datetime] NULL,
[MemberTypeId] [int] NOT NULL,
[EnrollmentDate] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL,
[InactiveDate] [datetime] NULL,
[AttendStr] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AttendPct] [real] NULL,
[LastAttended] [datetime] NULL,
[Pending] [bit] NULL,
[UserData] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
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
CREATE TRIGGER [dbo].[updOrganizationMember] 
   ON  [dbo].[OrganizationMembers]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF (
		UPDATE(MemberTypeId) 
		OR UPDATE(Pending) 
    )
	BEGIN
		DECLARE @pid INT
		DECLARE c CURSOR FOR
		SELECT PeopleId FROM inserted
		OPEN c;
		FETCH NEXT FROM c INTO @pid;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			UPDATE dbo.People SET Grade = dbo.SchoolGrade(PeopleId) WHERE PeopleId = @pid
			FETCH NEXT FROM c INTO @pid;
		END;
		CLOSE c;
		DEALLOCATE c;
	END

END
GO
ALTER TABLE [dbo].[OrganizationMembers] ADD CONSTRAINT [ORGANIZATION_MEMBERS_PK] PRIMARY KEY NONCLUSTERED  ([OrganizationId], [PeopleId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ORGANIZATION_MEMBERS_PPL_FK_IX] ON [dbo].[OrganizationMembers] ([PeopleId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrganizationMembers] WITH NOCHECK ADD CONSTRAINT [FK_ORGANIZATION_MEMBERS_TBL_MemberType] FOREIGN KEY ([MemberTypeId]) REFERENCES [lookup].[MemberType] ([Id])
GO
ALTER TABLE [dbo].[OrganizationMembers] WITH NOCHECK ADD CONSTRAINT [ORGANIZATION_MEMBERS_ORG_FK] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[OrganizationMembers] WITH NOCHECK ADD CONSTRAINT [ORGANIZATION_MEMBERS_PPL_FK] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId]) ON DELETE CASCADE
GO
