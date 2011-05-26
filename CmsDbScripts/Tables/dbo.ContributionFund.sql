CREATE TABLE [dbo].[ContributionFund]
(
[FundId] [int] NOT NULL,
[ChurchId] [int] NOT NULL,
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[RecordStatus] [bit] NOT NULL,
[FundName] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FundDescription] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FundStatusId] [int] NOT NULL,
[FundTypeId] [int] NOT NULL,
[FundPledgeFlag] [bit] NOT NULL,
[FundTarget] [numeric] (11, 2) NULL,
[FundOpenDate] [datetime] NOT NULL,
[FundCloseDate] [datetime] NULL,
[FundReopenDate] [datetime] NULL,
[FundDropDate] [datetime] NULL,
[FundAccountCode] [int] NULL,
[FundIncomeDept] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FundIncomeAccount] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FundIncomeFund] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FundCashDept] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FundCashAccount] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FundCashFund] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ContributionFund] ADD CONSTRAINT [FUND_PK] PRIMARY KEY NONCLUSTERED  ([FundId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [FUND_CHURCH_FK_IX] ON [dbo].[ContributionFund] ([ChurchId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [FUND_NAME_IX] ON [dbo].[ContributionFund] ([FundName]) ON [PRIMARY]
GO
