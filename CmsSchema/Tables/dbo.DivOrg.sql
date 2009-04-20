CREATE TABLE [dbo].[DivOrg]
(
[DivId] [int] NOT NULL,
[OrgId] [int] NOT NULL
)
GO
ALTER TABLE [dbo].[DivOrg] ADD CONSTRAINT [PK_DivOrg] PRIMARY KEY CLUSTERED ([DivId], [OrgId])
GO
ALTER TABLE [dbo].[DivOrg] ADD CONSTRAINT [FK_DivOrg_Division] FOREIGN KEY ([DivId]) REFERENCES [dbo].[Division] ([Id])
GO
ALTER TABLE [dbo].[DivOrg] ADD CONSTRAINT [FK_DivOrg_Organizations] FOREIGN KEY ([OrgId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
