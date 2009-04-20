CREATE TABLE [dbo].[OrganizationMembers]
(
[OrganizationId] [int] NOT NULL,
[PeopleId] [int] NOT NULL,
[CreatedBy] [int] NULL,
[CreatedDate] [datetime] NULL,
[RecordStatus] [bit] NOT NULL CONSTRAINT [DF_ORGANIZATION_MEMBERS_TBL_RECORD_STATUS] DEFAULT ((0)),
[MemberTypeId] [int] NOT NULL,
[RollSheetSectionId] [int] NOT NULL,
[FeePaid] [bit] NOT NULL CONSTRAINT [DF_ORGANIZATION_MEMBERS_TBL_FEE_PAID] DEFAULT ((0)),
[AllergyInfo] [bit] NOT NULL CONSTRAINT [DF_ORGANIZATION_MEMBERS_TBL_ALLERGY_INFO] DEFAULT ((0)),
[RobeNumber] [int] NULL,
[GroupNumber] [int] NULL,
[SectionId] [int] NULL,
[EnrollmentDate] [datetime] NULL,
[ClothingSizeInfo] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmergencyContactInfo] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[WhoBringsChildInfo] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AllergyText] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL,
[PositionId] [int] NULL,
[VipWeek1] [bit] NULL,
[VipWeek2] [bit] NULL,
[VipWeek3] [bit] NULL,
[VipWeek4] [bit] NULL,
[VipWeek5] [bit] NULL,
[InactiveDate] [datetime] NULL,
[AttendStr] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AttendPct] [real] NULL,
[LastAttended] [datetime] NULL
)
GO
ALTER TABLE [dbo].[OrganizationMembers] ADD CONSTRAINT [ORGANIZATION_MEMBERS_PK] PRIMARY KEY NONCLUSTERED ([OrganizationId], [PeopleId])
GO
CREATE NONCLUSTERED INDEX [ORGANIZATION_MEMBERS_PPL_FK_IX] ON [dbo].[OrganizationMembers] ([PeopleId])
GO
ALTER TABLE [dbo].[OrganizationMembers] WITH NOCHECK ADD CONSTRAINT [FK_ORGANIZATION_MEMBERS_TBL_MemberType] FOREIGN KEY ([MemberTypeId]) REFERENCES [lookup].[MemberType] ([Id])
GO
ALTER TABLE [dbo].[OrganizationMembers] WITH NOCHECK ADD CONSTRAINT [ORGANIZATION_MEMBERS_ORG_FK] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[OrganizationMembers] WITH NOCHECK ADD CONSTRAINT [ORGANIZATION_MEMBERS_PPL_FK] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId]) ON DELETE CASCADE
GO
