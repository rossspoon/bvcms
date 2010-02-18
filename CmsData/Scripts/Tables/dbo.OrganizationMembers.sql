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
[UserData] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Amount] [money] NULL,
[Request] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)






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

CREATE TRIGGER [dbo].[delOrganizationMember] 
   ON  [dbo].[OrganizationMembers]
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE dbo.Organizations
	SET MemberCount = dbo.OrganizationMemberCount(OrganizationId)
	WHERE OrganizationId IN 
	(SELECT OrganizationId FROM DELETED GROUP BY OrganizationId)

	UPDATE dbo.People
	SET Grade = dbo.SchoolGrade(PeopleId)
	WHERE PeopleId IN (SELECT PeopleId FROM DELETED)

	DECLARE c CURSOR FOR
	SELECT d.OrganizationId, MemberTypeId, o.LeaderMemberTypeId FROM DELETED d
	JOIN dbo.Organizations o ON o.OrganizationId = d.OrganizationId
	OPEN c;
	DECLARE @oid INT, @mt INT, @lmt INT
	FETCH NEXT FROM c INTO @oid, @mt, @lmt;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (@mt = @lmt)
		BEGIN
			UPDATE dbo.Organizations
			SET LeaderId = dbo.OrganizationLeaderId(OrganizationId),
			LeaderName = dbo.OrganizationLeaderName(OrganizationId)
			WHERE OrganizationId = @oid
			
			UPDATE dbo.People
			SET BibleFellowshipClassId = dbo.BibleFellowshipClassId(p.PeopleId)
			FROM dbo.People p
			JOIN dbo.OrganizationMembers om ON p.PeopleId = om.PeopleId
			WHERE om.OrganizationId = @oid
		END
		FETCH NEXT FROM c INTO @oid, @mt, @lmt;
	END;
	CLOSE c;
	DEALLOCATE c;

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

CREATE TRIGGER [dbo].[insOrganizationMember] 
   ON  [dbo].[OrganizationMembers]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE dbo.Organizations
	SET MemberCount = dbo.OrganizationMemberCount(OrganizationId)
	WHERE OrganizationId IN 
	(SELECT OrganizationId FROM INSERTED GROUP BY OrganizationId)

	UPDATE dbo.People
	SET Grade = dbo.SchoolGrade(PeopleId)
	WHERE PeopleId IN (SELECT PeopleId FROM INSERTED)

	DECLARE c CURSOR FOR
	SELECT d.OrganizationId, MemberTypeId, o.LeaderMemberTypeId FROM INSERTED d
	JOIN dbo.Organizations o ON o.OrganizationId = d.OrganizationId
	OPEN c;
	DECLARE @oid INT, @mt INT, @lmt INT
	FETCH NEXT FROM c INTO @oid, @mt, @lmt;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (@mt = @lmt)
			UPDATE dbo.Organizations
			SET LeaderId = dbo.OrganizationLeaderId(OrganizationId),
			LeaderName = dbo.OrganizationLeaderName(OrganizationId)
			WHERE OrganizationId = @oid

			UPDATE dbo.People
			SET BibleFellowshipClassId = dbo.BibleFellowshipClassId(p.PeopleId)
			FROM dbo.People p
			JOIN dbo.OrganizationMembers om ON p.PeopleId = om.PeopleId
			WHERE om.OrganizationId = @oid
		FETCH NEXT FROM c INTO @oid, @mt, @lmt;
	END;
	CLOSE c;
	DEALLOCATE c;
END
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE TRIGGER [dbo].[updOrganizationMember] 
   ON  [dbo].[OrganizationMembers]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF UPDATE(Pending)
	BEGIN
		UPDATE dbo.People
		SET Grade = dbo.SchoolGrade(PeopleId) 
		WHERE PeopleId IN (SELECT PeopleId FROM INSERTED)

		UPDATE dbo.Organizations
		SET MemberCount = dbo.OrganizationMemberCount(OrganizationId)
		WHERE OrganizationId IN 
		(SELECT OrganizationId FROM INSERTED GROUP BY OrganizationId)
	END

	DECLARE c CURSOR FOR
	SELECT d.OrganizationId, d.MemberTypeId, i.MemberTypeId, o.LeaderMemberTypeId 
	FROM DELETED d
	JOIN INSERTED i ON i.OrganizationId = d.OrganizationId AND i.PeopleId = d.PeopleId
	JOIN dbo.Organizations o ON o.OrganizationId = d.OrganizationId
	
    IF UPDATE(MemberTypeId)
    OR UPDATE(Pending)
    BEGIN
		OPEN c;
		DECLARE @oid INT, @dmt INT, @imt INT, @lmt INT
		FETCH NEXT FROM c INTO @oid, @dmt, @imt, @lmt;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF (@dmt = @lmt OR @imt = @lmt)
			BEGIN
				UPDATE dbo.Organizations
				SET LeaderId = dbo.OrganizationLeaderId(OrganizationId),
				LeaderName = dbo.OrganizationLeaderName(OrganizationId)
				WHERE OrganizationId = @oid
				
				UPDATE dbo.People
				SET BibleFellowshipClassId = dbo.BibleFellowshipClassId(p.PeopleId)
				FROM dbo.People p
				JOIN dbo.OrganizationMembers om ON p.PeopleId = om.PeopleId
				WHERE om.OrganizationId = @oid
			END
			FETCH NEXT FROM c INTO @oid, @dmt, @imt, @lmt;
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
