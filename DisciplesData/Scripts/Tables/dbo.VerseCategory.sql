CREATE TABLE [dbo].[VerseCategory]
(
[id] [int] NOT NULL IDENTITY(1, 1) NOT FOR REPLICATION,
[Name] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ModifiedOn] [datetime] NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VerseCategory] ADD CONSTRAINT [PK_btea_VerseCategory_3D610094] PRIMARY KEY CLUSTERED  ([id]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_VerseCategory] ON [dbo].[VerseCategory] ([CreatedBy], [Name]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VerseCategory] ADD CONSTRAINT [FK_VerseCategory_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
