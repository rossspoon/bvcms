CREATE TABLE [dbo].[BlogComment]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Comment] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BlogPostId] [int] NULL,
[DatePosted] [datetime] NOT NULL,
[PosterId] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogComment] ADD CONSTRAINT [PK_BlogComment] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogComment] ADD CONSTRAINT [FK_BlogComment_BlogPost] FOREIGN KEY ([BlogPostId]) REFERENCES [dbo].[BlogPost] ([Id])
GO
ALTER TABLE [dbo].[BlogComment] ADD CONSTRAINT [FK_BlogComment_Users] FOREIGN KEY ([PosterId]) REFERENCES [dbo].[Users] ([UserId])
GO
