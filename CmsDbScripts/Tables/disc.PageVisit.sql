CREATE TABLE [disc].[PageVisit]
(
[CreatedOn] [datetime] NOT NULL,
[PageTitle] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Id] [int] NOT NULL IDENTITY(1, 1),
[PageUrl] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VisitTime] [datetime] NULL,
[UserId] [int] NULL,
[cUserid] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [disc].[PageVisit] ADD CONSTRAINT [PK_PageVisit] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_VisitTime] ON [disc].[PageVisit] ([VisitTime]) ON [PRIMARY]
GO
ALTER TABLE [disc].[PageVisit] ADD CONSTRAINT [FK_PageVisit_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
