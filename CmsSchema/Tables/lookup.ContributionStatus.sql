CREATE TABLE [lookup].[ContributionStatus]
(
[Id] [int] NOT NULL,
[Code] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[ContributionStatus] ADD CONSTRAINT [PK_ContributionStatus] PRIMARY KEY CLUSTERED ([Id])
GO
