CREATE TABLE [dbo].[Organizations]
(
[OrganizationId] [int] NOT NULL IDENTITY(1, 1),
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[OrganizationStatusId] [int] NOT NULL,
[DivisionId] [int] NOT NULL,
[LeaderMemberTypeId] [int] NULL,
[RollSheetVisitorWks] [int] NULL,
[AttendTrkLevelId] [int] NOT NULL,
[AttendClassificationId] [int] NOT NULL CONSTRAINT [DF_Organizations_AttendClassificationId] DEFAULT ((0)),
[SecurityTypeId] [int] NOT NULL,
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
[GradeRangeStart] [int] NULL,
[GradeRangeEnd] [int] NULL,
[MemberCount] AS ([dbo].[OrganizationMemberCount]([OrganizationId])),
[LeaderId] AS ([dbo].[OrganizationLeaderId]([OrganizationId])),
[LeaderName] AS ([dbo].[organizationLeaderName]([OrganizationId]))
)
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [ORGANIZATIONS_PK] PRIMARY KEY NONCLUSTERED ([OrganizationId])
GO
CREATE NONCLUSTERED INDEX [IX_ORGANIZATIONS_TBL] ON [dbo].[Organizations] ([ScheduleId])
GO
ALTER TABLE [dbo].[Organizations] WITH NOCHECK ADD CONSTRAINT [ChildOrgs__ParentOrg] FOREIGN KEY ([ParentOrgId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_ORGANIZATIONS_TBL_AttendTrackLevel] FOREIGN KEY ([AttendTrkLevelId]) REFERENCES [lookup].[AttendTrackLevel] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_ORGANIZATIONS_TBL_EntryPoint] FOREIGN KEY ([EntryPointId]) REFERENCES [lookup].[EntryPoint] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_ORGANIZATIONS_TBL_OrganizationStatus] FOREIGN KEY ([OrganizationStatusId]) REFERENCES [lookup].[OrganizationStatus] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_ORGANIZATIONS_TBL_WeeklySchedule] FOREIGN KEY ([ScheduleId]) REFERENCES [lookup].[WeeklySchedule] ([Id])
GO
