CREATE TABLE [disc].[Forum]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOn] [datetime] NULL,
[GroupId] [int] NULL,
[CreatedBy] [int] NULL,
[cUserid] [int] NULL
)
GO
ALTER TABLE [disc].[Forum] ADD CONSTRAINT [PK_Forum] PRIMARY KEY CLUSTERED  ([Id])
GO
ALTER TABLE [disc].[Forum] ADD CONSTRAINT [FK_Forum_Group] FOREIGN KEY ([GroupId]) REFERENCES [disc].[Group] ([Id])
GO
ALTER TABLE [disc].[Forum] ADD CONSTRAINT [FK_Forum_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
