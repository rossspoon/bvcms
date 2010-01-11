CREATE TABLE [dbo].[Organizations]
(
[OrganizationId] [int] NOT NULL IDENTITY(1, 1),
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[OrganizationStatusId] [int] NOT NULL,
[DivisionId] [int] NULL,
[LeaderMemberTypeId] [int] NULL,
[GradeRangeStart] [int] NULL,
[GradeRangeEnd] [int] NULL,
[RollSheetVisitorWks] [int] NULL,
[AttendTrkLevelId] [int] NOT NULL,
[SecurityTypeId] [int] NOT NULL,
[AttendClassificationId] [int] NOT NULL CONSTRAINT [DF_Organizations_AttendClassificationId] DEFAULT ((0)),
[FirstMeetingDate] [datetime] NULL,
[LastMeetingDate] [datetime] NULL,
[OrganizationClosedDate] [datetime] NULL,
[Location] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OrganizationName] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL,
[ScheduleId] [int] NULL,
[EntryPointId] [int] NULL,
[ParentOrgId] [int] NULL,
[AllowAttendOverlap] [bit] NOT NULL CONSTRAINT [DF_Organizations_AllowAttendOverlap] DEFAULT ((0)),
[MemberCount] [int] NULL,
[LeaderId] [int] NULL,
[LeaderName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ClassFilled] [bit] NULL,
[OnLineCatalogSort] [int] NULL,
[PendingLoc] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CanSelfCheckin] [bit] NULL,
[NumCheckInLabels] [int] NULL,
[CampusId] [int] NULL,
[AllowNonCampusCheckIn] [bit] NULL,
[NumWorkerCheckInLabels] [int] NULL,
[SchedTime] [datetime] NULL,
[SchedDay] [int] NULL,
[MeetingTime] [datetime] NULL,
[ShowOnlyRegisteredAtCheckIn] [bit] NULL
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
CREATE TRIGGER [dbo].[updOrg] 
   ON  [dbo].[Organizations] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	IF UPDATE(SchedDay) OR UPDATE(SchedTime)
	BEGIN
		UPDATE dbo.Organizations
		SET ScheduleId = dbo.ScheduleId(SchedDay, SchedTime),
		MeetingTime = dbo.GetScheduleTime(SchedDay, SchedTime)
		WHERE OrganizationId IN (SELECT OrganizationId FROM INSERTED)
	END
	IF UPDATE(LeaderMemberTypeId)
	OR UPDATE(DivisionId)
	BEGIN
		UPDATE dbo.People
		SET BibleFellowshipTeacherId = dbo.BibleFellowshipTeacherId(p.PeopleId),
		BibleFellowshipClassId = dbo.BibleFellowshipClassId(p.PeopleId),
		BibleFellowshipTeacher = dbo.BibleFellowshipTeacher(p.PeopleId),
		InBFClass = dbo.InBFClass(p.PeopleId)
		FROM dbo.People p
		JOIN dbo.OrganizationMembers m ON p.PeopleId = m.PeopleId
		JOIN DELETED o ON m.OrganizationId = o.OrganizationId
		JOIN dbo.Division d ON d.Id = o.DivisionId
		JOIN dbo.Program pr ON pr.Id = d.ProgId
		WHERE pr.BFProgram = 1

		UPDATE dbo.People
		SET BibleFellowshipTeacherId = dbo.BibleFellowshipTeacherId(p.PeopleId),
		BibleFellowshipClassId = dbo.BibleFellowshipClassId(p.PeopleId),
		BibleFellowshipTeacher = dbo.BibleFellowshipTeacher(p.PeopleId),
		InBFClass = dbo.InBFClass(p.PeopleId)
		FROM dbo.People p
		JOIN dbo.OrganizationMembers m ON p.PeopleId = m.PeopleId
		JOIN INSERTED o ON m.OrganizationId = o.OrganizationId
		JOIN dbo.Division d ON d.Id = o.DivisionId
		JOIN dbo.Program pr ON pr.Id = d.ProgId
		WHERE pr.BFProgram = 1
	END
END
GO



ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [ORGANIZATIONS_PK] PRIMARY KEY NONCLUSTERED  ([OrganizationId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Organizations] WITH NOCHECK ADD CONSTRAINT [ChildOrgs__ParentOrg] FOREIGN KEY ([ParentOrgId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_Organizations_Campus] FOREIGN KEY ([CampusId]) REFERENCES [lookup].[Campus] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_Organizations_Division] FOREIGN KEY ([DivisionId]) REFERENCES [dbo].[Division] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_ORGANIZATIONS_TBL_AttendTrackLevel] FOREIGN KEY ([AttendTrkLevelId]) REFERENCES [lookup].[AttendTrackLevel] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_ORGANIZATIONS_TBL_EntryPoint] FOREIGN KEY ([EntryPointId]) REFERENCES [lookup].[EntryPoint] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_ORGANIZATIONS_TBL_OrganizationStatus] FOREIGN KEY ([OrganizationStatusId]) REFERENCES [lookup].[OrganizationStatus] ([Id])
GO
