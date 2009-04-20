CREATE TABLE [dbo].[Promotions]
(
[PromotionId] [int] NOT NULL IDENTITY(1, 1),
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[RecordStatus] [bit] NOT NULL,
[ChurchId] [int] NOT NULL,
[PromotionDate] [datetime] NULL,
[OpenedDate] [datetime] NULL,
[CompletedDate] [datetime] NULL,
[PromotionName] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PromotionDescription] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL
)
GO
ALTER TABLE [dbo].[Promotions] ADD CONSTRAINT [PROMOTIONS_PK] PRIMARY KEY NONCLUSTERED ([PromotionId])
GO
