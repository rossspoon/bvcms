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
[CoupleFlag] AS ([dbo].[CoupleFlag]([FamilyId])),
[HomePhoneLU] [char] (7) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HomePhoneAC] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
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
CREATE TRIGGER [dbo].[insFamily] 
   ON  [dbo].[Families] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @fid INT, @phone VARCHAR(20)

	DECLARE c CURSOR FORWARD_ONLY FOR
	SELECT FamilyId, HomePhone
	FROM inserted 

	OPEN c
	FETCH NEXT FROM c INTO @fid, @phone
	WHILE @@FETCH_STATUS = 0
	BEGIN
		UPDATE dbo.People
		SET HomePhone = @phone
		WHERE FamilyId = @fid
		
		UPDATE dbo.Families
		SET HomePhoneLU = RIGHT(HomePhone, 7),
			HomePhoneAC = LEFT(RIGHT(REPLICATE('0',10) + HomePhone, 10), 3),
			HeadOfHouseholdId = dbo.HeadOfHouseholdId(FamilyId),
			HeadOfHouseholdSpouseId = dbo.HeadOfHouseHoldSpouseId(FamilyId)
		WHERE FamilyId = @fid
		
		FETCH NEXT FROM c INTO @fid, @phone
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
CREATE TRIGGER [dbo].[updFamily] 
   ON  [dbo].[Families] 
   FOR UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF NOT UPDATE(HomePhone) 
		RETURN 

	DECLARE @fid INT, @phone VARCHAR(20)

	DECLARE c CURSOR FORWARD_ONLY FOR
	SELECT FamilyId, HomePhone
	FROM inserted

	OPEN c
	FETCH NEXT FROM c INTO @fid, @phone
	WHILE @@FETCH_STATUS = 0
	BEGIN
		UPDATE dbo.People
		SET HomePhone = @phone
		WHERE FamilyId = @fid
		
		UPDATE dbo.Families
		SET HomePhoneLU = RIGHT(@phone, 7),
			HomePhoneAC = LEFT(RIGHT(REPLICATE('0',10) + HomePhone, 10), 3),
			HeadOfHouseholdId = dbo.HeadOfHouseholdId(FamilyId),
			HeadOfHouseholdSpouseId = dbo.HeadOfHouseHoldSpouseId(FamilyId)

		WHERE FamilyId = @fid
		
		FETCH NEXT FROM c INTO @fid, @phone
	END
	CLOSE c
	DEALLOCATE c
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
