CREATE TABLE [dbo].[VolInterestInterestCodes]
(
[PeopleId] [int] NOT NULL,
[InterestCodeId] [int] NOT NULL
)

ALTER TABLE [dbo].[VolInterestInterestCodes] ADD 
CONSTRAINT [PK_VolInterestInterestCodes] PRIMARY KEY CLUSTERED  ([PeopleId], [InterestCodeId])
ALTER TABLE [dbo].[VolInterestInterestCodes] ADD
CONSTRAINT [FK_VolInterestInterestCodes_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO

ALTER TABLE [dbo].[VolInterestInterestCodes] ADD CONSTRAINT [FK_VolInterestInterestCodes_VolInterestCodes] FOREIGN KEY ([InterestCodeId]) REFERENCES [dbo].[VolInterestCodes] ([Id])
GO
