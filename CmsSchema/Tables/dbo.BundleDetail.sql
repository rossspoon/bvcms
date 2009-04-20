CREATE TABLE [dbo].[BundleDetail]
(
[BundleDetailId] [int] NOT NULL IDENTITY(1, 1),
[BundleHeaderId] [int] NOT NULL,
[ContributionId] [int] NOT NULL,
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL
)
ALTER TABLE [dbo].[BundleDetail] WITH NOCHECK ADD
CONSTRAINT [BUNDLE_DETAIL_BUNDLE_FK] FOREIGN KEY ([BundleHeaderId]) REFERENCES [dbo].[BundleHeader] ([BundleHeaderId])
GO
ALTER TABLE [dbo].[BundleDetail] ADD CONSTRAINT [BUNDLE_DETAIL_PK] PRIMARY KEY NONCLUSTERED ([BundleDetailId])
GO
CREATE NONCLUSTERED INDEX [BUNDLE_DETAIL_BUNDLE_FK_IX] ON [dbo].[BundleDetail] ([BundleHeaderId])
GO
CREATE NONCLUSTERED INDEX [BUNDLE_DETAIL_CONTR_FK_IX] ON [dbo].[BundleDetail] ([ContributionId])
GO

ALTER TABLE [dbo].[BundleDetail] WITH NOCHECK ADD CONSTRAINT [BUNDLE_DETAIL_CONTR_FK] FOREIGN KEY ([ContributionId]) REFERENCES [dbo].[Contribution] ([ContributionId])
GO
