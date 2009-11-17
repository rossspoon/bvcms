CREATE TABLE [lookup].[AttendTrackLevel]
(
[Id] [int] NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[AttendTrackLevel] ADD CONSTRAINT [PK_AttendTrackLevel] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
