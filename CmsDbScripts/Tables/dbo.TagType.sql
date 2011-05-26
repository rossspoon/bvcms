CREATE TABLE [dbo].[TagType]
(
[Id] [int] NOT NULL,
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TagType] ADD CONSTRAINT [PK_TagTypes] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
