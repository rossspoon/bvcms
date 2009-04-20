CREATE TABLE [dbo].[TagTag]
(
[Id] [int] NOT NULL,
[ParentTagId] [int] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[TagTag] ADD CONSTRAINT [PK_TagTag] PRIMARY KEY CLUSTERED  ([Id], [ParentTagId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_TagTag] ON [dbo].[TagTag] ([ParentTagId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TagTag] WITH NOCHECK ADD CONSTRAINT [Tags__ParentTag] FOREIGN KEY ([ParentTagId]) REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[TagTag] WITH NOCHECK ADD CONSTRAINT [TagTags__Tag] FOREIGN KEY ([Id]) REFERENCES [dbo].[Tag] ([Id])
GO
