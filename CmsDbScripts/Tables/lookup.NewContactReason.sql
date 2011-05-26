CREATE TABLE [lookup].[NewContactReason]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[NewContactReason] ADD CONSTRAINT [PK_ContactReasons] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
