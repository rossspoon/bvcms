CREATE TABLE [dbo].[EmailResponses]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[EmailQueueId] [int] NOT NULL,
[PeopleId] [int] NOT NULL,
[Type] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Dt] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmailResponses] ADD CONSTRAINT [PK_EmailResponses] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_EmailResponses] ON [dbo].[EmailResponses] ([EmailQueueId], [PeopleId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmailResponses] ADD CONSTRAINT [FK_EmailResponses_EmailQueue] FOREIGN KEY ([EmailQueueId]) REFERENCES [dbo].[EmailQueue] ([Id])
GO
ALTER TABLE [dbo].[EmailResponses] ADD CONSTRAINT [FK_EmailResponses_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
