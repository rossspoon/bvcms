CREATE TABLE [disc].[BlogComment]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Comment] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BlogPostId] [int] NULL,
[DatePosted] [datetime] NOT NULL,
[PosterId] [int] NULL,
[cUserid] [int] NULL
)
GO
ALTER TABLE [disc].[BlogComment] ADD CONSTRAINT [PK_BlogComment] PRIMARY KEY CLUSTERED  ([Id])
GO
ALTER TABLE [disc].[BlogComment] ADD CONSTRAINT [FK_BlogComment_BlogPost] FOREIGN KEY ([BlogPostId]) REFERENCES [disc].[BlogPost] ([Id])
GO
ALTER TABLE [disc].[BlogComment] ADD CONSTRAINT [FK_BlogComment_Users] FOREIGN KEY ([PosterId]) REFERENCES [dbo].[Users] ([UserId])
GO
