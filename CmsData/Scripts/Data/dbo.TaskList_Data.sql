SET IDENTITY_INSERT [dbo].[TaskList] ON
INSERT INTO [dbo].[TaskList] ([Id], [CreatedBy], [Name]) VALUES (1, 0, 'InBox')
INSERT INTO [dbo].[TaskList] ([Id], [CreatedBy], [Name]) VALUES (2, 0, 'Personal')
SET IDENTITY_INSERT [dbo].[TaskList] OFF
