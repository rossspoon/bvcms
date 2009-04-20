CREATE TABLE [lookup].[AttendType]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[AttendType] ADD CONSTRAINT [PK_AttendType] PRIMARY KEY CLUSTERED ([Id])
GO
