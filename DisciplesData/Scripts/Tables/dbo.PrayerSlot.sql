CREATE TABLE [dbo].[PrayerSlot]
(
[Day] [int] NOT NULL,
[Time] [datetime] NOT NULL,
[UserId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrayerSlot] ADD CONSTRAINT [PK_PrayerSlot] PRIMARY KEY CLUSTERED  ([Day], [Time], [UserId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrayerSlot] ADD CONSTRAINT [FK_PrayerSlot_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
