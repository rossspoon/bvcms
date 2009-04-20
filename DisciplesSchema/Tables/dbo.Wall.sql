CREATE TABLE [dbo].[Wall]
(
[Id] [int] NOT NULL,
[UserId] [int] NULL,
[Text] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOn] [datetime] NULL,
[PosterId] [int] NULL
)

GO
ALTER TABLE [dbo].[Wall] ADD CONSTRAINT [PK_Wall] PRIMARY KEY CLUSTERED ([Id])
GO
CREATE NONCLUSTERED INDEX [IX_Wall] ON [dbo].[Wall] ([UserId], [CreatedOn])
GO
