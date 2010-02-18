CREATE TABLE [dbo].[RecLeague]
(
[DivId] [int] NOT NULL,
[AgeDate] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ExtraFee] [money] NULL,
[ExpirationDt] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShirtFee] [money] NULL,
[EmailMessage] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailSubject] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailAddresses] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [dbo].[RecLeague] ADD CONSTRAINT [PK_RecLeague] PRIMARY KEY CLUSTERED  ([DivId])
GO
ALTER TABLE [dbo].[RecLeague] ADD CONSTRAINT [FK_RecLeague_Division] FOREIGN KEY ([DivId]) REFERENCES [dbo].[Division] ([Id])
GO
