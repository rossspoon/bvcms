CREATE TABLE [lookup].[MeetingType]
(
[Id] [int] NOT NULL,
[Code] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[MeetingType] ADD CONSTRAINT [PK_MeetingType] PRIMARY KEY CLUSTERED ([Id])
GO
