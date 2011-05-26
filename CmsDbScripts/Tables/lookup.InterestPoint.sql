CREATE TABLE [lookup].[InterestPoint]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[InterestPoint] ADD CONSTRAINT [PK_InterestPoint] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
