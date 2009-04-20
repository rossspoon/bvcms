CREATE TABLE [dbo].[RelatedFamilies]
(
[FamilyId] [int] NOT NULL,
[RelatedFamilyId] [int] NOT NULL,
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[FamilyRelationshipDesc] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL
)
GO
ALTER TABLE [dbo].[RelatedFamilies] ADD CONSTRAINT [RELATED_FAMILIES_PK] PRIMARY KEY NONCLUSTERED ([FamilyId], [RelatedFamilyId])
GO
CREATE NONCLUSTERED INDEX [RELATED_FAMILIES_RELATED_FK_IX] ON [dbo].[RelatedFamilies] ([RelatedFamilyId])
GO
ALTER TABLE [dbo].[RelatedFamilies] WITH NOCHECK ADD CONSTRAINT [RelatedFamilies1__RelatedFamily1] FOREIGN KEY ([FamilyId]) REFERENCES [dbo].[Families] ([FamilyId])
GO
ALTER TABLE [dbo].[RelatedFamilies] WITH NOCHECK ADD CONSTRAINT [RelatedFamilies2__RelatedFamily2] FOREIGN KEY ([RelatedFamilyId]) REFERENCES [dbo].[Families] ([FamilyId])
GO
