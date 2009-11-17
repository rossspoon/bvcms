CREATE TABLE [dbo].[VolInterestInterestCodes]
(
[VolInterestId] [int] NOT NULL,
[InterestCodeId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VolInterestInterestCodes] ADD CONSTRAINT [PK_VolInterestInterestCodes] PRIMARY KEY CLUSTERED  ([VolInterestId], [InterestCodeId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VolInterestInterestCodes] ADD CONSTRAINT [FK_VolInterestInterestCodes_VolInterest] FOREIGN KEY ([VolInterestId]) REFERENCES [dbo].[VolInterest] ([Id])
GO
ALTER TABLE [dbo].[VolInterestInterestCodes] ADD CONSTRAINT [FK_VolInterestInterestCodes_VolInterestCodes] FOREIGN KEY ([InterestCodeId]) REFERENCES [dbo].[VolInterestCodes] ([Id])
GO
