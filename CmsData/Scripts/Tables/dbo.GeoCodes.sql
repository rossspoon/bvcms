CREATE TABLE [dbo].[GeoCodes]
(
[Address] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Latitude] [float] NOT NULL CONSTRAINT [DF_GeoCodes_Latitude] DEFAULT ((0)),
[Longitude] [float] NOT NULL CONSTRAINT [DF_GeoCodes_Longitude] DEFAULT ((0))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GeoCodes] ADD CONSTRAINT [PK_GeoCodes_1] PRIMARY KEY CLUSTERED  ([Address]) ON [PRIMARY]
GO
