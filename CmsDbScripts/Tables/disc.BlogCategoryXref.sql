CREATE TABLE [disc].[BlogCategoryXref]
(
[CatId] [int] NOT NULL,
[BlogPostId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [disc].[BlogCategoryXref] ADD CONSTRAINT [PK_bc] PRIMARY KEY CLUSTERED  ([CatId], [BlogPostId]) ON [PRIMARY]
GO
ALTER TABLE [disc].[BlogCategoryXref] ADD CONSTRAINT [FK_BlogCategoryXref_BlogPost] FOREIGN KEY ([BlogPostId]) REFERENCES [disc].[BlogPost] ([Id])
GO
ALTER TABLE [disc].[BlogCategoryXref] ADD CONSTRAINT [FK_BlogCategoryXref_Category] FOREIGN KEY ([CatId]) REFERENCES [disc].[BlogCategory] ([Id])
GO
