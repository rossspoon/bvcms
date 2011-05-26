CREATE TABLE [dbo].[CardIdentifiers]
(
[Id] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PeopleId] [int] NULL,
[CreatedOn] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CardIdentifiers] ADD CONSTRAINT [PK_CardIdentifiers] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CardIdentifiers] ADD CONSTRAINT [FK_CardIdentifiers_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
