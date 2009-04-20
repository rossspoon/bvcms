CREATE TABLE [lookup].[DecisionType]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[DecisionType] ADD CONSTRAINT [PK_DecisionType] PRIMARY KEY CLUSTERED ([Id])
GO
