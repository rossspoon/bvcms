CREATE TABLE [dbo].[SaleItem]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Price] [money] NOT NULL,
[Available] [bit] NULL,
[URL] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MaxItems] [int] NULL,
[DefaultItems] [int] NULL,
[Email] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SaleItem] ADD CONSTRAINT [PK_SaleItem] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
