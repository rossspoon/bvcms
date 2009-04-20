CREATE TABLE [dbo].[BFCSummaryOrgTags]
(
[SortOrder] [int] NOT NULL,
[OrgTagId] [int] NOT NULL
)
ALTER TABLE [dbo].[BFCSummaryOrgTags] ADD 
CONSTRAINT [PK_BFCSummaryOrgTags_1] PRIMARY KEY CLUSTERED ([OrgTagId])
ALTER TABLE [dbo].[BFCSummaryOrgTags] ADD
CONSTRAINT [FK_BFCSummaryOrgTags_Division] FOREIGN KEY ([OrgTagId]) REFERENCES [dbo].[Division] ([Id])
GO

ALTER TABLE [dbo].[BFCSummaryOrgTags] ADD CONSTRAINT [FK_BFCSummaryOrgTags_Tag] FOREIGN KEY ([OrgTagId]) REFERENCES [dbo].[Tag] ([Id])
GO
