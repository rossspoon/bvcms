CREATE TABLE [lookup].[MemberStatus]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[MemberStatus] ADD CONSTRAINT [PK_MemberStatus_1] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
