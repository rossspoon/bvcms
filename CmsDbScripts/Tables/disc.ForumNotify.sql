CREATE TABLE [disc].[ForumNotify]
(
[ThreadId] [int] NOT NULL,
[UserId] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[cUserid] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [disc].[ForumNotify] ADD CONSTRAINT [PK_ForumNotify] PRIMARY KEY CLUSTERED  ([ThreadId], [UserId]) ON [PRIMARY]
GO
ALTER TABLE [disc].[ForumNotify] ADD CONSTRAINT [FK_ForumNotify_ForumEntry] FOREIGN KEY ([ThreadId]) REFERENCES [disc].[ForumEntry] ([Id])
GO
