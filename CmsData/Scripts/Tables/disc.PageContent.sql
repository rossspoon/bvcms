CREATE TABLE [disc].[PageContent]
(
[PageID] [int] NOT NULL IDENTITY(1, 1),
[Title] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Body] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PageUrl] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_Page_createdOn] DEFAULT (getdate()),
[ModifiedOn] [datetime] NULL CONSTRAINT [DF_Page_modifiedOn] DEFAULT (getdate()),
[Deleted] [bit] NOT NULL CONSTRAINT [DF_CMS_Page_Deleted] DEFAULT ((0)),
[CreatedById] [int] NULL,
[ModifiedById] [int] NULL,
[cUserid] [int] NULL,
[cUserid2] [int] NULL
)
GO
ALTER TABLE [disc].[PageContent] ADD CONSTRAINT [PK_CMS_Page] PRIMARY KEY CLUSTERED  ([PageID])
GO
ALTER TABLE [disc].[PageContent] ADD CONSTRAINT [CreatedPages__CreatedBy] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [disc].[PageContent] ADD CONSTRAINT [ModifiedPages__ModifiedBy] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[Users] ([UserId])
GO
