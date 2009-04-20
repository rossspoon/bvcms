CREATE TABLE [lookup].[MemberType]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AttendanceTypeId] [int] NOT NULL
)
GO
ALTER TABLE [lookup].[MemberType] ADD CONSTRAINT [PK_MemberType] PRIMARY KEY CLUSTERED ([Id])
GO
