CREATE TABLE [dbo].[Zips]
(
[ZipCode] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MetroMarginalCode] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Zips] ADD CONSTRAINT [PK_Zips] PRIMARY KEY CLUSTERED  ([ZipCode]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Zips] ADD CONSTRAINT [FK_Zips_ResidentCode] FOREIGN KEY ([MetroMarginalCode]) REFERENCES [lookup].[ResidentCode] ([Id])
GO
