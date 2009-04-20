CREATE TABLE [dbo].[Organizations]
(
[OrganizationId] [int] NOT NULL IDENTITY(1, 1),
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[RecordStatus] [bit] NOT NULL,
[OrganizationStatusId] [int] NOT NULL,
[OrganizationTypeId] [int] NOT NULL,
[GroupMeetingTypeId] [int] NOT NULL,
[DivisionId] [int] NOT NULL,
[LeaderMemberTypeId] [int] NULL,
[OrganizationSize] [int] NULL,
[GenderTypeId] [int] NOT NULL,
[MaritalStatusId] [int] NOT NULL,
[AgeRangeStart] [int] NULL,
[AgeRangeEnd] [int] NULL,
[GradeRangeStart] [int] NULL,
[GradeRangeEnd] [int] NULL,
[RollSheetTypeId] [int] NOT NULL,
[TrackVisitors] [bit] NOT NULL,
[RollSheetVisitorWks] [int] NULL,
[AttendTrkLevelId] [int] NOT NULL,
[SecurityTypeId] [int] NOT NULL,
[AttendClassificationId] [int] NOT NULL,
[AttendanceSummaryFlag] [bit] NOT NULL,
[QtrlySummaryInterval] [int] NULL,
[VipFlag] [bit] NOT NULL,
[Confidential] [bit] NOT NULL,
[FirstMeetingDate] [datetime] NULL,
[LastMeetingDate] [datetime] NULL,
[OrganizationClosedDate] [datetime] NULL,
[Location] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OrganizationName] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OrganizationCode] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OrganizationDescription] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UltIncidentId] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PromotableFlag] [bit] NULL,
[RollSheetPrintLead] [int] NOT NULL CONSTRAINT [DF__ORGANIZAT__ROLL___6ADB9D16] DEFAULT ((0)),
[MeetingSequence] [int] NOT NULL CONSTRAINT [DF__ORGANIZAT__MEETI__6BCFC14F] DEFAULT ((0)),
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL,
[ScheduleId] [int] NULL,
[EntryPointId] [int] NULL,
[ParentOrgId] [int] NULL,
[AllowAttendOverlap] [bit] NOT NULL CONSTRAINT [DF_Organizations_AllowAttendOverlap] DEFAULT ((0)),
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
