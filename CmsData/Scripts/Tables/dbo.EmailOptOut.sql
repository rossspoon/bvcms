CREATE TABLE [dbo].[EmailOptOut]
(
[ToPeopleId] [int] NOT NULL,
[FromEmail] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
ALTER TABLE [dbo].[EmailOptOut] ADD CONSTRAINT [PK_EmailOptOut_1] PRIMARY KEY CLUSTERED  ([ToPeopleId], [FromEmail])
GO
ALTER TABLE [dbo].[EmailOptOut] ADD CONSTRAINT [FK_EmailOptOut_People] FOREIGN KEY ([ToPeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
