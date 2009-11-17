CREATE TABLE [dbo].[UserRole]
(
[UserId] [int] NOT NULL,
[RoleId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED  ([UserId], [RoleId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT [FK_UserRole_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT [FK_UserRole_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
