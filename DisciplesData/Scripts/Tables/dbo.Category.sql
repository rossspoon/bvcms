CREATE TABLE [dbo].[Category]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Category] ADD CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
