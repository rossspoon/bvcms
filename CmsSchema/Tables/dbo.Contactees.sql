CREATE TABLE [dbo].[Contactees]
(
[ContactId] [int] NOT NULL,
[PeopleId] [int] NOT NULL,
[ProfessionOfFaith] [bit] NULL,
[PrayedForPerson] [bit] NULL
)
ALTER TABLE [dbo].[Contactees] ADD
CONSTRAINT [contactsHad__person] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])

GO
ALTER TABLE [dbo].[Contactees] ADD CONSTRAINT [PK_Contactees] PRIMARY KEY CLUSTERED  ([ContactId], [PeopleId])
GO
ALTER TABLE [dbo].[Contactees] ADD CONSTRAINT [contactees__contact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[NewContact] ([ContactId])
GO
