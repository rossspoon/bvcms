CREATE TABLE [dbo].[Preferences]
(
[UserId] [int] NOT NULL,
[Preference] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Value] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Preferences] ADD CONSTRAINT [PK_UserPreferences] PRIMARY KEY CLUSTERED  ([UserId], [Preference]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Preferences] ADD CONSTRAINT [FK_UserPreferences_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
GO
