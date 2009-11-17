CREATE TABLE [dbo].[ForumUserRead]
(
[ForumEntryId] [int] NOT NULL,
[UserId] [int] NOT NULL,
[CreatedOn] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ForumUserRead] ADD CONSTRAINT [PK_ForumUserRead] PRIMARY KEY CLUSTERED  ([ForumEntryId], [UserId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ForumUserRead] ADD CONSTRAINT [FK_ForumUserRead_ForumEntry] FOREIGN KEY ([ForumEntryId]) REFERENCES [dbo].[ForumEntry] ([Id])
GO
ALTER TABLE [dbo].[ForumUserRead] ADD CONSTRAINT [FK_ForumUserRead_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
