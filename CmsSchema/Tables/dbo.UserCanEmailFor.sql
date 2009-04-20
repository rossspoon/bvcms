CREATE TABLE [dbo].[UserCanEmailFor]
(
[UserId] [int] NOT NULL,
[CanEmailFor] [int] NOT NULL
)

ALTER TABLE [dbo].[UserCanEmailFor] ADD 
CONSTRAINT [PK_UserCanEmailFor] PRIMARY KEY CLUSTERED  ([UserId], [CanEmailFor])
ALTER TABLE [dbo].[UserCanEmailFor] ADD
CONSTRAINT [UsersICanEmailFor__Assistant] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
ALTER TABLE [dbo].[UserCanEmailFor] ADD
CONSTRAINT [UsersWhoCanEmailForMe__Boss] FOREIGN KEY ([CanEmailFor]) REFERENCES [dbo].[Users] ([UserId])

GO
