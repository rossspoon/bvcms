CREATE TABLE [dbo].[Division]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ProgId] [int] NULL,
[SortOrder] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Division] ADD CONSTRAINT [PK_Division] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Division] ADD CONSTRAINT [FK_Division_Program] FOREIGN KEY ([ProgId]) REFERENCES [dbo].[Program] ([Id])
GO
