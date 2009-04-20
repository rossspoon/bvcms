CREATE TABLE [dbo].[ActivityLog]
(
[Id] [bigint] NOT NULL IDENTITY(1, 1),
[ActivityDate] [datetime] NULL,
[UserId] [int] NULL,
[Activity] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PageUrl] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
ALTER TABLE [dbo].[ActivityLog] ADD 
CONSTRAINT [PK_alog] PRIMARY KEY CLUSTERED ([Id])






GO

ALTER TABLE [dbo].[ActivityLog] ADD CONSTRAINT [FK_ActivityLog_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
