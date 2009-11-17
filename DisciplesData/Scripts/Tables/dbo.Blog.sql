CREATE TABLE [dbo].[Blog]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Description] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Title] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GroupId] [int] NULL,
[NotOnMenu] [bit] NOT NULL CONSTRAINT [DF_Blog_NotOnMenu] DEFAULT ((0)),
[OwnerId] [int] NULL,
[PrivacyLevel] [int] NOT NULL CONSTRAINT [DF__Blog__PrivacyLev__73501C2F] DEFAULT ((1))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Blog] ADD CONSTRAINT [PK_Blog] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Blog] ON [dbo].[Blog] ([Name]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Blog] ADD CONSTRAINT [FK_Blog_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id])
GO
ALTER TABLE [dbo].[Blog] ADD CONSTRAINT [FK_Blog_Users] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Users] ([UserId])
GO
