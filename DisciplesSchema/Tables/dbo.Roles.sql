CREATE TABLE [dbo].[Roles]
(
[RoleName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RoleId] [int] NOT NULL IDENTITY(1, 1),
[GroupId] [int] NULL
)

GO
ALTER TABLE [dbo].[Roles] ADD CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleId])
GO
ALTER TABLE [dbo].[Roles] ADD CONSTRAINT [FK_Roles_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id])
GO
