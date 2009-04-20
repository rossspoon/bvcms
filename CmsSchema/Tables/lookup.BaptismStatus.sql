CREATE TABLE [lookup].[BaptismStatus]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[BaptismStatus] ADD CONSTRAINT [PK_BaptismStatus] PRIMARY KEY CLUSTERED ([Id])
GO
