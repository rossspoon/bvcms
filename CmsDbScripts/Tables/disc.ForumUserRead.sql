CREATE TABLE [disc].[ForumUserRead]
(
[ForumEntryId] [int] NOT NULL,
[UserId] [int] NOT NULL,
[CreatedOn] [datetime] NULL,
[cUserid] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [disc].[ForumUserRead] ADD CONSTRAINT [PK_ForumUserRead] PRIMARY KEY CLUSTERED  ([ForumEntryId], [UserId]) ON [PRIMARY]
GO
ALTER TABLE [disc].[ForumUserRead] ADD CONSTRAINT [FK_ForumUserRead_ForumEntry] FOREIGN KEY ([ForumEntryId]) REFERENCES [disc].[ForumEntry] ([Id])
GO
ALTER TABLE [disc].[ForumUserRead] ADD CONSTRAINT [FK_ForumUserRead_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
