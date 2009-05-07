CREATE TABLE [dbo].[People]
(
[PeopleId] [int] NOT NULL IDENTITY(1, 1),
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[DropCodeId] [int] NOT NULL,
[GenderId] [int] NOT NULL CONSTRAINT [DF_PEOPLE_TBL_GENDER_ID] DEFAULT ((0)),
[DoNotMailFlag] [bit] NOT NULL CONSTRAINT [DF_PEOPLE_TBL_DO_NOT_MAIL_FLAG] DEFAULT ((0)),
[DoNotCallFlag] [bit] NOT NULL CONSTRAINT [DF_PEOPLE_TBL_DO_NOT_CALL_FLAG] DEFAULT ((0)),
[DoNotVisitFlag] [bit] NOT NULL CONSTRAINT [DF_PEOPLE_TBL_DO_NOT_VISIT_FLAG] DEFAULT ((0)),
[AddressTypeId] [int] NOT NULL CONSTRAINT [DF_PEOPLE_TBL_ADDRESS_TYPE_ID] DEFAULT ((10)),
[PhonePrefId] [int] NOT NULL CONSTRAINT [DF_PEOPLE_TBL_PHONE_PREF_ID] DEFAULT ((10)),
[MaritalStatusId] [int] NOT NULL,
[PositionInFamilyId] [int] NOT NULL,
[MemberStatusId] [int] NOT NULL,
[FamilyId] [int] NOT NULL,
[Grade] AS ([dbo].[SchoolGrade]([PeopleId])),
[BirthMonth] [int] NULL,
[BirthDay] [int] NULL,
[BirthYear] [int] NULL,
[OriginId] [int] NULL,
[EntryPointId] [int] NULL,
[InterestPointId] [int] NULL,
[BaptismTypeId] [int] NULL,
[BaptismStatusId] [int] NULL,
[DecisionTypeId] [int] NULL,
[DiscoveryClassStatusId] [int] NULL,
[NewMbrClassStatusId] [int] NULL,
[LetterStatusId] [int] NULL,
[JoinCodeId] [int] NOT NULL CONSTRAINT [DF_PEOPLE_TBL_JOIN_CODE_ID] DEFAULT ((0)),
[EnvelopeOptionsId] [int] NULL,
[BadAddressFlag] [bit] NULL,
[AltBadAddressFlag] [bit] NULL,
[ResCodeId] [int] NULL,
[AltResCodeId] [int] NULL,
[AddressFromDate] [datetime] NULL,
[AddressToDate] [datetime] NULL,
[AltAddressFromDate] [datetime] NULL,
[AltAddressToDate] [datetime] NULL,
[WeddingDate] [datetime] NULL,
[OriginDate] [datetime] NULL,
[BaptismSchedDate] [datetime] NULL,
[BaptismDate] [datetime] NULL,
[DecisionDate] [datetime] NULL,
[DiscoveryClassDate] [datetime] NULL,
[NewMbrClassDateCompleted] [datetime] NULL,
[LetterDateRequested] [datetime] NULL,
[LetterDateReceived] [datetime] NULL,
[JoinDate] [datetime] NULL,
[DropDate] [datetime] NULL,
[DeceasedDate] [datetime] NULL,
[TitleCode] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FirstName] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MiddleName] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MaidenName] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LastName] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SuffixCode] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NickName] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AddressLineOne] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AddressLineTwo] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityName] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateCode] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ZipCode] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CountryName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StreetName] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AltAddressLineOne] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AltAddressLineTwo] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AltCityName] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AltStateCode] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AltZipCode] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AltCountryName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AltStreetName] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CellPhone] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[WorkPhone] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailAddress] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OtherPreviousChurch] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OtherNewChurch] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SchoolOther] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmployerOther] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OccupationOther] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HobbyOther] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SkillOther] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InterestOther] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LetterStatusNotes] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Comments] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ChristAsSavior] [bit] NOT NULL CONSTRAINT [DF_PEOPLE_TBL_CHRIST_AS_SAVIOR] DEFAULT ((0)),
[MemberAnyChurch] [bit] NULL,
[InterestedInJoining] [bit] NOT NULL CONSTRAINT [DF_PEOPLE_TBL_INTERESTED_IN_JOINING] DEFAULT ((0)),
[PleaseVisit] [bit] NOT NULL CONSTRAINT [DF_PEOPLE_TBL_PLEASE_VISIT] DEFAULT ((0)),
[InfoBecomeAChristian] [bit] NOT NULL CONSTRAINT [DF_PEOPLE_TBL_INFO_BECOME_A_CHRISTIAN] DEFAULT ((0)),
[ContributionsStatement] [bit] NOT NULL CONSTRAINT [DF_PEOPLE_TBL_CONTRIBUTIONS_STATEMENT] DEFAULT ((0)),
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL,
[PictureId] [int] NULL,
[ContributionOptionsId] [int] NULL,
[Age] AS ([dbo].[Age]([PeopleId])),
[PrimaryCity] AS ([dbo].[PrimaryCity]([PeopleId])),
[PrimaryZip] AS ([dbo].[PrimaryZip]([PeopleId])),
[PrimaryAddress] AS ([dbo].[PrimaryAddress]([PeopleId])),
[PrimaryState] AS ([dbo].[PrimaryState]([PeopleId])),
[HomePhone] AS ([dbo].[HomePhone]([PeopleId])),
[SpouseId] AS ([dbo].[SpouseId]([PeopleId])),
[PrimaryAddress2] AS ([dbo].[PrimaryAddress2]([PeopleId])),
[Name] AS ((case when [Nickname]<>'' then [nickname] else [FirstName] end+' ')+[LastName]),
[PrimaryResCode] AS ([dbo].[PrimaryResCode]([PeopleId])),
[PrimaryBadAddrFlag] AS ([dbo].[PrimaryBadAddressFlag]([PeopleId])),
[BibleFellowshipTeacher] AS ([dbo].[BibleFellowshipTeacher]([PeopleId])),
[PreferredName] AS (case when [Nickname]<>'' then [nickname] else [FirstName] end+' '),
[BibleFellowshipTeacherId] AS ([dbo].[BibleFellowshipTeacherId]([PeopleId])),
[Name2] AS (([LastName]+', ')+case when [Nickname]<>'' then [nickname] else [FirstName] end),
[LastContact] AS ([dbo].[LastContact]([PeopleId])),
[BibleFellowshipClassId] AS ([dbo].[BibleFellowshipClassId]([PeopleId])),
[InBFClass] AS ([dbo].[InBFClass]([PeopleId]))
)

