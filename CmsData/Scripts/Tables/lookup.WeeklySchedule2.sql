CREATE TABLE [lookup].[WeeklySchedule2]
(
[Id] [int] NOT NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Day] [int] NOT NULL,
[MeetingTime] [datetime] NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ScheduleId] AS ([dbo].[ScheduleId]([Day],[MeetingTime]))
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[WeeklySchedule2] ADD CONSTRAINT [PK_WeeklySchedule] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
