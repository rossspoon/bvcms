CREATE TABLE [disc].[Verse]
(
[id] [int] NOT NULL IDENTITY(1, 1) NOT FOR REPLICATION,
[VerseRef] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VerseText] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Version] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Book] [int] NULL,
[Chapter] [int] NULL,
[VerseNum] [int] NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[cUserid] [int] NULL
)
GO
ALTER TABLE [disc].[Verse] ADD CONSTRAINT [PK_btea_Verse_1D7E4B61] PRIMARY KEY CLUSTERED  ([id])
GO
ALTER TABLE [disc].[Verse] ADD CONSTRAINT [FK_Verse_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