ALTER TABLE [dbo].[People] ADD
CONSTRAINT [FK_PEOPLE_TBL_DiscoveryClassStatus] FOREIGN KEY ([DiscoveryClassStatusId]) REFERENCES [lookup].[DiscoveryClassStatus] ([Id])
ALTER TABLE [dbo].[People] ADD
CONSTRAINT [FK_PEOPLE_TBL_DropStatus] FOREIGN KEY ([DropCodeId]) REFERENCES [lookup].[DropType] ([Id])
ALTER TABLE [dbo].[People] ADD
CONSTRAINT [FK_PEOPLE_TBL_JoinType] FOREIGN KEY ([JoinCodeId]) REFERENCES [lookup].[JoinType] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [PEOPLE_PK] PRIMARY KEY NONCLUSTERED ([PeopleId])
GO

CREATE NONCLUSTERED INDEX [IX_PEOPLE_TBL] ON [dbo].[People] ([EmailAddress])
GO

CREATE NONCLUSTERED INDEX [PEOPLE_FAMILY_FK_IX] ON [dbo].[People] ([FamilyId])
GO

ALTER TABLE [dbo].[People] WITH NOCHECK ADD CONSTRAINT [FK_PEOPLE_TBL_AddressType] FOREIGN KEY ([AddressTypeId]) REFERENCES [lookup].[AddressType] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_BaptismStatus] FOREIGN KEY ([BaptismStatusId]) REFERENCES [lookup].[BaptismStatus] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_BaptismType] FOREIGN KEY ([BaptismTypeId]) REFERENCES [lookup].[BaptismType] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_DecisionType] FOREIGN KEY ([DecisionTypeId]) REFERENCES [lookup].[DecisionType] ([Id])
GO

ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_EntryPoint] FOREIGN KEY ([EntryPointId]) REFERENCES [lookup].[EntryPoint] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_EnvelopeOption] FOREIGN KEY ([EnvelopeOptionsId]) REFERENCES [lookup].[EnvelopeOption] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_FamilyPosition] FOREIGN KEY ([PositionInFamilyId]) REFERENCES [lookup].[FamilyPosition] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_Gender] FOREIGN KEY ([GenderId]) REFERENCES [lookup].[Gender] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_InterestPoint] FOREIGN KEY ([InterestPointId]) REFERENCES [lookup].[InterestPoint] ([Id])
GO

ALTER TABLE [dbo].[People] WITH NOCHECK ADD CONSTRAINT [FK_PEOPLE_TBL_MaritalStatus] FOREIGN KEY ([MaritalStatusId]) REFERENCES [lookup].[MaritalStatus] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_MemberLetterStatus] FOREIGN KEY ([LetterStatusId]) REFERENCES [lookup].[MemberLetterStatus] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_MemberStatus] FOREIGN KEY ([MemberStatusId]) REFERENCES [lookup].[MemberStatus] ([Id])
GO

ALTER TABLE [dbo].[People] WITH NOCHECK ADD CONSTRAINT [FK_PEOPLE_TBL_Origin] FOREIGN KEY ([OriginId]) REFERENCES [lookup].[Origin] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_PhonePreference] FOREIGN KEY ([PhonePrefId]) REFERENCES [lookup].[PhonePreference] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_Picture] FOREIGN KEY ([PictureId]) REFERENCES [dbo].[Picture] ([PictureId])
GO

ALTER TABLE [dbo].[People] WITH NOCHECK ADD CONSTRAINT [PEOPLE_FAMILY_FK] FOREIGN KEY ([FamilyId]) REFERENCES [dbo].[Families] ([FamilyId]) ON DELETE CASCADE
GO
