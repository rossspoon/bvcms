BEGIN TRANSACTION

TRUNCATE TABLE dbo.Content
TRUNCATE TABLE dbo.UserRole;
TRUNCATE TABLE dbo.Preferences;
DELETE FROM dbo.Users;
DELETE FROM dbo.Roles;
DELETE FROM dbo.People
DELETE FROM dbo.Families
TRUNCATE TABLE dbo.ActivityLog;
DBCC CHECKIDENT('[dbo].[QueryBuilderClauses]', RESEED, 0)
TRUNCATE TABLE dbo.QueryBuilderClauses
DBCC CHECKIDENT('[dbo].[QueryBuilderClauses]', RESEED, 0)
TRUNCATE TABLE dbo.RssFeed

DELETE FROM [lookup].[AddressType]
DELETE FROM [lookup].[AttendanceClassification]
DELETE FROM [lookup].[AttendTrackLevel]
DELETE FROM [lookup].[AttendType]
DELETE FROM [lookup].[BaptismStatus]
DELETE FROM [lookup].[BaptismType]
DELETE FROM [lookup].[BFCStatus]
DELETE FROM [lookup].[BundleHeaderTypes]
DELETE FROM [lookup].BundleStatusTypes
DELETE FROM [lookup].[ContactPreference]
DELETE FROM [lookup].[ContributionStatus]
DELETE FROM [lookup].[ContributionType]
DELETE FROM [lookup].[CountryLookup]
DELETE FROM [lookup].[DecisionType]
DELETE FROM [lookup].[DiscoveryClassStatus]
DELETE FROM [lookup].[DropType]
DELETE FROM [lookup].[EntryPoint]
DELETE FROM [lookup].[EnvelopeOption]
DELETE FROM [lookup].[FamilyMemberType]
DELETE FROM [lookup].[FamilyPosition]
DELETE FROM [lookup].[FamilyRelationship]
DELETE FROM [lookup].[Gender]
DELETE FROM [lookup].[GenderClass]
DELETE FROM [lookup].[InterestPoint]
DELETE FROM [lookup].[JoinType]
DELETE FROM [lookup].[MaritalStatus]
DELETE FROM [lookup].[MeetingType]
DELETE FROM [lookup].[MemberLetterStatus]
DELETE FROM [lookup].[MemberStatus]
DELETE FROM [lookup].[MemberType]
DELETE FROM [lookup].[NameSuffix]
DELETE FROM [lookup].[NameTitle]
DELETE FROM [lookup].[NewContactReason]
DELETE FROM [lookup].[NewContactType]
DELETE FROM [lookup].[NewMbrClassStatus]
DELETE FROM [lookup].[OrganizationStatus]
DELETE FROM [lookup].[Origin]
DELETE FROM [lookup].[PhonePreference]
DELETE FROM [lookup].[ResidentCode]
DELETE FROM [lookup].[RollsheetType]
DELETE FROM [lookup].[StateLookup]
DELETE FROM [lookup].[TaskStatus]
DELETE FROM [lookup].[VolApplicationStatus]
DELETE FROM [lookup].[VolunteerCodes]
DELETE FROM [lookup].[WeeklySchedule]

INSERT INTO [lookup].[AddressType] ([Id], [Code], [Description]) VALUES (10, 'F1', 'Family')
INSERT INTO [lookup].[AddressType] ([Id], [Code], [Description]) VALUES (20, 'F2', 'Family Alternate')
INSERT INTO [lookup].[AddressType] ([Id], [Code], [Description]) VALUES (30, 'P1', 'Personal')
INSERT INTO [lookup].[AddressType] ([Id], [Code], [Description]) VALUES (40, 'P2', 'Personal Alternate')

INSERT INTO [lookup].[AttendanceClassification] ([Id], [Code], [Description]) VALUES (0, 'N', 'Normal')
INSERT INTO [lookup].[AttendanceClassification] ([Id], [Code], [Description]) VALUES (1, 'IS', 'InService')
INSERT INTO [lookup].[AttendanceClassification] ([Id], [Code], [Description]) VALUES (2, 'OS', 'OffSite')
INSERT INTO [lookup].[AttendanceClassification] ([Id], [Code], [Description]) VALUES (3, 'B', 'Baptism')
INSERT INTO [lookup].[AttendanceClassification] ([Id], [Code], [Description]) VALUES (4, 'DC', 'Step 1 Class')

INSERT INTO [lookup].[AttendTrackLevel] ([Id], [Code], [Description]) VALUES (0, 'N', 'None')
INSERT INTO [lookup].[AttendTrackLevel] ([Id], [Code], [Description]) VALUES (10, 'HC', 'Headcount')
INSERT INTO [lookup].[AttendTrackLevel] ([Id], [Code], [Description]) VALUES (20, 'I', 'Individual')

