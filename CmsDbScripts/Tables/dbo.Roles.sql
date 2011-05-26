CREATE TABLE [dbo].[Roles]
(
[RoleName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RoleId] [int] NOT NULL IDENTITY(1, 1)
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Roles] ADD CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED  ([RoleId]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Roles] ON [dbo].[Roles] ([RoleName]) ON [PRIMARY]
GO
