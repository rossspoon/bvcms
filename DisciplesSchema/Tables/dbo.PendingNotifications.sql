CREATE TABLE [dbo].[PendingNotifications]
(
[UserId] [int] NOT NULL,
[NotifyType] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)

GO
ALTER TABLE [dbo].[PendingNotifications] ADD CONSTRAINT [PK_PendingNotifications] PRIMARY KEY CLUSTERED ([UserId], [NotifyType])
GO
ALTER TABLE [dbo].[PendingNotifications] ADD CONSTRAINT [FK_PendingNotifications_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
