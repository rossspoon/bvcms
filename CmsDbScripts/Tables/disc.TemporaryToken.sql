CREATE TABLE [disc].[TemporaryToken]
(
[id] [uniqueidentifier] NOT NULL,
[expired] [bit] NOT NULL CONSTRAINT [DF_TemporaryTokens_expired] DEFAULT ((0)),
[CreatedOn] [datetime] NOT NULL,
[CreatedBy] [int] NULL,
[cUserid] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [disc].[TemporaryToken] ADD CONSTRAINT [PK_TemporaryTokens] PRIMARY KEY CLUSTERED  ([id]) ON [PRIMARY]
GO
ALTER TABLE [disc].[TemporaryToken] ADD CONSTRAINT [FK_TemporaryToken_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId])
GO
