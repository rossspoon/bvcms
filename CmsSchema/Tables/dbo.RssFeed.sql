CREATE TABLE [dbo].[RssFeed]
(
[Url] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Data] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ETag] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LastModified] [datetime] NULL
)
GO
ALTER TABLE [dbo].[RssFeed] ADD CONSTRAINT [PK_RssFeed] PRIMARY KEY CLUSTERED ([Url])
GO
