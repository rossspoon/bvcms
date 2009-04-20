CREATE TABLE [dbo].[ForumNotify]
(
[ThreadId] [int] NOT NULL,
[UserId] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)

GO
ALTER TABLE [dbo].[ForumNotify] ADD CONSTRAINT [PK_ForumNotify] PRIMARY KEY CLUSTERED ([ThreadId], [UserId])
GO
ALTER TABLE [dbo].[ForumNotify] WITH NOCHECK ADD CONSTRAINT [FK_ForumNotify_ForumEntry] FOREIGN KEY ([ThreadId]) REFERENCES [dbo].[ForumEntry] ([Id])
GO
