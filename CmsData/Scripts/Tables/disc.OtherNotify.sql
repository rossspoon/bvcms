CREATE TABLE [disc].[OtherNotify]
(
[Id] [int] NOT NULL,
[email] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BlogId] [int] NULL
)
GO
ALTER TABLE [disc].[OtherNotify] ADD CONSTRAINT [PK_OtherNotify] PRIMARY KEY CLUSTERED  ([Id])
GO
CREATE NONCLUSTERED INDEX [IX_OtherNotify] ON [disc].[OtherNotify] ([BlogId])
GO
ALTER TABLE [disc].[OtherNotify] ADD CONSTRAINT [FK_OtherNotify_Blog] FOREIGN KEY ([BlogId]) REFERENCES [disc].[Blog] ([Id])
GO