INSERT INTO [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (10, 'L', 'Leader')
INSERT INTO [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (20, 'VO', 'Volunteer')
INSERT INTO [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (30, 'M', 'Member')
INSERT INTO [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (40, 'VM', 'Visiting Member')
INSERT INTO [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (50, 'RV', 'Recent Visitor')
INSERT INTO [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (60, 'NV', 'New Visitor')
INSERT INTO [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (70, 'ISM', 'In-Service')
INSERT INTO [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (80, 'OFS', 'Offsite')
INSERT INTO [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (90, 'GRP', 'Group')
INSERT INTO [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (100, 'HMB', 'Homebound')
INSERT INTO [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (110, 'OC', 'Other Class')

INSERT INTO [lookup].[BaptismStatus] ([Id], [Code], [Description]) VALUES (0, 'NSP', 'Not Specified')
INSERT INTO [lookup].[BaptismStatus] ([Id], [Code], [Description]) VALUES (10, 'SCH', 'Scheduled')
INSERT INTO [lookup].[BaptismStatus] ([Id], [Code], [Description]) VALUES (20, 'NSC', 'Not Scheduled')
INSERT INTO [lookup].[BaptismStatus] ([Id], [Code], [Description]) VALUES (30, 'CMP', 'Completed')
INSERT INTO [lookup].[BaptismStatus] ([Id], [Code], [Description]) VALUES (40, 'CAN', 'Cancelled')

INSERT INTO [lookup].[BaptismType] ([Id], [Code], [Description]) VALUES (0, 'NSP', 'Not Specified')
INSERT INTO [lookup].[BaptismType] ([Id], [Code], [Description]) VALUES (10, 'ORI', 'Original')
INSERT INTO [lookup].[BaptismType] ([Id], [Code], [Description]) VALUES (20, 'SUB', 'Subsequent')
INSERT INTO [lookup].[BaptismType] ([Id], [Code], [Description]) VALUES (30, 'BIO', 'Biological')
INSERT INTO [lookup].[BaptismType] ([Id], [Code], [Description]) VALUES (40, 'NON', 'Non-Member')
INSERT INTO [lookup].[BaptismType] ([Id], [Code], [Description]) VALUES (50, 'RFM', 'Required')

INSERT INTO [lookup].[BFCStatus] ([Id], [Code], [Description]) VALUES (0, 'NON', 'Non-member')
INSERT INTO [lookup].[BFCStatus] ([Id], [Code], [Description]) VALUES (10, 'VST', 'Visitor')
INSERT INTO [lookup].[BFCStatus] ([Id], [Code], [Description]) VALUES (20, 'RVT', 'Recent Visitor')
INSERT INTO [lookup].[BFCStatus] ([Id], [Code], [Description]) VALUES (30, 'MEM', 'Member')
INSERT INTO [lookup].[BFCStatus] ([Id], [Code], [Description]) VALUES (40, 'HMB', 'Homebound')

INSERT INTO [lookup].[BundleHeaderTypes] ([Id], [Code], [Description]) VALUES (1, 'G', 'Generic Envelopes')
INSERT INTO [lookup].[BundleHeaderTypes] ([Id], [Code], [Description]) VALUES (2, 'LC', 'Loose Checks and Cash')
INSERT INTO [lookup].[BundleHeaderTypes] ([Id], [Code], [Description]) VALUES (3, 'PE', 'Preprinted Envelopes')
INSERT INTO [lookup].[BundleHeaderTypes] ([Id], [Code], [Description]) VALUES (4, 'OL', 'Online')

INSERT INTO [lookup].[BundleStatusTypes] ([Id], [Code], [Description]) VALUES (0, 'C', 'Closed')
INSERT INTO [lookup].[BundleStatusTypes] ([Id], [Code], [Description]) VALUES (1, 'O', 'Open')

INSERT INTO [lookup].[ContactPreference] ([Id], [Code], [Description]) VALUES (0, 'NO', 'Do Not Contact')
INSERT INTO [lookup].[ContactPreference] ([Id], [Code], [Description]) VALUES (10, 'PST', 'Mail')
INSERT INTO [lookup].[ContactPreference] ([Id], [Code], [Description]) VALUES (20, 'PHN', 'Phone')
INSERT INTO [lookup].[ContactPreference] ([Id], [Code], [Description]) VALUES (30, 'EML', 'Email')
INSERT INTO [lookup].[ContactPreference] ([Id], [Code], [Description]) VALUES (40, 'VST', 'Visit')

INSERT INTO [lookup].[ContributionStatus] ([Id], [Code], [Description]) VALUES (0, 'C', 'Recorded')
INSERT INTO [lookup].[ContributionStatus] ([Id], [Code], [Description]) VALUES (1, 'V', 'Reversed')
INSERT INTO [lookup].[ContributionStatus] ([Id], [Code], [Description]) VALUES (2, 'R', 'Returned')

INSERT INTO [lookup].[ContributionType] ([Id], [Code], [Description]) VALUES (1, 'CC', 'Check/Cash')
INSERT INTO [lookup].[ContributionType] ([Id], [Code], [Description]) VALUES (3, 'P', 'Brokered Property')
INSERT INTO [lookup].[ContributionType] ([Id], [Code], [Description]) VALUES (4, 'G', 'Grave Site')
INSERT INTO [lookup].[ContributionType] ([Id], [Code], [Description]) VALUES (6, 'RC', 'Returned Check')
INSERT INTO [lookup].[ContributionType] ([Id], [Code], [Description]) VALUES (7, 'RV', 'Reversed')
INSERT INTO [lookup].[ContributionType] ([Id], [Code], [Description]) VALUES (8, 'PL', 'Pledge')

INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('AFG', 'Afghanistan', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('AGO', 'Angola', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('ALB', 'Albania', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('ALG', 'Algeria', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('ARG', 'Argentina', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('ATA', 'Antarctica', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('AUS', 'Australia', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('AUT', 'Austria', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('BGD', 'Bangladesh', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('BGR', 'Bulgaria', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('BHR', 'Bahrain', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('BHS', 'Bahamas', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('BMU', 'Bermuda', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('BRA', 'Brazil', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('BRN', 'Brunei', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('BTN', 'Bhutan', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('BWA', 'Botswana', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('CAN', 'Canada', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('CHL', 'Chile', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('COL', 'Colombia', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('CPV', 'Cape Verde', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('CUB', 'Cuba', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('CYP', 'Cyprus', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('CZE', 'Czech Republic', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('DNK', 'Denmark', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('EGY', 'Egypt', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('ENG', 'England', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('ETH', 'Ethiopia', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('FIN', 'Finland', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('FRA', 'France', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('GER', 'Germany', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('GHA', 'Ghana', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('GIN', 'Guinea', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('GRC', 'Greece', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('GRD', 'Grenada', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('GUY', 'Guyana', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('HK', 'Hongkong', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('HUN', 'Hungary', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('IDN', 'Indonesia', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('IND', 'India', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('IRN', 'Iran', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('IRQ', 'Iraq', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('ISL', 'Iceland', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('ISR', 'Israel', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('ITA', 'Italy', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('JAM', 'Jamaica', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('JOR', 'Jordan', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('JPN', 'Japan', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('KOR', 'Korea', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('LBN', 'Lebanon', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('MAL', 'Malta', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('MDV', 'Maldives', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('MEX', 'Mexico', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('MLI', 'Mali', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('MMR', 'Myanmar', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('MNG', 'Mongolia', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('MOZ', 'Mozambique', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('MUS', 'Mauritius', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('MYS', 'Malaysia', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('NAM', 'Namibia', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('NLD', 'Netherlands', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('NOR', 'Norway', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('NPL', 'Nepal', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('NZL', 'New Zealand', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('PAN', 'Panama', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('POL', 'Poland', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('PRT', 'Portugal', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('QAT', 'Qatar', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('RUS', 'Russia', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('SDN', 'Sudan ', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('SGP', 'Singapore', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('SRI', 'Srilanka', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('THA', 'Thailand', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('TWN', 'Taiwan', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('UAE', 'United Arab Emirates', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('UGA', 'Uganda', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('UKR', 'Ukraine', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('URY', 'Uruguay', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('USA', 'USA', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('UZB', 'Uzbekistan', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('VEN', 'Venezuela', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('VNM', 'Vietnam', 1)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('YUG', 'Yugoslavia', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('ZMB', 'Zambia', 0)
INSERT INTO [lookup].[CountryLookup] ([CountryCode], [CountryName], [DisplayFlag]) VALUES ('ZWE', 'Zimbabwe', 1)

INSERT INTO [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (0, 'UNK', 'Unknown')
INSERT INTO [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (10, 'POF-MEM', 'POF for Membership')
INSERT INTO [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (20, 'POF-NON', 'POF NOT for Membership')
INSERT INTO [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (30, 'LETTER', 'Letter in from SBC')
INSERT INTO [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (40, 'STATEMENT', 'Statement')
INSERT INTO [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (50, 'BAP-REQD', 'Stmt requiring Baptism')
INSERT INTO [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (60, 'CANCELLED', 'Cancelled')

INSERT INTO [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (0, 'NSP', 'Not Specified')
INSERT INTO [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (10, 'PN', 'Pending')
INSERT INTO [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (20, 'AT', 'Attended')
INSERT INTO [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (30, 'AA', 'Admin Approval')
INSERT INTO [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (40, 'GF', 'Grandfathered')
INSERT INTO [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (50, 'EX', 'Exempted Child (thru Grade 8)')
INSERT INTO [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (99, 'UNK', 'Unknown')

INSERT INTO [lookup].[DropType] ([Id], [Code], [Description]) VALUES (0, 'NON', 'Non-Dropped')
INSERT INTO [lookup].[DropType] ([Id], [Code], [Description]) VALUES (10, 'DUP', 'Duplicate')
INSERT INTO [lookup].[DropType] ([Id], [Code], [Description]) VALUES (20, 'ADM', 'Administrative')
INSERT INTO [lookup].[DropType] ([Id], [Code], [Description]) VALUES (30, 'DEC', 'Deceased')
INSERT INTO [lookup].[DropType] ([Id], [Code], [Description]) VALUES (40, 'LET', 'Lettered Out')
INSERT INTO [lookup].[DropType] ([Id], [Code], [Description]) VALUES (50, 'REQ', 'Requested Drop')
INSERT INTO [lookup].[DropType] ([Id], [Code], [Description]) VALUES (60, 'AND', 'Another Denomination')
INSERT INTO [lookup].[DropType] ([Id], [Code], [Description]) VALUES (98, 'OTH', 'Other')

INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (0, 'NSP', 'Not Specified')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (10, 'BFC', 'Bible Fellowship')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (15, 'ENROLL', 'Enrollment (non-BF)')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (20, 'WORSHIP', 'Worship')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (30, 'SCT', 'SCT')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (35, 'CA', 'Celebrate America')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (40, 'MPP', 'MPP')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (50, 'ACTIVITIES', 'Activities Ministry')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (60, 'VBS', 'VBS')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (70, 'MUSIC', 'Music')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (80, 'FAMFUN', 'Family Fun Festival')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (82, 'SML', 'SoulMate Live')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (83, 'SMEN', 'Sportsmen')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (98, 'OTHER', 'Other')
INSERT INTO [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (99, 'UNKNOWN', 'Unknown')

INSERT INTO [lookup].[EnvelopeOption] ([Id], [Code], [Description]) VALUES (0, 'Null', 'Not Specified')
INSERT INTO [lookup].[EnvelopeOption] ([Id], [Code], [Description]) VALUES (1, 'I', 'Individual')
INSERT INTO [lookup].[EnvelopeOption] ([Id], [Code], [Description]) VALUES (2, 'J', 'Joint')
INSERT INTO [lookup].[EnvelopeOption] ([Id], [Code], [Description]) VALUES (9, 'N', 'None')

INSERT INTO [lookup].[FamilyMemberType] ([Id], [Code], [Description]) VALUES (0, 'ADU', 'Adult')
INSERT INTO [lookup].[FamilyMemberType] ([Id], [Code], [Description]) VALUES (1, 'CHI', 'Child')

INSERT INTO [lookup].[FamilyPosition] ([Id], [Code], [Description]) VALUES (10, '10', 'Primary Adult')
INSERT INTO [lookup].[FamilyPosition] ([Id], [Code], [Description]) VALUES (20, '20', 'Secondary Adult')
INSERT INTO [lookup].[FamilyPosition] ([Id], [Code], [Description]) VALUES (30, '30', 'Child')

INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (100, 'HOH', 'Head of Household')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (110, 'SPS', 'Spouse')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (120, 'SEC', 'Secondary Adult')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (130, 'AUN', 'Aunt')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (135, 'UNC', 'Uncle')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (140, 'GRM', 'Grand Mother')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (145, 'GRF', 'Grand Father')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (200, 'CHI', 'Child')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (210, 'DTR', 'Daughter')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (215, 'SON', 'Son')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (220, 'NCE', 'Niece')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (225, 'NPH', 'Nephew')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (230, 'GRD', 'Grand Daughter')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (235, 'GRS', 'Grand Son')
INSERT INTO [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (980, 'OTH', 'Other')

INSERT INTO [lookup].[Gender] ([Id], [Code], [Description]) VALUES (0, 'U', 'Unknown')
INSERT INTO [lookup].[Gender] ([Id], [Code], [Description]) VALUES (1, 'M', 'Male')
INSERT INTO [lookup].[Gender] ([Id], [Code], [Description]) VALUES (2, 'F', 'Female')

INSERT INTO [lookup].[GenderClass] ([Id], [Code], [Description]) VALUES (0, 'C', 'Coed')
INSERT INTO [lookup].[GenderClass] ([Id], [Code], [Description]) VALUES (1, 'M', 'Male')
INSERT INTO [lookup].[GenderClass] ([Id], [Code], [Description]) VALUES (2, 'F', 'Female')

INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (0, 'NSP', 'Not Specified')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (10, 'TV', 'TV')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (15, 'TV Ad', 'TV Ad')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (20, 'Radio', 'Radio')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (25, 'Radio Ad', 'Radio Ad')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (30, 'Newspaper', 'Newspaper')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (35, 'Mail', 'Mail')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (40, 'Friend', 'Friend')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (50, 'Relative', 'Relative')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (60, 'BillBoard', 'Billboard')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (70, 'Website', 'Website')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (80, 'EE Team', 'EE Team')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (98, 'Other', 'Other')
INSERT INTO [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (99, 'UNKNOWN', 'Unknown')

INSERT INTO [lookup].[JoinType] ([Id], [Code], [Description]) VALUES (0, 'UNK', 'Unknown')
INSERT INTO [lookup].[JoinType] ([Id], [Code], [Description]) VALUES (10, 'BPP', 'Baptism POF')
INSERT INTO [lookup].[JoinType] ([Id], [Code], [Description]) VALUES (20, 'BPS', 'Baptism SRB')
INSERT INTO [lookup].[JoinType] ([Id], [Code], [Description]) VALUES (30, 'BPB', 'Baptism BIO')
INSERT INTO [lookup].[JoinType] ([Id], [Code], [Description]) VALUES (40, 'STM', 'Statement')
INSERT INTO [lookup].[JoinType] ([Id], [Code], [Description]) VALUES (50, 'LET', 'Letter')

INSERT INTO [lookup].[MaritalStatus] ([Id], [Code], [Description]) VALUES (0, 'UNK', 'Unknown')
INSERT INTO [lookup].[MaritalStatus] ([Id], [Code], [Description]) VALUES (10, 'SNG', 'Single')
INSERT INTO [lookup].[MaritalStatus] ([Id], [Code], [Description]) VALUES (20, 'MAR', 'Married')
INSERT INTO [lookup].[MaritalStatus] ([Id], [Code], [Description]) VALUES (30, 'SEP', 'Separated')
INSERT INTO [lookup].[MaritalStatus] ([Id], [Code], [Description]) VALUES (40, 'DIV', 'Divorced')
INSERT INTO [lookup].[MaritalStatus] ([Id], [Code], [Description]) VALUES (50, 'WID', 'Widowed')

INSERT INTO [lookup].[MeetingType] ([Id], [Code], [Description]) VALUES (0, 'G', 'Group')
INSERT INTO [lookup].[MeetingType] ([Id], [Code], [Description]) VALUES (1, 'R', 'Roster')

INSERT INTO [lookup].[MemberLetterStatus] ([Id], [Code], [Description]) VALUES (0, 'NSP', '')
INSERT INTO [lookup].[MemberLetterStatus] ([Id], [Code], [Description]) VALUES (10, '1stReq', '1st Request')
INSERT INTO [lookup].[MemberLetterStatus] ([Id], [Code], [Description]) VALUES (20, '2ndReq', '2nd Request')
INSERT INTO [lookup].[MemberLetterStatus] ([Id], [Code], [Description]) VALUES (30, 'Non-Resp', 'Non-Responsive')
INSERT INTO [lookup].[MemberLetterStatus] ([Id], [Code], [Description]) VALUES (40, 'Complete', 'Complete')

INSERT INTO [lookup].[MemberStatus] ([Id], [Code], [Description]) VALUES (10, 'Yes', 'Member')
INSERT INTO [lookup].[MemberStatus] ([Id], [Code], [Description]) VALUES (20, 'No', 'Not Member')
INSERT INTO [lookup].[MemberStatus] ([Id], [Code], [Description]) VALUES (30, 'Pending', 'Pending Member')
INSERT INTO [lookup].[MemberStatus] ([Id], [Code], [Description]) VALUES (40, 'Previous', 'Previous Member')
INSERT INTO [lookup].[MemberStatus] ([Id], [Code], [Description]) VALUES (50, 'Add', 'Just Added')

INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (100, 'AD', 'Administrator', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (101, 'PR', 'President', 10)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (102, 'VP', 'Vice President', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (103, 'DR', 'Director', 10)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (104, 'C', 'Co-director', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (107, 'GO', 'General Officer', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (109, 'F', 'Facilitator', 10)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (110, 'RG', 'Registrar', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (111, 'CT', 'Staff Consultant', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (120, 'YT', 'Youth', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (122, 'PN', 'Pianist', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (125, 'SL', 'Section Leader', 10)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (130, 'CH', 'Chairman', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (131, 'SO', 'Social Chairman', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (132, 'VC', 'Vice-Chairman', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (135, 'IS', 'Instructor', 10)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (136, 'CC', 'Coach', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (140, 'L', 'Leader', 10)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (142, 'AL', 'Assistant Leader', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (143, 'GL', 'Group Leader', 10)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (145, 'CR', 'Coordinator', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (146, 'RL', 'Recreation Leader', 10)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (150, 'GR', 'Greeter', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (155, 'WK', 'Worker', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (160, 'T', 'Teacher', 10)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (161, 'AT', 'Assistant Teacher', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (162, 'SC', 'Secretary', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (163, 'AS', 'Assistant Secretary', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (166, 'CF', 'Craft Teacher', 10)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (167, 'CE', 'Co Teacher', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (170, 'IR', 'In Reach Leader', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (171, 'AI', 'Asst In Reach Ldr', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (172, 'OR', 'Outreach Leader', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (220, 'M', 'Member', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (230, 'IA', 'InActive', 40)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (280, 'AN', 'Deacon New', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (281, 'AR', 'Deacon Returning', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (300, 'VM', 'Visiting Member', 30)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (310, 'VS', 'Visitor', 60)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (400, 'IL', 'Independent Living', 100)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (405, 'LV', 'Assisted Living', 100)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (410, 'IH', 'In Home', 100)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (415, 'HB', 'Homebound', 100)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (420, 'NH', 'Nursing Home', 100)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (500, 'IM', 'In-Service Member', 70)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (700, 'VI', 'VIP', 20)
INSERT INTO [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (710, 'VL', 'Volunteer', 20)

INSERT INTO [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (100, 'Jr.', 'Jr.')
INSERT INTO [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (110, 'Sr.', 'Sr.')
INSERT INTO [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (120, 'I', 'I')
INSERT INTO [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (130, 'II', 'II')
INSERT INTO [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (140, 'III', 'III')
INSERT INTO [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (150, 'IV', 'IV')
INSERT INTO [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (160, 'V', 'V')

INSERT INTO [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (0, '', '')
INSERT INTO [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (100, 'Mr.', 'Mr.')
INSERT INTO [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (110, 'Mrs.', 'Mrs.')
INSERT INTO [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (120, 'Ms.', 'Ms.')
INSERT INTO [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (130, 'Miss', 'Miss')
INSERT INTO [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (140, 'Dr.', 'Dr.')
INSERT INTO [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (150, 'Rev.', 'Rev.')

INSERT INTO [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (99, 'U', 'Unknown')
INSERT INTO [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (100, 'B', 'Bereavement')
INSERT INTO [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (110, 'H', 'Health')
INSERT INTO [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (120, 'P', 'Personal')
INSERT INTO [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (130, 'OR', 'Out-Reach')
INSERT INTO [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (131, 'CS', 'Come and See')
INSERT INTO [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (140, 'IR', 'In-Reach')
INSERT INTO [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (150, 'I', 'Information')
INSERT INTO [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (160, 'O', 'Other')

INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (1, 'PV', 'Personal Visit')
INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (2, 'PC', 'Phone Call')
INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (3, 'L', 'Letter Sent')
INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (4, 'C', 'Card Sent')
INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (5, 'E', 'EMail Sent')
INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (6, 'I', 'Info Pack Sent')
INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (7, 'O', 'Other')
INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (8, 'WV', 'Worship Visit')
INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (9, 'SSV', 'BFC Visit')
INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (10, 'EV', 'Event Visit')
INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (11, 'PI', 'Phone In')
INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (12, 'SV', 'SurveyEE')
INSERT INTO [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (99, 'U', 'Unknown')

INSERT INTO [lookup].[NewMbrClassStatus] ([Id], [Code], [Description]) VALUES (0, 'NSP', 'Not Specified')
INSERT INTO [lookup].[NewMbrClassStatus] ([Id], [Code], [Description]) VALUES (10, 'Pending', 'Pending')
INSERT INTO [lookup].[NewMbrClassStatus] ([Id], [Code], [Description]) VALUES (20, 'Scheduled', 'Scheduled')
INSERT INTO [lookup].[NewMbrClassStatus] ([Id], [Code], [Description]) VALUES (30, 'Completed', 'Completed')

INSERT INTO [lookup].[OrganizationStatus] ([Id], [Code], [Description]) VALUES (10, 'C', 'Create')
INSERT INTO [lookup].[OrganizationStatus] ([Id], [Code], [Description]) VALUES (20, 'R', 'Review')
INSERT INTO [lookup].[OrganizationStatus] ([Id], [Code], [Description]) VALUES (30, 'A', 'Active')
INSERT INTO [lookup].[OrganizationStatus] ([Id], [Code], [Description]) VALUES (40, 'I', 'Inactive')

INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (0, 'NSP', 'Not Specified')
INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (10, 'VISIT', 'Visit')
INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (20, 'NEWNBR', 'New Neighbor List')
INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (30, 'REFERL', 'Referral')
INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (40, 'REQUEST', 'Request (Newsletter/Info)')
INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (50, 'DTC', 'Deacon Telephone')
INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (60, 'EESRVY', 'Survey (EE)')
INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (70, 'ENROLL', 'Enrollment')
INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (80, 'MEMDEC', 'Membership Decision')
INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (90, 'CONTRIB', 'Contribution')
INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (98, 'OTHER', 'Other')
INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (99, 'UNKNOWN', 'Unknown')
INSERT INTO [lookup].[Origin] ([Id], [Code], [Description]) VALUES (100, 'FAM', 'New Family Member')

INSERT INTO [lookup].[PhonePreference] ([Id], [Code], [Description]) VALUES (0, 'NO', 'Do Not Call')
INSERT INTO [lookup].[PhonePreference] ([Id], [Code], [Description]) VALUES (10, 'HME', 'Home')
INSERT INTO [lookup].[PhonePreference] ([Id], [Code], [Description]) VALUES (20, 'CEL', 'Cell')
INSERT INTO [lookup].[PhonePreference] ([Id], [Code], [Description]) VALUES (30, 'WRK', 'Work')

-- Add 4 rows to [lookup].[ResidentCode]
INSERT INTO [lookup].[ResidentCode] ([Id], [Code], [Description]) VALUES (10, 'M', 'Metro')
INSERT INTO [lookup].[ResidentCode] ([Id], [Code], [Description]) VALUES (20, 'G', 'Marginal')
INSERT INTO [lookup].[ResidentCode] ([Id], [Code], [Description]) VALUES (30, 'N', 'Non-Resident')
INSERT INTO [lookup].[ResidentCode] ([Id], [Code], [Description]) VALUES (40, 'U', 'Unable to Locate')

-- Add 7 rows to [lookup].[RollsheetType]
INSERT INTO [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (0, 'N', 'None')
INSERT INTO [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (1, 'S', 'Standard')
INSERT INTO [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (2, 'SBD', 'Standard with birthday')
INSERT INTO [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (4, 'YC', 'Younger Children (1-2 gr)')
INSERT INTO [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (5, 'OP1', 'Older Preschool (4-5 yr)')
INSERT INTO [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (6, 'OP2', 'Older Preschool (1-3 yr)')
INSERT INTO [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (7, 'BB', 'Bed Babies')

-- Add 59 rows to [lookup].[StateLookup]
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('AA', 'Armed Forces America')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('AE', 'Armed Forces East')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('AK', 'Alaska')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('AL', 'Alabama')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('AP', 'Armed Forces Pacific')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('AR', 'Arkansas')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('AZ', 'Arizona')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('CA', 'California')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('CO', 'Colorado')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('CT', 'Connecticut')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('CZ', 'Canal Zone')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('DC', 'District Of Columbia')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('DE', 'Delaware')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('FL', 'Florida')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('FR', 'Foreign Address')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('GA', 'Georgia')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('GU', 'Guam')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('HI', 'Hawaii')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('IA', 'Iowa')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('ID', 'Idaho')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('IL', 'Illinois')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('IN', 'Indiana')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('KS', 'Kansas')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('KY', 'Kentucky')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('LA', 'Louisiana')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('MA', 'Massachusetts')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('MD', 'Maryland')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('ME', 'Maine')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('MI', 'Michigan')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('MN', 'Minnesota')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('MO', 'Missouri')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('MS', 'Mississippi')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('MT', 'Montana')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('NC', 'North Carolina')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('ND', 'North Dakota')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('NE', 'Nebraska')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('NH', 'New Hampshire')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('NJ', 'New Jersey')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('NM', 'New Mexico')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('NV', 'Nevada')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('NY', 'New York')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('OH', 'Ohio')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('OK', 'Oklahoma')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('OR', 'Oregon')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('PA', 'Pennsylvania')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('PR', 'Puerto Rico')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('RI', 'Rhode Island')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('SC', 'South Carolina')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('SD', 'South Dakota')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('TN', 'Tennessee')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('TX', 'Texas')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('UT', 'Utah')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('VA', 'Virginia')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('VI', 'Virgin Islands')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('VT', 'Vermont')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('WA', 'Washington')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('WI', 'Wisconsin')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('WV', 'West Virginia')
INSERT INTO [lookup].[StateLookup] ([StateCode], [StateName]) VALUES ('WY', 'Wyoming')

-- Add 6 rows to [lookup].[TaskStatus]
INSERT INTO [lookup].[TaskStatus] ([Id], [Code], [Description]) VALUES (10, 'A', 'Active')
INSERT INTO [lookup].[TaskStatus] ([Id], [Code], [Description]) VALUES (20, 'W', 'Waiting For')
INSERT INTO [lookup].[TaskStatus] ([Id], [Code], [Description]) VALUES (30, 'S', 'Someday')
INSERT INTO [lookup].[TaskStatus] ([Id], [Code], [Description]) VALUES (40, 'C', 'Completed')
INSERT INTO [lookup].[TaskStatus] ([Id], [Code], [Description]) VALUES (50, 'P', 'Pending Acceptance')
INSERT INTO [lookup].[TaskStatus] ([Id], [Code], [Description]) VALUES (60, 'R', 'ReDelegated')

-- Add 5 rows to [lookup].[VolApplicationStatus]
INSERT INTO [lookup].[VolApplicationStatus] ([Id], [Code], [Description]) VALUES (0, 'UK', '(not specified)')
INSERT INTO [lookup].[VolApplicationStatus] ([Id], [Code], [Description]) VALUES (10, 'Appr', 'Approved')
INSERT INTO [lookup].[VolApplicationStatus] ([Id], [Code], [Description]) VALUES (20, 'WD', 'Withdrawn')
INSERT INTO [lookup].[VolApplicationStatus] ([Id], [Code], [Description]) VALUES (30, 'Not', 'Not Approved')
INSERT INTO [lookup].[VolApplicationStatus] ([Id], [Code], [Description]) VALUES (40, 'Pend', 'Pending')

-- Add 3 rows to [lookup].[VolunteerCodes]
INSERT INTO [lookup].[VolunteerCodes] ([Id], [Code], [Description]) VALUES (0, 'NA', 'None')
INSERT INTO [lookup].[VolunteerCodes] ([Id], [Code], [Description]) VALUES (10, 'S', 'Standard')
INSERT INTO [lookup].[VolunteerCodes] ([Id], [Code], [Description]) VALUES (30, 'L', 'Leader')

-- Add 27 rows to [lookup].[WeeklySchedule]
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1001, 'Sunday  8:00 AM', 0, '2008-11-14 08:00:00.000', 'Su0800')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1002, 'Sunday  9:30 AM', 0, '2008-11-14 09:30:00.000', 'Su0930')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1003, 'Sunday  11:00 AM', 0, '2008-11-14 11:00:00.000', 'Su1100')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1006, 'Sunday  4:00 PM', 0, '2008-11-14 16:00:00.000', 'Su1600')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1009, 'Sunday  6:00 PM', 0, '2008-11-14 18:00:00.000', 'Su1800')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1011, 'Monday 11:30 AM', 1, '2008-11-14 11:30:00.000', 'Mo1130')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1012, 'Monday 6:30 PM', 1, '2008-11-14 18:30:00.000', 'Mo1830')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1014, 'Tuesday 9:00 AM', 2, '2008-11-14 09:00:00.000', 'Tu0900')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1015, 'Tuesday 6:00 PM', 2, '2008-11-14 18:00:00.000', 'Tu1800')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1017, 'Tuesday 6:30 PM', 2, '2008-11-14 18:30:00.000', 'Tu1830')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1018, 'Wednesday 9:30 AM', 3, '2008-11-14 21:30:00.000', 'We0930')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1019, 'Wednesday 10:00 AM', 3, '2008-11-14 10:00:00.000', 'We1000')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1020, 'Wednesday 4:30 PM', 3, '2008-11-14 16:30:00.000', 'We1630')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1021, 'Wednesday 6:00 PM', 3, '2008-11-14 18:00:00.000', 'We1800')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1022, 'Wednesday 6:15 PM', 3, '2008-11-14 18:15:00.000', 'We1815')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1023, 'Wednesday 6:30 PM', 3, '2008-11-14 18:30:00.000', 'We1830')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1024, 'Wednesday 7:00 PM', 3, '2008-11-14 19:00:00.000', 'We1900')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1025, 'Wednesday 7:30 PM', 3, '2008-11-14 07:30:00.000', 'We1930')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1026, 'Thursday 9:30 AM', 4, '2008-11-14 09:30:00.000', 'Th0930')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1027, 'Thursday 7:30 PM', 2, '2008-11-14 19:30:00.000', 'Th1930')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1028, 'Friday 10:00 AM', 5, '2008-11-14 10:00:00.000', 'Fr1000')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1029, 'Friday 1:30 PM', 5, '2008-11-14 13:30:00.000', 'Fr1330')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1030, 'Friday 5:00 PM', 5, '2008-11-14 17:00:00.000', 'Fr1700')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1031, 'Friday 6:30 PM', 5, '2008-11-14 18:30:00.000', 'Fr1830')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1032, 'Saturday 8:30 AM', 6, '2008-11-14 08:30:00.000', 'Sa0830')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1034, 'Saturday 6:00 PM', 6, '2008-11-14 18:00:00.000', 'Sa1800')
INSERT INTO [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1035, 'Sunday 2:00 PM', 0, '2008-11-14 14:00:00.000', 'Su1400')


SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [dbo].[Users] ([UserId], [Username], [Password], [IsApproved], [EmailAddress], CreationDate)
VALUES (1, 'Admin', '', 1, 'bbcms01@bellevue.org', GETDATE())
SET IDENTITY_INSERT [dbo].[Users] OFF
DBCC CHECKIDENT('[dbo].[Users]', RESEED, 1)

SET IDENTITY_INSERT [dbo].[Families] ON
INSERT INTO dbo.Families (FamilyId,CreatedBy,CreatedDate,RecordStatus) VALUES (1,1,GETDATE(),0)
SET IDENTITY_INSERT [dbo].[Families] OFF
DBCC CHECKIDENT('[dbo].[Families]', RESEED, 1)

SET IDENTITY_INSERT [dbo].[People] ON
INSERT INTO dbo.People (PeopleId,CreatedBy,CreatedDate,DropCodeId,GenderId,
DoNotMailFlag,DoNotCallFlag,DoNotVisitFlag,
NewsletterFlag,AddressTypeId,PhonePrefId,
MaritalStatusId,PositionInFamilyId,MemberStatusId,LmApprovalLevelId,
FamilyId,FirstName,LastName,	
ChristAsSavior,InterestedInJoining,PleaseVisit,InfoBecomeAChristian,ContributionsStatement) 
VALUES (1,1,GETDATE(),0,0,
0,0,0,
0,10,0,
0,10,50,0,
1,'The','Admin',
0,0,0,0,0)
SET IDENTITY_INSERT [dbo].[People] OFF
DBCC CHECKIDENT('[dbo].[People]', RESEED, 1)

UPDATE dbo.Users
SET PeopleId = 1
WHERE UserId = 1

SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT INTO [dbo].[Roles] ([RoleId], [RoleName]) VALUES (1, 'Admin')
INSERT INTO [dbo].[Roles] ([RoleId], [RoleName]) VALUES (2, 'Staff')
INSERT INTO [dbo].[Roles] ([RoleId], [RoleName]) VALUES (3, 'Attendance')
INSERT INTO [dbo].[Roles] ([RoleId], [RoleName]) VALUES (4, 'Edit')
INSERT INTO [dbo].[Roles] ([RoleId], [RoleName]) VALUES (5, 'Membership')
INSERT INTO [dbo].[Roles] ([RoleId], [RoleName]) VALUES (6, 'OrgTagger')
INSERT INTO [dbo].[Roles] ([RoleId], [RoleName]) VALUES (7, 'OrgMembersOnly')
INSERT INTO [dbo].[Roles] ([RoleId], [RoleName]) VALUES (8, 'Finance')
INSERT INTO [dbo].[Roles] ([RoleId], [RoleName]) VALUES (9, 'Developer')
INSERT INTO [dbo].[Roles] ([RoleId], [RoleName]) VALUES (10, 'NoRemoteAccess')
SET IDENTITY_INSERT [dbo].[Roles] OFF
DBCC CHECKIDENT('[dbo].[Roles]', RESEED, 10)

INSERT INTO [dbo].[UserRole] ([UserId], [RoleId]) VALUES (1, 1)
INSERT INTO [dbo].[UserRole] ([UserId], [RoleId]) VALUES (1, 2)

INSERT INTO [dbo].[Content] ([Name], [Title], [Body]) 
VALUES ('Header', 'Header', '
<div style="float: left"><a href="/">
<img style="border-right-width: 0px; margin: 0px 20px 0px 5px; display: inline; border-top-width: 0px; border-bottom-width: 0px; border-left-width: 0px" title="bvcms" border="0" alt="bvcms" width="240" height="74" src="/Content/images/bvcms_8.png" /></a></div>
<div style="float: left">
<h1 style="color=#004080;margin-bottom:10px">Demo Baptist Church</h1>
<font color="#004080" size="4" face="Arial">
<em>A church where nothing is real</em></font></div>
')

INSERT INTO [dbo].[Content] ([Name], [Title], [Body]) 
VALUES ('Settings', 'Settings', 
'<dl>
    <dt>BFClassOrgTagId</dt>
    <dd>1</dd>
    <dt>TaskHost</dt>
    <dd>http://demo.bvcms.com</dd>
    <dt>CheckRemoteAccessRole</dt>
    <dd>false</dd>
    <dt>NewPeopleManagerId</dt>
    <dd>1</dd>
    <dt>SystemEmailAddress</dt>
    <dd>bbcms01@bellevue.org</dd>
    <dt>MaxExcelRows</dt>
    <dd>10000</dd>
    <dt>ChangePasswordDays</dt>
    <dd>360</dd>
    <dt>BlogAppUrl</dt>
    <dd>http://www.bvcms.com/blog/</dd>
    <dt>BlogFeedUrl</dt>
    <dd>http://disciples.bellevue.org/Blog/Feed.aspx?name=CMS2News</dd>
    <dt>QAServer</dt>
    <dd></dd>
</dl>')

INSERT INTO [dbo].[Content] ([Name], [Title], [Body]) 
VALUES ('TermsOfUse', 'Terms Of Use', '
<div style="width: 300px">
<p><span style="font-size: medium">Access to this site is given by request only. Please visit </span><a href="http://www.bvcms.com"><span style="font-size: medium">http://www.bvcms.com</span></a><span style="font-size: medium">&nbsp;and read the Demo Site page for more information about how to request credentials.</span></p>
<p>This web site is for demonstration purposes. Any resemblance to real people on this site is coincidental and unintended.</p>
<p>Copyright (c) 2009, Bellevue Baptist Church.</p>
<p>The source code is licensed under the GPL (see <a href="http://bvcms.codeplex.com/license">license</a>)</p>
<p>By logging in below, you agree that you understand these terms.</p>
</div>
')

COMMIT TRANSACTION