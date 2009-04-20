CREATE TABLE [dbo].[Zips]
(
[ZipCode] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MetroMarginalCode] [int] NULL
)
GO
ALTER TABLE [dbo].[Zips] ADD CONSTRAINT [PK_Zips] PRIMARY KEY CLUSTERED ([ZipCode])
GO
