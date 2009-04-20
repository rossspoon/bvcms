CREATE TABLE [lookup].[TaskStatus]
(
[Id] [int] NOT NULL,
[Code] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[TaskStatus] ADD CONSTRAINT [PK_TaskStatus] PRIMARY KEY CLUSTERED ([Id])
GO
