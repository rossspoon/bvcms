CREATE TABLE [dbo].[BundleHeader]
(
[BundleHeaderId] [int] NOT NULL IDENTITY(1, 1),
[ChurchId] [int] NOT NULL,
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[RecordStatus] [bit] NOT NULL,
[BundleStatusId] [int] NOT NULL,
[ContributionDate] [datetime] NOT NULL,
[BundleHeaderTypeId] [int] NOT NULL,
[DepositDate] [datetime] NULL,
[BundleTotal] [numeric] (10, 2) NULL,
[TotalCash] [numeric] (10, 2) NULL,
[TotalChecks] [numeric] (10, 2) NULL,
[TotalEnvelopes] [numeric] (10, 2) NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL,
[FundId] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BundleHeader] ADD CONSTRAINT [BUNDLE_HEADER_PK] PRIMARY KEY NONCLUSTERED  ([BundleHeaderId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [BUNDLE_HEADER_CHURCH_FK_IX] ON [dbo].[BundleHeader] ([ChurchId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BundleHeader] ADD CONSTRAINT [BundleHeaders__Fund] FOREIGN KEY ([FundId]) REFERENCES [dbo].[ContributionFund] ([FundId])
GO
ALTER TABLE [dbo].[BundleHeader] ADD CONSTRAINT [FK_BUNDLE_HEADER_TBL_BundleHeaderTypes] FOREIGN KEY ([BundleHeaderTypeId]) REFERENCES [lookup].[BundleHeaderTypes] ([Id])
GO
ALTER TABLE [dbo].[BundleHeader] ADD CONSTRAINT [FK_BUNDLE_HEADER_TBL_BundleStatusTypes] FOREIGN KEY ([BundleStatusId]) REFERENCES [lookup].[BundleStatusTypes] ([Id])
GO
