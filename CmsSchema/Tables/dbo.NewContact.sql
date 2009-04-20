CREATE TABLE [dbo].[NewContact]
(
[ContactId] [int] NOT NULL IDENTITY(200000, 1),
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ContactTypeId] [int] NOT NULL,
[ContactDate] [datetime] NOT NULL,
[ContactReasonId] [int] NOT NULL,
[MinistryId] [int] NULL,
[NotAtHome] [bit] NULL,
[LeftDoorHanger] [bit] NULL,
[LeftMessage] [bit] NULL,
[GospelShared] [bit] NULL,
[PrayerRequest] [bit] NULL,
[ContactMade] [bit] NULL,
[GiftBagGiven] [bit] NULL,
[Comments] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL
)
ALTER TABLE [dbo].[NewContact] ADD
CONSTRAINT [FK_Contacts_Ministries] FOREIGN KEY ([MinistryId]) REFERENCES [dbo].[Ministries] ([MinistryId])

ALTER TABLE [dbo].[NewContact] ADD
CONSTRAINT [FK_Contacts_ContactTypes] FOREIGN KEY ([ContactTypeId]) REFERENCES [lookup].[NewContactType] ([Id])
ALTER TABLE [dbo].[NewContact] ADD
CONSTRAINT [FK_NewContacts_ContactReasons] FOREIGN KEY ([ContactReasonId]) REFERENCES [lookup].[NewContactReason] ([Id])




GO
ALTER TABLE [dbo].[NewContact] ADD CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED  ([ContactId])
GO
