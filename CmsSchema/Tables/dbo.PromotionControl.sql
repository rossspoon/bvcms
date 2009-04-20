CREATE TABLE [dbo].[PromotionControl]
(
[PromotionId] [int] NOT NULL,
[PromotionControlId] [int] NOT NULL,
[ScheduleId] [int] NOT NULL,
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[RecordStatus] [bit] NOT NULL,
[DivisionId] [int] NOT NULL,
[GenderControlId] [int] NOT NULL,
[TeacherControl] [bit] NOT NULL,
[MixControl] [bit] NOT NULL,
[NbrOfClasses] [int] NOT NULL,
[NbrOfFemaleClasses] [int] NOT NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL,
[OldDiisionId] [int] NULL
)
GO
ALTER TABLE [dbo].[PromotionControl] ADD CONSTRAINT [PROMOTION_CONTROL_PK] PRIMARY KEY NONCLUSTERED ([PromotionId], [PromotionControlId], [ScheduleId])
GO
ALTER TABLE [dbo].[PromotionControl] WITH NOCHECK ADD CONSTRAINT [PROMOTION_CONTROL_PROMO_FK] FOREIGN KEY ([PromotionId]) REFERENCES [dbo].[Promotions] ([PromotionId])
GO
