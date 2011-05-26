CREATE TABLE [dbo].[ProgDiv]
(
[ProgId] [int] NOT NULL,
[DivId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ProgDiv] ADD CONSTRAINT [PK_ProgDiv] PRIMARY KEY CLUSTERED  ([ProgId], [DivId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ProgDiv] ADD CONSTRAINT [FK_ProgDiv_Division] FOREIGN KEY ([DivId]) REFERENCES [dbo].[Division] ([Id])
GO
ALTER TABLE [dbo].[ProgDiv] ADD CONSTRAINT [FK_ProgDiv_Program] FOREIGN KEY ([ProgId]) REFERENCES [dbo].[Program] ([Id])
GO
