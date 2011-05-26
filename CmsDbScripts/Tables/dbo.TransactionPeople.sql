CREATE TABLE [dbo].[TransactionPeople]
(
[Id] [int] NOT NULL,
[PeopleId] [int] NOT NULL,
[Amt] [money] NULL,
[OrgId] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TransactionPeople] ADD CONSTRAINT [PK_TransactionPeople] PRIMARY KEY CLUSTERED  ([Id], [PeopleId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TransactionPeople] ADD CONSTRAINT [FK_TransactionPeople_Person] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[TransactionPeople] ADD CONSTRAINT [FK_TransactionPeople_Transaction] FOREIGN KEY ([Id]) REFERENCES [dbo].[Transaction] ([Id])
GO
