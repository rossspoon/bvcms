CREATE TABLE [dbo].[SaleTransaction]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[SaleDate] [datetime] NOT NULL,
[Amount] [money] NOT NULL,
[TransactionId] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PeopleId] [int] NOT NULL,
[Username] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Password] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ItemId] [int] NOT NULL,
[Quantity] [int] NOT NULL,
[EmailAddress] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ItemDescription] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SaleTransaction] ADD CONSTRAINT [PK_SaleTransaction] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SaleTransaction] ADD CONSTRAINT [FK_SaleTransaction_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[SaleTransaction] ADD CONSTRAINT [FK_SaleTransaction_SaleItem] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[SaleItem] ([Id])
GO
