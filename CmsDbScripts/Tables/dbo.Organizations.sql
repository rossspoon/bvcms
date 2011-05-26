CREATE TABLE [dbo].[Organizations]
(
[OrganizationId] [int] NOT NULL IDENTITY(1, 1),
[CreatedBy] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[OrganizationStatusId] [int] NOT NULL,
[DivisionId] [int] NULL,
[LeaderMemberTypeId] [int] NULL,
[GradeAgeStart] [int] NULL,
[GradeAgeEnd] [int] NULL,
[RollSheetVisitorWks] [int] NULL,
[AttendTrkLevelId] [int] NOT NULL,
[SecurityTypeId] [int] NOT NULL,
[AttendClassificationId] [int] NOT NULL CONSTRAINT [DF_Organizations_AttendClassificationId] DEFAULT ((0)),
[FirstMeetingDate] [datetime] NULL,
[LastMeetingDate] [datetime] NULL,
[OrganizationClosedDate] [datetime] NULL,
[Location] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OrganizationName] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL,
[ScheduleId] [int] NULL,
[EntryPointId] [int] NULL,
[ParentOrgId] [int] NULL,
[AllowAttendOverlap] [bit] NOT NULL CONSTRAINT [DF_Organizations_AllowAttendOverlap] DEFAULT ((0)),
[MemberCount] [int] NULL,
[LeaderId] [int] NULL,
[LeaderName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ClassFilled] [bit] NULL,
[OnLineCatalogSort] [int] NULL,
[PendingLoc] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CanSelfCheckin] [bit] NULL,
[NumCheckInLabels] [int] NULL,
[CampusId] [int] NULL,
[AllowNonCampusCheckIn] [bit] NULL,
[NumWorkerCheckInLabels] [int] NULL,
[SchedTime] [datetime] NULL,
[SchedDay] [int] NULL,
[MeetingTime] [datetime] NULL,
[ShowOnlyRegisteredAtCheckIn] [bit] NULL,
[Limit] [int] NULL,
[EmailAddresses] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RegType] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailMessage] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailSubject] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Instructions] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GenderId] [int] NULL,
[Fee] [money] NULL,
[Description] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BirthDayStart] [datetime] NULL,
[BirthDayEnd] [datetime] NULL,
[VisitorDate] AS (dateadd(day, -(7)*isnull([RollSheetVisitorWks],(3)),getdate())),
[Deposit] [money] NULL,
[ShirtFee] [money] NULL,
[ExtraFee] [money] NULL,
[LastDayBeforeExtra] [datetime] NULL,
[AskTylenolEtc] [bit] NULL,
[AskAllergies] [bit] NULL,
[AskShirtSize] [bit] NULL,
[AskRequest] [bit] NULL,
[AskParents] [bit] NULL,
[AskEmContact] [bit] NULL,
[AskMedical] [bit] NULL,
[AskInsurance] [bit] NULL,
[AllowLastYearShirt] [bit] NULL,
[AskDoctor] [bit] NULL,
[AskCoaching] [bit] NULL,
[AskChurch] [bit] NULL,
[AskGrade] [bit] NULL,
[Terms] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AskTickets] [bit] NULL,
[MaximumFee] [money] NULL,
[AskOptions] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AgeFee] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AllowOnlyOne] [bit] NULL,
[RegistrationTypeId] [int] NULL,
[AgeGroups] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ValidateOrgs] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MemberOnly] [bit] NULL,
[YesNoQuestions] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OrgMemberFees] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ExtraQuestions] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PhoneNumber] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GradeOptions] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LinkGroupsFromOrgs] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RegistrationClosed] [bit] NULL,
[AllowKioskRegister] [bit] NULL,
[RequestLabel] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[WorshipGroupCodes] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsBibleFellowshipOrg] [bit] NULL,
[ExtraOptions] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Shell] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ShirtSizes] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NoSecurityLabel] [bit] NULL,
[AlwaysSecurityLabel] [bit] NULL,
[MenuItems] [varchar] (2500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OptionsLabel] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ExtraOptionsLabel] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DaysToIgnoreHistory] [int] NULL,
[GroupToJoin] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GiveOrgMembAccess] [bit] NULL,
[NumItemsLabel] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NotReqDOB] [bit] NULL,
[NotReqAddr] [bit] NULL,
[NotReqZip] [bit] NULL,
[NotReqPhone] [bit] NULL,
[NotReqGender] [bit] NULL,
[NotReqMarital] [bit] NULL,
[GradeLabel] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Checkboxes] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CheckboxesLabel] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AskDonation] [bit] NULL,
[NotifyIds] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DonationFundId] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
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
CREATE TRIGGER [dbo].[insOrg] 
   ON  dbo.Organizations 
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Organizations
	SET ScheduleId = dbo.ScheduleId(SchedDay, SchedTime),
	MeetingTime = dbo.GetScheduleTime(SchedDay, SchedTime)
	WHERE OrganizationId IN (SELECT OrganizationId FROM INSERTED)
	
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
CREATE TRIGGER [dbo].[updOrg] 
   ON  [dbo].[Organizations] 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	IF UPDATE(SchedDay) OR UPDATE(SchedTime)
		UPDATE dbo.Organizations
		SET ScheduleId = dbo.ScheduleId(SchedDay, SchedTime),
		MeetingTime = dbo.GetScheduleTime(SchedDay, SchedTime)
		WHERE OrganizationId IN (SELECT OrganizationId FROM INSERTED)
	
	IF UPDATE(IsBibleFellowshipOrg)
		OR UPDATE(LeaderMemberTypeId)
	BEGIN
		IF UPDATE(LeaderMemberTypeId)
			UPDATE dbo.Organizations
			SET LeaderId = dbo.OrganizationLeaderId(OrganizationId),
			LeaderName = dbo.OrganizationLeaderName(OrganizationId)
			WHERE OrganizationId IN (SELECT OrganizationId FROM INSERTED)
		UPDATE dbo.People
		SET BibleFellowshipClassId = dbo.BibleFellowshipClassId(p.PeopleId)
		FROM dbo.People p
		JOIN dbo.OrganizationMembers m ON p.PeopleId = m.PeopleId
		JOIN INSERTED o ON m.OrganizationId = o.OrganizationId
	END
