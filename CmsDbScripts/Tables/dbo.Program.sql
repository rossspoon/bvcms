CREATE TABLE [dbo].[Program]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RptGroup] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StartHoursOffset] [real] NULL,
[EndHoursOffset] [real] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Program] ADD CONSTRAINT [PK_Program] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
