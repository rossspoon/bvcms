CREATE TABLE [dbo].[EmailQueueTo]
(
[Id] [int] NOT NULL,
[PeopleId] [int] NOT NULL,
[OrgId] [int] NULL,
[Sent] [datetime] NULL,
[AddEmail] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[guid] [uniqueidentifier] NULL,
[messageid] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmailQueueTo] ADD CONSTRAINT [PK_EmailQueueTo_1] PRIMARY KEY CLUSTERED  ([Id], [PeopleId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_EmailQueueTo] ON [dbo].[EmailQueueTo] ([guid]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmailQueueTo] ADD CONSTRAINT [FK_EmailQueueTo_EmailQueue] FOREIGN KEY ([Id]) REFERENCES [dbo].[EmailQueue] ([Id])
GO
ALTER TABLE [dbo].[EmailQueueTo] ADD CONSTRAINT [FK_EmailQueueTo_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
