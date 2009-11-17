CREATE TABLE [dbo].[LoveRespect]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Created] [datetime] NOT NULL,
[HimId] [int] NULL,
[HisEmail] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HisEmailPreferred] [bit] NULL,
[HerId] [int] NULL,
[HerEmail] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HerEmailPreferred] [bit] NULL,
[OrgId] [int] NULL,
[Relationship] [int] NULL,
[PreferNight] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LoveRespect] ADD CONSTRAINT [PK_LoveRespect] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LoveRespect] ADD CONSTRAINT [FK_LoveRespect_Organizations] FOREIGN KEY ([OrgId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[LoveRespect] ADD CONSTRAINT [HerLoveRespects__Her] FOREIGN KEY ([HerId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[LoveRespect] ADD CONSTRAINT [HisLoveRespects__Him] FOREIGN KEY ([HimId]) REFERENCES [dbo].[People] ([PeopleId])
GO
