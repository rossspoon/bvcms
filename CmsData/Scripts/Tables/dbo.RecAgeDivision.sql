CREATE TABLE [dbo].[RecAgeDivision]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[DivId] [int] NULL,
[OrgId] [int] NULL,
[StartAge] [int] NULL,
[EndAge] [int] NULL,
[AgeDate] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GenderId] [int] NULL,
[Fee] [money] NULL,
[ExtraFee] [money] NULL,
[ExpirationDt] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RecAgeDivision] ADD CONSTRAINT [PK_Recreation] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RecAgeDivision] ADD CONSTRAINT [FK_Recreation_Division] FOREIGN KEY ([DivId]) REFERENCES [dbo].[Division] ([Id])
GO
ALTER TABLE [dbo].[RecAgeDivision] ADD CONSTRAINT [FK_Recreation_Gender] FOREIGN KEY ([GenderId]) REFERENCES [lookup].[Gender] ([Id])
GO
ALTER TABLE [dbo].[RecAgeDivision] ADD CONSTRAINT [FK_Recreation_Organizations] FOREIGN KEY ([OrgId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
