CREATE TABLE [disc].[ForumEntry]
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
[CreatedBy] [int] NULL,
[cUserid] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [disc].[ForumEntry] ADD CONSTRAINT [PK_ForumEntry] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [disc].[ForumEntry] ADD CONSTRAINT [FK_ForumEntry_Forum] FOREIGN KEY ([ForumId]) REFERENCES [disc].[Forum] ([Id])
GO
ALTER TABLE [disc].[ForumEntry] ADD CONSTRAINT [FK_ForumEntry_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [disc].[ForumEntry] ADD CONSTRAINT [Replies__RepliedTo] FOREIGN KEY ([ReplyToId]) REFERENCES [disc].[ForumEntry] ([Id])
GO
ALTER TABLE [disc].[ForumEntry] ADD CONSTRAINT [ThreadEntries__ThreadPost] FOREIGN KEY ([ThreadId]) REFERENCES [disc].[ForumEntry] ([Id])
GO
