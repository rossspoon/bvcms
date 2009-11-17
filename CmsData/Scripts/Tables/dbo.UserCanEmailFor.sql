CREATE TABLE [dbo].[UserCanEmailFor]
(
[UserId] [int] NOT NULL,
[CanEmailFor] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserCanEmailFor] ADD CONSTRAINT [PK_UserCanEmailFor] PRIMARY KEY CLUSTERED  ([UserId], [CanEmailFor]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserCanEmailFor] ADD CONSTRAINT [UsersICanEmailFor__Assistant] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserCanEmailFor] ADD CONSTRAINT [UsersWhoCanEmailForMe__Boss] FOREIGN KEY ([CanEmailFor]) REFERENCES [dbo].[Users] ([UserId])
GO
