CREATE TABLE [dbo].[Contribution]
(
[ContributionId] [int] NOT NULL IDENTITY(1, 1),
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[FundId] [int] NOT NULL,
[ContributionTypeId] [int] NOT NULL,
[PeopleId] [int] NULL,
[ContributionDate] [datetime] NULL,
[ContributionAmount] [numeric] (11, 2) NULL,
[ContributionDesc] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ContributionStatusId] [int] NULL,
[PledgeFlag] [bit] NOT NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL,
[PostingDate] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Contribution] ADD CONSTRAINT [CONTRIBUTION_PK] PRIMARY KEY NONCLUSTERED  ([ContributionId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CONTRIBUTION_DATE_IX] ON [dbo].[Contribution] ([ContributionDate]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_INDIVIDUAL_CONTRIBUTION_TBL_1] ON [dbo].[Contribution] ([ContributionStatusId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ContributionTypeId] ON [dbo].[Contribution] ([ContributionTypeId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CONTRIBUTION_FUND_FK_IX] ON [dbo].[Contribution] ([FundId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CONTRIBUTION_PEOPLE_FK_IX] ON [dbo].[Contribution] ([PeopleId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_INDIVIDUAL_CONTRIBUTION_TBL_2] ON [dbo].[Contribution] ([PledgeFlag]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Contribution] ADD CONSTRAINT [FK_Contribution_ContributionFund] FOREIGN KEY ([FundId]) REFERENCES [dbo].[ContributionFund] ([FundId])
GO
ALTER TABLE [dbo].[Contribution] ADD CONSTRAINT [FK_Contribution_ContributionStatus] FOREIGN KEY ([ContributionStatusId]) REFERENCES [lookup].[ContributionStatus] ([Id])
GO
ALTER TABLE [dbo].[Contribution] ADD CONSTRAINT [FK_Contribution_ContributionType] FOREIGN KEY ([ContributionTypeId]) REFERENCES [lookup].[ContributionType] ([Id])
GO
ALTER TABLE [dbo].[Contribution] ADD CONSTRAINT [FK_Contribution_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
