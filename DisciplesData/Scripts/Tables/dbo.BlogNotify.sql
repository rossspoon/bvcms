CREATE TABLE [dbo].[BlogNotify]
(
[BlogId] [int] NOT NULL,
[UserId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogNotify] ADD CONSTRAINT [PK_BlogNotify] PRIMARY KEY CLUSTERED  ([BlogId], [UserId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogNotify] ADD CONSTRAINT [FK_BlogNotify_Blog] FOREIGN KEY ([BlogId]) REFERENCES [dbo].[Blog] ([Id])
GO
ALTER TABLE [dbo].[BlogNotify] ADD CONSTRAINT [FK_BlogNotify_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
