CREATE TABLE [disc].[PendingNotifications]
(
[PeopleId] [int] NOT NULL,
[NotifyType] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [disc].[PendingNotifications] ADD CONSTRAINT [PK_PendingNotifications_1] PRIMARY KEY CLUSTERED  ([PeopleId], [NotifyType]) ON [PRIMARY]
GO
ALTER TABLE [disc].[PendingNotifications] ADD CONSTRAINT [FK_PendingNotifications_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
