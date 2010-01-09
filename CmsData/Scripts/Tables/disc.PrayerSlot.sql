CREATE TABLE [disc].[PrayerSlot]
(
[Day] [int] NOT NULL,
[Time] [datetime] NOT NULL,
[PeopleId] [int] NOT NULL
)
GO
ALTER TABLE [disc].[PrayerSlot] ADD CONSTRAINT [PK_PrayerSlot_1] PRIMARY KEY CLUSTERED  ([Day], [Time], [PeopleId])
GO
ALTER TABLE [disc].[PrayerSlot] ADD CONSTRAINT [FK_PrayerSlot_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
