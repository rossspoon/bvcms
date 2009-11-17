CREATE TABLE [dbo].[VolInterest]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[PeopleId] [int] NULL,
[ImgId] [int] NULL,
[IsDocument] [bit] NULL,
[Created] [datetime] NULL,
[OpportunityCode] [int] NULL,
[question] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VolInterest] ADD CONSTRAINT [PK_VolInterest] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VolInterest] ADD CONSTRAINT [FK_VolInterest_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[VolInterest] ADD CONSTRAINT [FK_VolInterest_VolOpportunity] FOREIGN KEY ([OpportunityCode]) REFERENCES [dbo].[VolOpportunity] ([Id])
GO
