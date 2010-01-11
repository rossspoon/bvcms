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
)
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[insdelOrganizationMember] 
   ON  [dbo].[OrganizationMembers]
   AFTER INSERT, DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE dbo.Organizations
	SET MemberCount = dbo.OrganizationMemberCount(OrganizationId)
	WHERE OrganizationId IN (SELECT OrganizationId FROM INSERTED)
	OR OrganizationId IN (SELECT OrganizationId FROM DELETED)

	UPDATE dbo.People
	SET Grade = dbo.SchoolGrade(PeopleId) 
	WHERE PeopleId IN (SELECT PeopleId FROM INSERTED)
	OR PeopleId IN (SELECT PeopleId FROM DELETED)

	UPDATE dbo.People
	SET BibleFellowshipTeacherId = dbo.BibleFellowshipTeacherId(p.PeopleId),
	BibleFellowshipClassId = dbo.BibleFellowshipClassId(p.PeopleId),
	BibleFellowshipTeacher = dbo.BibleFellowshipTeacher(p.PeopleId),
	InBFClass = dbo.InBFClass(p.PeopleId)
	FROM dbo.People p
	JOIN dbo.OrganizationMembers om ON p.PeopleId = om.PeopleId
	WHERE om.OrganizationId IN (SELECT OrganizationId FROM DELETED)
	OR om.OrganizationId IN (SELECT OrganizationId FROM INSERTED)
END
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
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF UPDATE(MemberTypeId)
	BEGIN
		UPDATE dbo.Organizations
		SET LeaderId = dbo.OrganizationLeaderId(OrganizationId),
		LeaderName = dbo.OrganizationLeaderName(OrganizationId)
		WHERE OrganizationId IN (SELECT OrganizationId FROM INSERTED)

	END
	
	IF UPDATE(Pending)
	BEGIN
		UPDATE dbo.People
		SET Grade = dbo.SchoolGrade(PeopleId) 
		WHERE PeopleId IN (SELECT PeopleId FROM INSERTED)

		UPDATE dbo.Organizations
		SET MemberCount = dbo.OrganizationMemberCount(OrganizationId)
		WHERE OrganizationId IN (SELECT OrganizationId FROM INSERTED)
	END

	UPDATE dbo.People
	SET BibleFellowshipTeacherId = dbo.BibleFellowshipTeacherId(p.PeopleId),
	BibleFellowshipClassId = dbo.BibleFellowshipClassId(p.PeopleId),
	BibleFellowshipTeacher = dbo.BibleFellowshipTeacher(p.PeopleId),
	InBFClass = dbo.InBFClass(p.PeopleId)
	FROM dbo.People p
	JOIN dbo.OrganizationMembers om ON p.PeopleId = om.PeopleId
	WHERE om.OrganizationId IN 
	(	SELECT i.OrganizationId FROM INSERTED i
		JOIN DELETED d ON i.OrganizationId = d.OrganizationId AND i.PeopleId = d.PeopleId
		JOIN dbo.Organizations o ON o.OrganizationId = i.OrganizationId
		WHERE o.LeaderMemberTypeId IN (i.MemberTypeId, d.MemberTypeId)
	)
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
