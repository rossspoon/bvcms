CREATE TABLE [lookup].[NameTitle]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[NameTitle] ADD CONSTRAINT [PK_NameTitle] PRIMARY KEY CLUSTERED ([Id])
GO
