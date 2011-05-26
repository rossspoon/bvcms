CREATE TABLE [dbo].[Tag]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TypeId] [int] NOT NULL CONSTRAINT [DF_Tag_TypeId] DEFAULT ((1)),
[Owner] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Active] [bit] NULL,
[PeopleId] [int] NULL,
[OwnerName] AS ([dbo].[UName]([PeopleId]))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tag] ADD CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Tag] ON [dbo].[Tag] ([TypeId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tag] WITH NOCHECK ADD CONSTRAINT [Tags__TagType] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[TagType] ([Id])
GO
ALTER TABLE [dbo].[Tag] ADD CONSTRAINT [TagsOwned__PersonOwner] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
