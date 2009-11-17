CREATE TABLE [dbo].[OtherNotify]
(
[Id] [int] NOT NULL,
[email] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BlogId] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OtherNotify] ADD CONSTRAINT [PK_OtherNotify] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_OtherNotify] ON [dbo].[OtherNotify] ([BlogId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OtherNotify] WITH NOCHECK ADD CONSTRAINT [FK_OtherNotify_Blog] FOREIGN KEY ([BlogId]) REFERENCES [dbo].[Blog] ([Id])
GO
