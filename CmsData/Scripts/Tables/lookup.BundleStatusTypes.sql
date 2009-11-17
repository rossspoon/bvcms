CREATE TABLE [lookup].[BundleStatusTypes]
(
[Id] [int] NOT NULL,
[Code] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[BundleStatusTypes] ADD CONSTRAINT [PK_BundleStatusTypes] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
