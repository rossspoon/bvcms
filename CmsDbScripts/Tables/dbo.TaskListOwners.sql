CREATE TABLE [dbo].[TaskListOwners]
(
[TaskListId] [int] NOT NULL,
[PeopleId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TaskListOwners] ADD CONSTRAINT [PK_TaskListOwners] PRIMARY KEY CLUSTERED  ([TaskListId], [PeopleId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TaskListOwners] ADD CONSTRAINT [FK_TaskListOwners_PEOPLE_TBL] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[TaskListOwners] ADD CONSTRAINT [FK_TaskListOwners_TaskList] FOREIGN KEY ([TaskListId]) REFERENCES [dbo].[TaskList] ([Id])
GO
