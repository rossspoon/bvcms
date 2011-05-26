CREATE TABLE [dbo].[StreetTypes]
(
[Type] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StreetTypes] ADD CONSTRAINT [PK_StreetTypes] PRIMARY KEY CLUSTERED  ([Type]) ON [PRIMARY]
GO
