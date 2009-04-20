CREATE TABLE [dbo].[TemporaryToken]
(
[id] [uniqueidentifier] NOT NULL,
[expired] [bit] NOT NULL CONSTRAINT [DF_TemporaryTokens_expired] DEFAULT ((0)),
[CreatedOn] [datetime] NOT NULL,
[CreatedBy] [int] NULL
)

GO
ALTER TABLE [dbo].[TemporaryToken] ADD CONSTRAINT [PK_TemporaryTokens] PRIMARY KEY CLUSTERED ([id])
GO
ALTER TABLE [dbo].[TemporaryToken] ADD CONSTRAINT [FK_TemporaryToken_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
