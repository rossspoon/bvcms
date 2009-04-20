CREATE TABLE [lookup].[VolunteerCodes]
(
[Id] [int] NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[VolunteerCodes] ADD CONSTRAINT [PK_VolunteerCodes] PRIMARY KEY CLUSTERED ([Id])
GO
