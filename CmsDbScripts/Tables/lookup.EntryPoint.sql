CREATE TABLE [lookup].[EntryPoint]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[EntryPoint] ADD CONSTRAINT [PK_EntryPoint] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
