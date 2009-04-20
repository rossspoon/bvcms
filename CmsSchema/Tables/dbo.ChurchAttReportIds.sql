CREATE TABLE [dbo].[ChurchAttReportIds]
(
[Name] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Id] [int] NOT NULL
)
GO
ALTER TABLE [dbo].[ChurchAttReportIds] ADD CONSTRAINT [PK_ChurchAttReportIds] PRIMARY KEY CLUSTERED ([Name])
GO
