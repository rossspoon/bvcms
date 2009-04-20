CREATE TABLE [lookup].[MemberLetterStatus]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[MemberLetterStatus] ADD CONSTRAINT [PK_MemberLetterStatus] PRIMARY KEY CLUSTERED ([Id])
GO
