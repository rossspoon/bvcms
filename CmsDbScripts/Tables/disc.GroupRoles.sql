CREATE TABLE [disc].[GroupRoles]
(
[RoleName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RoleId] [int] NOT NULL IDENTITY(1, 1),
[GroupId] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [disc].[GroupRoles] ADD CONSTRAINT [PK_GroupRoles] PRIMARY KEY CLUSTERED  ([RoleId]) ON [PRIMARY]
GO
ALTER TABLE [disc].[GroupRoles] ADD CONSTRAINT [FK_GroupRoles_Group] FOREIGN KEY ([GroupId]) REFERENCES [disc].[Group] ([Id])
GO
