CREATE TABLE [disc].[Group]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ContentId] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [disc].[Group] ADD CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [disc].[Group] ADD CONSTRAINT [Groups__WelcomeText] FOREIGN KEY ([ContentId]) REFERENCES [disc].[ParaContent] ([ContentID])
GO
