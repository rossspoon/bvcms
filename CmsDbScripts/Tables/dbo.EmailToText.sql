CREATE TABLE [dbo].[EmailToText]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Carrier] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[domain] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmailToText] ADD CONSTRAINT [PK_EmailToText] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
