CREATE TABLE [dbo].[words]
(
[word] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[n] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[words] ADD CONSTRAINT [PK_words_1] PRIMARY KEY CLUSTERED  ([word]) ON [PRIMARY]
GO
