CREATE TABLE [lookup].[GenderClass]
(
[Id] [int] NOT NULL,
[Code] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[GenderClass] ADD CONSTRAINT [PK_GenderClass] PRIMARY KEY CLUSTERED ([Id])
GO
