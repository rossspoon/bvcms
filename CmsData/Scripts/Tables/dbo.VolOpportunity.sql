CREATE TABLE [dbo].[VolOpportunity]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailNoCva] [varchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailYesCva] [varchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[email] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UrlKey] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ExtraQuestion] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ExtraInstructions] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MaxChecks] [int] NULL,
[HelpMessage] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VolOpportunity] ADD CONSTRAINT [PK_VolOpportunity] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
