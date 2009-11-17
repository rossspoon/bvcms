SET IDENTITY_INSERT [dbo].[Tag] ON
INSERT INTO [dbo].[Tag] ([Id], [Name], [TypeId], [Owner], [Active], [PeopleId]) VALUES (1, 'TrackBirthdays', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Tag] ([Id], [Name], [TypeId], [Owner], [Active], [PeopleId]) VALUES (2, 'UnNamed', 1, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Tag] OFF
