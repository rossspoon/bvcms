CREATE TABLE [dbo].[VolInterestCodes]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Description] [varchar] (180) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Code] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Org] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VolInterestCodes] ADD CONSTRAINT [PK_VolInterestCodes] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
