CREATE TABLE [dbo].[TaskList]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[CreatedBy] [int] NULL,
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TaskList] ADD CONSTRAINT [PK_TaskList] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
