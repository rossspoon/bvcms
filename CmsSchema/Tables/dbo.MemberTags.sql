CREATE TABLE [dbo].[MemberTags]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ProgId] [int] NULL
)
GO
ALTER TABLE [dbo].[MemberTags] ADD CONSTRAINT [PK_MemberTags] PRIMARY KEY CLUSTERED ([Id])
GO
ALTER TABLE [dbo].[MemberTags] ADD CONSTRAINT [FK_MemberTags_Program] FOREIGN KEY ([ProgId]) REFERENCES [dbo].[Program] ([Id])
GO
