CREATE TABLE [dbo].[ForumEntry]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Title] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Entry] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ForumId] [int] NULL,
[ReplyToId] [int] NULL,
[ThreadId] [int] NULL,
[DisplayOrder] [int] NULL,
[DisplayDepth] [int] NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ForumEntry] ADD CONSTRAINT [PK_ForumEntry] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ForumEntry] ADD CONSTRAINT [FK_ForumEntry_Forum] FOREIGN KEY ([ForumId]) REFERENCES [dbo].[Forum] ([Id])
GO
ALTER TABLE [dbo].[ForumEntry] ADD CONSTRAINT [FK_ForumEntry_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[ForumEntry] ADD CONSTRAINT [Replies__RepliedTo] FOREIGN KEY ([ReplyToId]) REFERENCES [dbo].[ForumEntry] ([Id])
GO
ALTER TABLE [dbo].[ForumEntry] ADD CONSTRAINT [ThreadEntries__ThreadPost] FOREIGN KEY ([ThreadId]) REFERENCES [dbo].[ForumEntry] ([Id])
GO
EXEC sp_addextendedproperty N'MS_Description', N'Replies->RepliedTo', 'SCHEMA', N'dbo', 'TABLE', N'ForumEntry', 'CONSTRAINT', N'Replies__RepliedTo'
GO
EXEC sp_addextendedproperty N'MS_Description', N'ThreadEntries->ThreadPost', 'SCHEMA', N'dbo', 'TABLE', N'ForumEntry', 'CONSTRAINT', N'ThreadEntries__ThreadPost'
GO
