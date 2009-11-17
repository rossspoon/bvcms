CREATE TABLE [dbo].[ZipCodes]
(
[zip] [char] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[state] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[City] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ZipCodes] ADD CONSTRAINT [PK_ZipCodes] PRIMARY KEY CLUSTERED  ([zip]) ON [PRIMARY]
GO
