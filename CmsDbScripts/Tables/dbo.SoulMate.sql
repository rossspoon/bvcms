CREATE TABLE [dbo].[SoulMate]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[HimId] [int] NULL,
[HisEmail] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HisEmailPreferred] [bit] NULL,
[HerId] [int] NULL,
[HerEmail] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HerEmailPreferred] [bit] NULL,
[EventId] [int] NULL,
[Relationship] [int] NULL,
[ChildcareId] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SoulMate] ADD CONSTRAINT [PK_SoulMate] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_SoulMate] ON [dbo].[SoulMate] ([EventId], [HimId], [HerId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SoulMate] ADD CONSTRAINT [ChildSoulMates__ChildCareMeeting] FOREIGN KEY ([ChildcareId]) REFERENCES [dbo].[Meetings] ([MeetingId])
GO
ALTER TABLE [dbo].[SoulMate] ADD CONSTRAINT [FK_SoulMate_Meetings] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Meetings] ([MeetingId])
GO
