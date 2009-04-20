CREATE TABLE [lookup].[FamilyPosition]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[FamilyPosition] ADD CONSTRAINT [PK_FamilyPosition] PRIMARY KEY CLUSTERED ([Id])
GO
