CREATE TABLE [dbo].[EmailQueue]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[SendWhen] [datetime] NULL,
[Subject] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Body] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FromAddr] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Sent] [datetime] NULL,
[Started] [datetime] NULL,
[Queued] [datetime] NOT NULL,
[FromName] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[QueuedBy] [int] NULL,
[Redacted] [bit] NULL,
[Transactional] [bit] NULL,
[Public] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmailQueue] ADD CONSTRAINT [PK_EmailQueue] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmailQueue] ADD CONSTRAINT [FK_EmailQueue_People] FOREIGN KEY ([QueuedBy]) REFERENCES [dbo].[People] ([PeopleId])
GO
