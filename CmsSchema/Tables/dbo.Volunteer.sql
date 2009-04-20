CREATE TABLE [dbo].[Volunteer]
(
[PeopleId] [int] NOT NULL,
[StatusId] [int] NULL,
[ProcessedDate] [datetime] NULL,
[Standard] [bit] NOT NULL CONSTRAINT [DF_Volunteer_Standard] DEFAULT ((0)),
[Children] [bit] NOT NULL CONSTRAINT [DF_Volunteer_Children] DEFAULT ((0)),
[Leader] [bit] NOT NULL CONSTRAINT [DF_Volunteer_Leader] DEFAULT ((0)),
[Comments] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
ALTER TABLE [dbo].[Volunteer] ADD
CONSTRAINT [FK_Volunteer_PEOPLE_TBL] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
ALTER TABLE [dbo].[Volunteer] ADD
CONSTRAINT [FK_Volunteer_VolApplicationStatus] FOREIGN KEY ([StatusId]) REFERENCES [lookup].[VolApplicationStatus] ([Id])



GO
ALTER TABLE [dbo].[Volunteer] ADD CONSTRAINT [PK_VolunteerApproval] PRIMARY KEY CLUSTERED  ([PeopleId])
GO
