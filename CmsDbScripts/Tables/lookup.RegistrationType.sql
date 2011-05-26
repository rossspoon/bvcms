CREATE TABLE [lookup].[RegistrationType]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[RegistrationType] ADD CONSTRAINT [PK_RegistrationType] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
