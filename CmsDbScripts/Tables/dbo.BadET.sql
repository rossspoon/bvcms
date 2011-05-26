CREATE TABLE [dbo].[BadET]
(
[id] [int] NOT NULL IDENTITY(1, 1),
[PeopleId] [int] NOT NULL,
[OrgId] [int] NULL,
[TranId] [int] NOT NULL,
[Flag] [int] NOT NULL,
[Status] [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BadET] ADD CONSTRAINT [PK_BadET] PRIMARY KEY CLUSTERED  ([id]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BadET] ON [dbo].[BadET] ([PeopleId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BadET_1] ON [dbo].[BadET] ([TranId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BadET] ADD CONSTRAINT [FK_BadET_Organizations] FOREIGN KEY ([OrgId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
