CREATE TABLE [disc].[OtherNotify]
(
[Id] [int] NOT NULL,
[email] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BlogId] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [disc].[OtherNotify] ADD CONSTRAINT [PK_OtherNotify] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_OtherNotify] ON [disc].[OtherNotify] ([BlogId]) ON [PRIMARY]
GO
ALTER TABLE [disc].[OtherNotify] ADD CONSTRAINT [FK_OtherNotify_Blog] FOREIGN KEY ([BlogId]) REFERENCES [disc].[Blog] ([Id])
GO
