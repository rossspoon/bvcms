CREATE TABLE [dbo].[AuditValues]
(
[AuditId] [int] NOT NULL,
[MemberName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OldValue] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NewValue] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)

GO
ALTER TABLE [dbo].[AuditValues] ADD CONSTRAINT [PK_AuditValues] PRIMARY KEY CLUSTERED  ([AuditId], [MemberName])
GO
ALTER TABLE [dbo].[AuditValues] ADD CONSTRAINT [FK_AuditValues_Audits] FOREIGN KEY ([AuditId]) REFERENCES [dbo].[Audits] ([Id])
GO
