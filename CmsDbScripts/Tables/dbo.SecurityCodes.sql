CREATE TABLE [dbo].[SecurityCodes]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Code] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DateUsed] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SecurityCodes] ADD CONSTRAINT [PK_SecurityCodes] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SecurityCodes] ON [dbo].[SecurityCodes] ([Id]) ON [PRIMARY]
GO
