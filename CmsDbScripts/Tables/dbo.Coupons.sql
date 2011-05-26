CREATE TABLE [dbo].[Coupons]
(
[Id] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Created] [datetime] NOT NULL CONSTRAINT [DF_Coupons_Created] DEFAULT (getdate()),
[Used] [datetime] NULL,
[Canceled] [datetime] NULL,
[Amount] [money] NULL,
[DivId] [int] NULL,
[OrgId] [int] NULL,
[PeopleId] [int] NULL,
[Name] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UserId] [int] NULL,
[RegAmount] [money] NULL,
[DivOrg] AS (case when [divid] IS NOT NULL then 'div.'+CONVERT([varchar],[divid],(0)) else 'org.'+CONVERT([varchar],[orgid],(0)) end)
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Coupons] ADD CONSTRAINT [PK_Coupons] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Coupons] ON [dbo].[Coupons] ([Created] DESC) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Coupons_1] ON [dbo].[Coupons] ([DivId], [OrgId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Coupons] ADD CONSTRAINT [FK_Coupons_Division] FOREIGN KEY ([DivId]) REFERENCES [dbo].[Division] ([Id])
GO
ALTER TABLE [dbo].[Coupons] ADD CONSTRAINT [FK_Coupons_Organizations] FOREIGN KEY ([OrgId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Coupons] ADD CONSTRAINT [FK_Coupons_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Coupons] ADD CONSTRAINT [FK_Coupons_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
