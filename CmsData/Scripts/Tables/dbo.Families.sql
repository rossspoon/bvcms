CREATE TABLE [dbo].[Families]
(
[FamilyId] [int] NOT NULL IDENTITY(1, 1),
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NULL,
[RecordStatus] [bit] NOT NULL CONSTRAINT [DF_FAMILIES_TBL_RECORD_STATUS] DEFAULT ((0)),
[BadAddressFlag] [bit] NULL,
[AltBadAddressFlag] [bit] NULL,
[ResCodeId] [int] NULL,
[AltResCodeId] [int] NULL,
[AddressFromDate] [datetime] NULL,
[AddressToDate] [datetime] NULL,
[AltAddressFromDate] [datetime] NULL,
[AltAddressToDate] [datetime] NULL,
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
[HomePhone] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL,
[HeadOfHouseholdId] [int] NULL,
[HeadOfHouseholdSpouseId] [int] NULL,
[CoupleFlag] [int] NULL,
[HomePhoneLU] [char] (7) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HomePhoneAC] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)



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
CREATE TRIGGER [dbo].[updFamily] 
   ON  [dbo].[Families] 
   FOR UPDATE, INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF UPDATE(HomePhone)
	BEGIN
		UPDATE dbo.People
		SET HomePhone = f.HomePhone
		FROM dbo.People p
		JOIN dbo.Families f ON p.FamilyId = f.FamilyId
		WHERE f.FamilyId IN (SELECT FamilyId FROM INSERTED)
		
		UPDATE dbo.Families
		SET HomePhoneLU = RIGHT(HomePhone, 7),
			HomePhoneAC = LEFT(RIGHT(REPLICATE('0',10) + HomePhone, 10), 3)
		WHERE FamilyId IN (SELECT FamilyId FROM INSERTED)
	END
	
	IF UPDATE(CityName) 
	OR UPDATE(AltCityName)
	OR UPDATE(AddressLineOne) 
	OR UPDATE(AltAddressLineOne)
	OR UPDATE(AddressLineTwo) 
	OR UPDATE(AltAddressLineTwo)
	OR UPDATE(StateCode) 
	OR UPDATE(AltStateCode)
	OR UPDATE(ZipCode)
	OR UPDATE(AltZipCode)
	OR UPDATE(BadAddressFlag) 
	OR UPDATE(AltBadAddressFlag)
	OR UPDATE(ResCodeId) 
	OR UPDATE(AltResCodeId)
	BEGIN
		UPDATE dbo.People
		SET PrimaryCity = dbo.PrimaryCity(PeopleId),
		PrimaryAddress = dbo.PrimaryAddress(PeopleId),
		PrimaryAddress2 = dbo.PrimaryAddress2(PeopleId),
		PrimaryState = dbo.PrimaryState(PeopleId),
		PrimaryBadAddrFlag = dbo.PrimaryBadAddressFlag(PeopleId),
		PrimaryResCode = dbo.PrimaryResCode(PeopleId),
		PrimaryZip = dbo.PrimaryZip(PeopleId)
		WHERE FamilyId IN (SELECT FamilyId FROM inserted)
	END

END
GO



ALTER TABLE [dbo].[Families] ADD CONSTRAINT [FAMILIES_PK] PRIMARY KEY NONCLUSTERED  ([FamilyId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Families] ON [dbo].[Families] ([HomePhoneLU]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Families] ADD CONSTRAINT [FamiliesHeaded__HeadOfHousehold] FOREIGN KEY ([HeadOfHouseholdId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Families] ADD CONSTRAINT [FamiliesHeaded2__HeadOfHouseholdSpouse] FOREIGN KEY ([HeadOfHouseholdSpouseId]) REFERENCES [dbo].[People] ([PeopleId])
GO
