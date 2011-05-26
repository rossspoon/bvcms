CREATE TABLE [disc].[BlogNotify]
(
[BlogId] [int] NOT NULL,
[UserId] [int] NOT NULL,
[cUserid] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [disc].[BlogNotify] ADD CONSTRAINT [PK_BlogNotify] PRIMARY KEY CLUSTERED  ([BlogId], [UserId]) ON [PRIMARY]
GO
ALTER TABLE [disc].[BlogNotify] ADD CONSTRAINT [FK_BlogNotify_Blog] FOREIGN KEY ([BlogId]) REFERENCES [disc].[Blog] ([Id])
GO
ALTER TABLE [disc].[BlogNotify] ADD CONSTRAINT [FK_BlogNotify_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
