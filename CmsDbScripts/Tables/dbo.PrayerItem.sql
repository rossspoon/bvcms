CREATE TABLE [dbo].[PrayerItem]
(
[Id] [int] NOT NULL,
[PeopleId] [int] NULL,
[Text] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Type] [int] NULL,
[Link] [int] NULL,
[Submitted] [datetime] NULL,
[Approved] [int] NULL,
[TimesPrayedFor] [int] NULL,
[Visibility] [int] NULL,
[Expires] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrayerItem] ADD CONSTRAINT [PK_PrayerItem] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
