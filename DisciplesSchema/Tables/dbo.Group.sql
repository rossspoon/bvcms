CREATE TABLE [dbo].[Group]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ContentId] [int] NULL
)

GO
ALTER TABLE [dbo].[Group] ADD CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED ([Id])
GO
ALTER TABLE [dbo].[Group] ADD CONSTRAINT [Groups__WelcomeText] FOREIGN KEY ([ContentId]) REFERENCES [dbo].[Content] ([ContentID])
GO
EXEC sp_addextendedproperty N'MS_Description', N'Groups->WelcomeText', 'SCHEMA', N'dbo', 'TABLE', N'Group', 'CONSTRAINT', N'Groups__WelcomeText'
GO
