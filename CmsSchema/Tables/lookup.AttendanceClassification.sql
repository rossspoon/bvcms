CREATE TABLE [lookup].[AttendanceClassification]
(
[Id] [int] NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[AttendanceClassification] ADD CONSTRAINT [PK_AttendanceClassification] PRIMARY KEY CLUSTERED ([Id])
GO
