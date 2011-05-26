CREATE TABLE [dbo].[Content]
(
[Name] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Title] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Body] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DateCreated] [datetime] NULL,
[Id] [int] NOT NULL IDENTITY(1, 1)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Content] ADD CONSTRAINT [PK_Content_1] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Content] ON [dbo].[Content] ([Name]) ON [PRIMARY]
GO
