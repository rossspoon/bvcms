CREATE TABLE [dbo].[Content]
(
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Title] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Body] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DateCreated] [datetime] NULL
)
GO
ALTER TABLE [dbo].[Content] ADD CONSTRAINT [PK_Content_1] PRIMARY KEY CLUSTERED ([Name])
GO
