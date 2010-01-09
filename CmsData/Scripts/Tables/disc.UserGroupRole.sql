CREATE TABLE [disc].[UserGroupRole]
(
[UserId] [int] NOT NULL,
[RoleId] [int] NOT NULL,
[cUserid] [int] NULL
)
GO
ALTER TABLE [disc].[UserGroupRole] ADD CONSTRAINT [PK_UserGroupRole] PRIMARY KEY CLUSTERED  ([UserId], [RoleId])
GO
CREATE NONCLUSTERED INDEX [IX_UserGroupRole] ON [disc].[UserGroupRole] ([cUserid], [RoleId])
GO
ALTER TABLE [disc].[UserGroupRole] ADD CONSTRAINT [FK_UserGroupRole_GroupRoles] FOREIGN KEY ([RoleId]) REFERENCES [disc].[GroupRoles] ([RoleId])
GO
ALTER TABLE [disc].[UserGroupRole] ADD CONSTRAINT [FK_UserGroupRole_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
