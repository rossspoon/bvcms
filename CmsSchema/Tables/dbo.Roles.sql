CREATE TABLE [dbo].[Roles]
(
[RoleName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RoleId] [int] NOT NULL IDENTITY(1, 1)
)

ALTER TABLE [dbo].[Roles] ADD 
CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED  ([RoleId])
CREATE UNIQUE NONCLUSTERED INDEX [IX_Roles] ON [dbo].[Roles] ([RoleName])


GO
