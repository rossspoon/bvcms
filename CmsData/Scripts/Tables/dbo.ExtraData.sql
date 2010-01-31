CREATE TABLE [dbo].[ExtraData]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Data] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Stamp] [datetime] NULL
)
GO
ALTER TABLE [dbo].[ExtraData] ADD CONSTRAINT [PK_ExtraData] PRIMARY KEY CLUSTERED  ([Id])
GO
