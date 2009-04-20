CREATE TABLE [dbo].[TagShare]
(
[TagId] [int] NOT NULL,
[PeopleId] [int] NOT NULL
)
GO
ALTER TABLE [dbo].[TagShare] ADD CONSTRAINT [PK_TagShare] PRIMARY KEY CLUSTERED ([TagId], [PeopleId])
GO
ALTER TABLE [dbo].[TagShare] ADD CONSTRAINT [FK_TagShare_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[TagShare] ADD CONSTRAINT [FK_TagShare_Tag] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tag] ([Id])
GO
