CREATE TABLE [lookup].[BundleHeaderTypes]
(
[Id] [int] NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[BundleHeaderTypes] ADD CONSTRAINT [PK_BundleHeaderTypes] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