END
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [ORGANIZATIONS_PK] PRIMARY KEY NONCLUSTERED  ([OrganizationId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Organizations] ON [dbo].[Organizations] ([DivisionId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Organizations_1] ON [dbo].[Organizations] ([OrganizationStatusId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Organizations] WITH NOCHECK ADD CONSTRAINT [ChildOrgs__ParentOrg] FOREIGN KEY ([ParentOrgId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_Organizations_AttendTrackLevel] FOREIGN KEY ([AttendTrkLevelId]) REFERENCES [lookup].[AttendTrackLevel] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_Organizations_Campus] FOREIGN KEY ([CampusId]) REFERENCES [lookup].[Campus] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_Organizations_ContributionFund] FOREIGN KEY ([DonationFundId]) REFERENCES [dbo].[ContributionFund] ([FundId])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_Organizations_Division] FOREIGN KEY ([DivisionId]) REFERENCES [dbo].[Division] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_Organizations_Gender] FOREIGN KEY ([GenderId]) REFERENCES [lookup].[Gender] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_Organizations_RegistrationType] FOREIGN KEY ([RegistrationTypeId]) REFERENCES [lookup].[RegistrationType] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_ORGANIZATIONS_TBL_EntryPoint] FOREIGN KEY ([EntryPointId]) REFERENCES [lookup].[EntryPoint] ([Id])
GO
ALTER TABLE [dbo].[Organizations] ADD CONSTRAINT [FK_ORGANIZATIONS_TBL_OrganizationStatus] FOREIGN KEY ([OrganizationStatusId]) REFERENCES [lookup].[OrganizationStatus] ([Id])
GO
