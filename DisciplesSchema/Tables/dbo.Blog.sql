CREATE TABLE [dbo].[Blog]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Description] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Title] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GroupId] [int] NULL,
[IsPublic] [bit] NOT NULL CONSTRAINT [DF_Blog_IsPublic] DEFAULT ((0)),
[NotOnMenu] [bit] NOT NULL CONSTRAINT [DF_Blog_NotOnMenu] DEFAULT ((0)),
[OwnerId] [int] NULL
)

GO
ALTER TABLE [dbo].[Blog] ADD CONSTRAINT [PK_Blog] PRIMARY KEY CLUSTERED ([Id])
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Blog] ON [dbo].[Blog] ([Name])
GO
ALTER TABLE [dbo].[Blog] ADD CONSTRAINT [FK_Blog_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id])
GO
ALTER TABLE [dbo].[Blog] ADD CONSTRAINT [FK_Blog_Users] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Users] ([UserId])
GO
