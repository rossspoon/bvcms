CREATE TABLE [dbo].[People]
(
[PeopleId] [int] NOT NULL IDENTITY(1, 1),
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NULL,
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
[FirstName] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MiddleName] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MaidenName] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LastName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[HomePhone] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SpouseId] AS ([dbo].[SpouseId]([PeopleId])),
[PrimaryAddress2] AS ([dbo].[PrimaryAddress2]([PeopleId])),
[PrimaryResCode] AS ([dbo].[PrimaryResCode]([PeopleId])),
[PrimaryBadAddrFlag] AS ([dbo].[PrimaryBadAddressFlag]([PeopleId])),
[BibleFellowshipTeacher] AS ([dbo].[BibleFellowshipTeacher]([PeopleId])),
[BibleFellowshipTeacherId] AS ([dbo].[BibleFellowshipTeacherId]([PeopleId])),
[LastContact] AS ([dbo].[LastContact]([PeopleId])),
[InBFClass] AS ([dbo].[InBFClass]([PeopleId])),
[Grade] [int] NULL,
[CellPhoneLU] [char] (7) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[WorkPhoneLU] [char] (7) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BibleFellowshipClassId] AS ([dbo].[BibleFellowshipClassId2]([PeopleId])),
[Name] AS ((case when [Nickname]<>'' then [nickname] else [FirstName] end+' ')+[LastName]),
[PreferredName] AS (case when [Nickname]<>'' then [nickname] else [FirstName] end+' '),
[Name2] AS (([LastName]+', ')+case when [Nickname]<>'' then [nickname] else [FirstName] end),
[HashNum] AS (checksum([FirstName]+[LastName])),
[CampusId] [int] NULL,
[CellPhoneAC] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[delPeople] 
   ON  [dbo].[People]
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @fid INT
	DECLARE c CURSOR FOR
	SELECT FamilyId FROM deleted GROUP BY FamilyId
	OPEN c;
	FETCH NEXT FROM c INTO @fid;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		UPDATE dbo.Families SET HeadOfHouseHoldId = dbo.HeadOfHouseholdId(FamilyId),
			HeadOfHouseHoldSpouseId = dbo.HeadOfHouseHoldSpouseId(FamilyId)
		WHERE FamilyId = @fid
		FETCH NEXT FROM c INTO @fid;
	END;
	CLOSE c;
	DEALLOCATE c;

END

GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER insPerson 
   ON  dbo.People 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF NOT UPDATE(FamilyId)
		RETURN
		
	DECLARE @pid INT

	DECLARE c CURSOR FORWARD_ONLY FOR
	SELECT PeopleId
	FROM inserted

	OPEN c
	FETCH NEXT FROM c INTO @pid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		UPDATE dbo.People
		SET HomePhone = f.HomePhone
		FROM dbo.People p JOIN dbo.Families f ON p.FamilyId = f.FamilyId
		WHERE p.PeopleId = @pid
		
		FETCH NEXT FROM c INTO @pid
	END
	CLOSE c
	DEALLOCATE c

END
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[updPeople] 
   ON  [dbo].[People]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF (
		UPDATE(PositionInFamilyId) 
		OR UPDATE(GenderId) 
		OR UPDATE(DeceasedDate) 
		OR UPDATE(FirstName)
		OR UPDATE(MaritalStatusId)
		OR UPDATE(FamilyId)
    )
	BEGIN
		DECLARE @fid INT
		DECLARE c CURSOR FOR
		SELECT FamilyId FROM inserted GROUP BY FamilyId
		OPEN c;
		FETCH NEXT FROM c INTO @fid;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			UPDATE dbo.Families SET HeadOfHouseHoldId = dbo.HeadOfHouseholdId(FamilyId),
				HeadOfHouseHoldSpouseId = dbo.HeadOfHouseHoldSpouseId(FamilyId)
			WHERE FamilyId = @fid
			FETCH NEXT FROM c INTO @fid;
		END;
		CLOSE c;
		DEALLOCATE c;
	END
    IF UPDATE(CellPhone)
	BEGIN
		UPDATE dbo.People
		SET CellPhoneLU = RIGHT(CellPhone, 7),
			CellPhoneAC = LEFT(RIGHT(REPLICATE('0',10) + CellPhone, 10), 3)
		WHERE PeopleId IN (SELECT PeopleId FROM inserted)
	END

END

GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [PEOPLE_PK] PRIMARY KEY NONCLUSTERED  ([PeopleId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_People_2] ON [dbo].[People] ([CellPhoneLU]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PEOPLE_TBL] ON [dbo].[People] ([EmailAddress]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [PEOPLE_FAMILY_FK_IX] ON [dbo].[People] ([FamilyId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_People_BaptismType] FOREIGN KEY ([BaptismTypeId]) REFERENCES [lookup].[BaptismType] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_People_Campus] FOREIGN KEY ([CampusId]) REFERENCES [lookup].[Campus] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_People_DecisionType] FOREIGN KEY ([DecisionTypeId]) REFERENCES [lookup].[DecisionType] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_People_DropType] FOREIGN KEY ([DropCodeId]) REFERENCES [lookup].[DropType] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_People_Families] FOREIGN KEY ([FamilyId]) REFERENCES [dbo].[Families] ([FamilyId])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_People_FamilyPosition] FOREIGN KEY ([PositionInFamilyId]) REFERENCES [lookup].[FamilyPosition] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_People_Gender] FOREIGN KEY ([GenderId]) REFERENCES [lookup].[Gender] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_People_JoinType] FOREIGN KEY ([JoinCodeId]) REFERENCES [lookup].[JoinType] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_People_MaritalStatus] FOREIGN KEY ([MaritalStatusId]) REFERENCES [lookup].[MaritalStatus] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_People_MemberStatus] FOREIGN KEY ([MemberStatusId]) REFERENCES [lookup].[MemberStatus] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_People_Origin] FOREIGN KEY ([OriginId]) REFERENCES [lookup].[Origin] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_InterestPoint] FOREIGN KEY ([InterestPointId]) REFERENCES [lookup].[InterestPoint] ([Id])
GO
ALTER TABLE [dbo].[People] ADD CONSTRAINT [FK_PEOPLE_TBL_Picture] FOREIGN KEY ([PictureId]) REFERENCES [dbo].[Picture] ([PictureId])
GO
