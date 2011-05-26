CREATE TABLE [dbo].[OneTimeLinks]
(
[Id] [uniqueidentifier] NOT NULL,
[querystring] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[used] [bit] NOT NULL CONSTRAINT [DF_OneTimeLinks_used] DEFAULT ((0)),
[expires] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OneTimeLinks] ADD CONSTRAINT [PK_OneTimeLinks] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
