CREATE TABLE [dbo].[BlogCategory]
(
[Category] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BlogPostId] [int] NOT NULL
)

GO
ALTER TABLE [dbo].[BlogCategory] ADD CONSTRAINT [PK_BlogCategory] PRIMARY KEY CLUSTERED ([Category], [BlogPostId])
GO
ALTER TABLE [dbo].[BlogCategory] ADD CONSTRAINT [FK_BlogCategory_Blog] FOREIGN KEY ([BlogPostId]) REFERENCES [dbo].[BlogPost] ([Id])
GO
