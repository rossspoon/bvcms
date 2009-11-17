CREATE TABLE [dbo].[VolInterestCodes]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[OpportunityId] [int] NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VolInterestCodes] ADD CONSTRAINT [PK_VolInterestCodes] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VolInterestCodes] ADD CONSTRAINT [FK_VolInterestCodes_VolOpportunity] FOREIGN KEY ([OpportunityId]) REFERENCES [dbo].[VolOpportunity] ([Id])
GO
