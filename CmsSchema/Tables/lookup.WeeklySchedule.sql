CREATE TABLE [lookup].[WeeklySchedule]
(
[Id] [int] NOT NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Day] [int] NOT NULL,
[MeetingTime] [datetime] NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[WeeklySchedule] ADD CONSTRAINT [PK_WeeklySchedule] PRIMARY KEY CLUSTERED ([Id])
GO
