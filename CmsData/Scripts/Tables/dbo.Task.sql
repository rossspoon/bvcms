CREATE TABLE [dbo].[Task]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[OwnerId] [int] NOT NULL,
[ListId] [int] NOT NULL,
[CoOwnerId] [int] NULL,
[CoListId] [int] NULL,
[StatusId] [int] NULL,
[CreatedOn] [datetime] NOT NULL,
[SourceContactId] [int] NULL,
[CompletedContactId] [int] NULL,
[Notes] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ModifiedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[Project] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Archive] [bit] NOT NULL CONSTRAINT [DF_Task_Archive] DEFAULT ((0)),
[Priority] [int] NULL,
[WhoId] [int] NULL,
[Due] [datetime] NULL,
[Location] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CompletedOn] [datetime] NULL,
[ForceCompleteWContact] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Task] ADD CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Task] ON [dbo].[Task] ([OwnerId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Task] ADD CONSTRAINT [CoTasks__CoTaskList] FOREIGN KEY ([CoListId]) REFERENCES [dbo].[TaskList] ([Id])
GO
ALTER TABLE [dbo].[Task] ADD CONSTRAINT [FK_Task_TaskStatus] FOREIGN KEY ([StatusId]) REFERENCES [lookup].[TaskStatus] ([Id])
GO
ALTER TABLE [dbo].[Task] ADD CONSTRAINT [Tasks__Owner] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Task] ADD CONSTRAINT [Tasks__TaskList] FOREIGN KEY ([ListId]) REFERENCES [dbo].[TaskList] ([Id])
GO
ALTER TABLE [dbo].[Task] ADD CONSTRAINT [TasksAboutPerson__AboutWho] FOREIGN KEY ([WhoId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Task] ADD CONSTRAINT [TasksAssigned__SourceContact] FOREIGN KEY ([SourceContactId]) REFERENCES [dbo].[NewContact] ([ContactId])
GO
ALTER TABLE [dbo].[Task] ADD CONSTRAINT [TasksCompleted__CompletedContact] FOREIGN KEY ([CompletedContactId]) REFERENCES [dbo].[NewContact] ([ContactId])
GO
ALTER TABLE [dbo].[Task] ADD CONSTRAINT [TasksCoOwned__CoOwner] FOREIGN KEY ([CoOwnerId]) REFERENCES [dbo].[People] ([PeopleId])
GO
