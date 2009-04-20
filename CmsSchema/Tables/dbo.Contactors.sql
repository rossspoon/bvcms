CREATE TABLE [dbo].[Contactors]
(
[ContactId] [int] NOT NULL,
[PeopleId] [int] NOT NULL
)
ALTER TABLE [dbo].[Contactors] ADD
CONSTRAINT [contactsMade__person] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])

GO
ALTER TABLE [dbo].[Contactors] ADD CONSTRAINT [PK_Contactors] PRIMARY KEY CLUSTERED  ([ContactId], [PeopleId])
GO

ALTER TABLE [dbo].[Contactors] ADD CONSTRAINT [contactsMakers__contact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[NewContact] ([ContactId])
GO
