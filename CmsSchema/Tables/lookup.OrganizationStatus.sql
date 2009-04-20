CREATE TABLE [lookup].[OrganizationStatus]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[OrganizationStatus] ADD CONSTRAINT [PK_OrganizationStatus] PRIMARY KEY CLUSTERED ([Id])
GO
