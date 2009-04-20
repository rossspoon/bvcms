CREATE TABLE [dbo].[Audits]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Action] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TableName] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TableKey] [int] NULL,
[UserName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AuditDate] [smalldatetime] NOT NULL
)

GO
ALTER TABLE [dbo].[Audits] ADD CONSTRAINT [PK_Audits] PRIMARY KEY CLUSTERED  ([Id])
GO
