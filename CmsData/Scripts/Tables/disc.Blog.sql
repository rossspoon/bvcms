CREATE TABLE [disc].[Blog]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Description] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Title] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GroupId] [int] NULL,
[NotOnMenu] [bit] NOT NULL CONSTRAINT [DF_Blog_NotOnMenu] DEFAULT ((0)),
[OwnerId] [int] NULL,
[PrivacyLevel] [int] NOT NULL CONSTRAINT [DF__Blog__PrivacyLev__73501C2F] DEFAULT ((1)),
[cUserid] [int] NULL
)
GO
ALTER TABLE [disc].[Blog] ADD CONSTRAINT [PK_Blog] PRIMARY KEY CLUSTERED  ([Id])
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Blog] ON [disc].[Blog] ([Name])
GO
ALTER TABLE [disc].[Blog] ADD CONSTRAINT [FK_Blog_Group] FOREIGN KEY ([GroupId]) REFERENCES [disc].[Group] ([Id])
GO
ALTER TABLE [disc].[Blog] ADD CONSTRAINT [FK_Blog_Users] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Users] ([UserId])
GO
