CREATE TABLE [dbo].[TagOrg]
(
[Id] [int] NOT NULL,
[OrganizationId] [int] NOT NULL
)
ALTER TABLE [dbo].[TagOrg] WITH NOCHECK ADD
CONSTRAINT [Tags__Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])

GO
ALTER TABLE [dbo].[TagOrg] ADD CONSTRAINT [PK_TagOrg] PRIMARY KEY CLUSTERED  ([Id], [OrganizationId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_TagOrg] ON [dbo].[TagOrg] ([OrganizationId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TagOrg] WITH NOCHECK ADD CONSTRAINT [OrgTags__Tag] FOREIGN KEY ([Id]) REFERENCES [dbo].[Tag] ([Id])
GO
