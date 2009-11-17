CREATE TABLE [dbo].[ActivityLog]
(
[Id] [bigint] NOT NULL IDENTITY(1, 1),
[ActivityDate] [datetime] NULL,
[UserId] [int] NULL,
[Activity] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PageUrl] [varchar] (400) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ActivityLog] ADD CONSTRAINT [PK_alog] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ActivityLog] ADD CONSTRAINT [FK_ActivityLog_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
