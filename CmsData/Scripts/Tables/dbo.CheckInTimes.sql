CREATE TABLE [dbo].[CheckInTimes]
(
[Id] [int] NOT NULL,
[PeopleId] [int] NULL,
[OrganizationId] [int] NULL,
[CheckInDay] [datetime] NULL,
[CheckInTime] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CheckInTimes] ADD CONSTRAINT [PK_CheckInTimes] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CheckInTimes] ON [dbo].[CheckInTimes] ([CheckInDay]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CheckInTimes_1] ON [dbo].[CheckInTimes] ([CheckInTime]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CheckInTimes] ADD CONSTRAINT [FK_CheckInTimes_Organizations] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[CheckInTimes] ADD CONSTRAINT [FK_CheckInTimes_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
