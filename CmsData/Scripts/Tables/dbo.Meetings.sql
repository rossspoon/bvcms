CREATE TABLE [dbo].[Meetings]
(
[MeetingId] [int] NOT NULL IDENTITY(1, 1),
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[OrganizationId] [int] NOT NULL,
[NumPresent] [int] NOT NULL CONSTRAINT [DF__MEETINGS___NUM_P__4D4B3A2F] DEFAULT ((0)),
[NumMembers] [int] NOT NULL CONSTRAINT [DF__MEETINGS___NUM_M__4F3382A1] DEFAULT ((0)),
[NumVstMembers] [int] NOT NULL CONSTRAINT [DF__MEETINGS___NUM_V__5027A6DA] DEFAULT ((0)),
[NumRepeatVst] [int] NOT NULL CONSTRAINT [DF__MEETINGS___NUM_R__511BCB13] DEFAULT ((0)),
[NumNewVisit] [int] NOT NULL CONSTRAINT [DF__MEETINGS___NUM_N__520FEF4C] DEFAULT ((0)),
[Location] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MeetingDate] [datetime] NULL,
[GroupMeetingFlag] [bit] NOT NULL CONSTRAINT [DF__MEETINGS___GROUP__5AA5354D] DEFAULT ((0)),
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Meetings] ADD CONSTRAINT [MEETINGS_PK] PRIMARY KEY NONCLUSTERED  ([MeetingId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Meetings_MeetingDate] ON [dbo].[Meetings] ([MeetingDate] DESC) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_MEETINGS_ORG_ID] ON [dbo].[Meetings] ([OrganizationId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Meetings] WITH NOCHECK ADD CONSTRAINT [FK_MEETINGS_TBL_ORGANIZATIONS_TBL] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
