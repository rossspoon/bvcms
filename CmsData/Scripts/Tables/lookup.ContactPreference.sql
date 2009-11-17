CREATE TABLE [lookup].[ContactPreference]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[ContactPreference] ADD CONSTRAINT [PK_ContactPreference] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
