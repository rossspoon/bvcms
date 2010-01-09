CREATE TABLE [dbo].[VolInterestCodes]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Description] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Code] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Org] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)

GO
ALTER TABLE [dbo].[VolInterestCodes] ADD CONSTRAINT [PK_VolInterestCodes] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
