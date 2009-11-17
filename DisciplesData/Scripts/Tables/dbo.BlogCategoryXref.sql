CREATE TABLE [dbo].[BlogCategoryXref]
(
[CatId] [int] NOT NULL,
[BlogPostId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogCategoryXref] ADD CONSTRAINT [PK_bc] PRIMARY KEY CLUSTERED  ([CatId], [BlogPostId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogCategoryXref] ADD CONSTRAINT [FK_BlogCategoryXref_BlogPost] FOREIGN KEY ([BlogPostId]) REFERENCES [dbo].[BlogPost] ([Id])
GO
ALTER TABLE [dbo].[BlogCategoryXref] ADD CONSTRAINT [FK_BlogCategoryXref_Category] FOREIGN KEY ([CatId]) REFERENCES [dbo].[Category] ([Id])
GO
