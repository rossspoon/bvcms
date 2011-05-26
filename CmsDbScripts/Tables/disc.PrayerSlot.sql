CREATE TABLE [disc].[PrayerSlot]
(
[Day] [int] NOT NULL,
[Time] [datetime] NOT NULL,
[PeopleId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [disc].[PrayerSlot] ADD CONSTRAINT [PK_PrayerSlot_1] PRIMARY KEY CLUSTERED  ([Day], [Time], [PeopleId]) ON [PRIMARY]
GO
ALTER TABLE [disc].[PrayerSlot] ADD CONSTRAINT [FK_PrayerSlot_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
