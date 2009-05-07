/****** Object:  Schema [lookup]    Script Date: 05/06/2009 19:26:35 ******/
CREATE SCHEMA [lookup] AUTHORIZATION [dbo]
GO
/****** Object:  Table [lookup].[MemberStatus]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[MemberStatus](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_MemberStatus_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[MemberStatus] ([Id], [Code], [Description]) VALUES (10, N'Yes', N'Member')
INSERT [lookup].[MemberStatus] ([Id], [Code], [Description]) VALUES (20, N'No', N'Not Member')
INSERT [lookup].[MemberStatus] ([Id], [Code], [Description]) VALUES (30, N'Pending', N'Pending Member')
INSERT [lookup].[MemberStatus] ([Id], [Code], [Description]) VALUES (40, N'Previous', N'Previous Member')
INSERT [lookup].[MemberStatus] ([Id], [Code], [Description]) VALUES (50, N'Add', N'Just Added')
/****** Object:  Table [lookup].[PhonePreference]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[PhonePreference](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_PhonePreference] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[PhonePreference] ([Id], [Code], [Description]) VALUES (0, N'NO', N'Do Not Call')
INSERT [lookup].[PhonePreference] ([Id], [Code], [Description]) VALUES (10, N'HME', N'Home')
INSERT [lookup].[PhonePreference] ([Id], [Code], [Description]) VALUES (20, N'CEL', N'Cell')
INSERT [lookup].[PhonePreference] ([Id], [Code], [Description]) VALUES (30, N'WRK', N'Work')
/****** Object:  Table [dbo].[Picture]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Picture](
	[PictureId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[LargeId] [int] NULL,
	[MediumId] [int] NULL,
	[SmallId] [int] NULL,
 CONSTRAINT [PK_Picture] PRIMARY KEY CLUSTERED 
(
	[PictureId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [lookup].[Origin]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[Origin](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_Origin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (0, N'NSP', N'Not Specified')
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (10, N'VISIT', N'Visit')
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (20, N'NEWNBR', N'New Neighbor List')
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (30, N'REFERL', N'Referral')
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (40, N'REQUEST', N'Request (Newsletter/Info)')
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (50, N'DTC', N'Deacon Telephone')
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (60, N'EESRVY', N'Survey (EE)')
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (70, N'ENROLL', N'Enrollment')
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (80, N'MEMDEC', N'Membership Decision')
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (90, N'CONTRIB', N'Contribution')
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (98, N'OTHER', N'Other')
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (99, N'UNKNOWN', N'Unknown')
INSERT [lookup].[Origin] ([Id], [Code], [Description]) VALUES (100, N'FAM', N'New Family Member')
/****** Object:  Table [lookup].[MemberLetterStatus]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[MemberLetterStatus](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_MemberLetterStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[MemberLetterStatus] ([Id], [Code], [Description]) VALUES (0, N'NSP', N'')
INSERT [lookup].[MemberLetterStatus] ([Id], [Code], [Description]) VALUES (10, N'1stReq', N'1st Request')
INSERT [lookup].[MemberLetterStatus] ([Id], [Code], [Description]) VALUES (20, N'2ndReq', N'2nd Request')
INSERT [lookup].[MemberLetterStatus] ([Id], [Code], [Description]) VALUES (30, N'Non-Resp', N'Non-Responsive')
INSERT [lookup].[MemberLetterStatus] ([Id], [Code], [Description]) VALUES (40, N'Complete', N'Complete')
/****** Object:  Table [lookup].[MaritalStatus]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[MaritalStatus](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_MaritalStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[MaritalStatus] ([Id], [Code], [Description]) VALUES (0, N'UNK', N'Unknown')
INSERT [lookup].[MaritalStatus] ([Id], [Code], [Description]) VALUES (10, N'SNG', N'Single')
INSERT [lookup].[MaritalStatus] ([Id], [Code], [Description]) VALUES (20, N'MAR', N'Married')
INSERT [lookup].[MaritalStatus] ([Id], [Code], [Description]) VALUES (30, N'SEP', N'Separated')
INSERT [lookup].[MaritalStatus] ([Id], [Code], [Description]) VALUES (40, N'DIV', N'Divorced')
INSERT [lookup].[MaritalStatus] ([Id], [Code], [Description]) VALUES (50, N'WID', N'Widowed')
/****** Object:  Table [lookup].[Gender]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[Gender](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_Gender] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[Gender] ([Id], [Code], [Description]) VALUES (0, N'U', N'Unknown')
INSERT [lookup].[Gender] ([Id], [Code], [Description]) VALUES (1, N'M', N'Male')
INSERT [lookup].[Gender] ([Id], [Code], [Description]) VALUES (2, N'F', N'Female')
/****** Object:  Table [lookup].[FamilyPosition]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[FamilyPosition](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_FamilyPosition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[FamilyPosition] ([Id], [Code], [Description]) VALUES (10, N'10', N'Primary Adult')
INSERT [lookup].[FamilyPosition] ([Id], [Code], [Description]) VALUES (20, N'20', N'Secondary Adult')
INSERT [lookup].[FamilyPosition] ([Id], [Code], [Description]) VALUES (30, N'30', N'Child')
/****** Object:  Table [lookup].[InterestPoint]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[InterestPoint](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_InterestPoint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (0, N'NSP', N'Not Specified')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (10, N'TV', N'TV')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (15, N'TV Ad', N'TV Ad')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (20, N'Radio', N'Radio')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (25, N'Radio Ad', N'Radio Ad')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (30, N'Newspaper', N'Newspaper')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (35, N'Mail', N'Mail')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (40, N'Friend', N'Friend')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (50, N'Relative', N'Relative')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (60, N'BillBoard', N'Billboard')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (70, N'Website', N'Website')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (80, N'EE Team', N'EE Team')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (98, N'Other', N'Other')
INSERT [lookup].[InterestPoint] ([Id], [Code], [Description]) VALUES (99, N'UNKNOWN', N'Unknown')
/****** Object:  Table [lookup].[JoinType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[JoinType](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_JoinType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[JoinType] ([Id], [Code], [Description]) VALUES (0, N'UNK', N'Unknown')
INSERT [lookup].[JoinType] ([Id], [Code], [Description]) VALUES (10, N'BPP', N'Baptism POF')
INSERT [lookup].[JoinType] ([Id], [Code], [Description]) VALUES (20, N'BPS', N'Baptism SRB')
INSERT [lookup].[JoinType] ([Id], [Code], [Description]) VALUES (30, N'BPB', N'Baptism BIO')
INSERT [lookup].[JoinType] ([Id], [Code], [Description]) VALUES (40, N'STM', N'Statement')
INSERT [lookup].[JoinType] ([Id], [Code], [Description]) VALUES (50, N'LET', N'Letter')
/****** Object:  Table [lookup].[DiscoveryClassStatus]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[DiscoveryClassStatus](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_DiscoveryClassStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (0, N'NSP', N'Not Specified')
INSERT [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (10, N'PN', N'Pending')
INSERT [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (20, N'AT', N'Attended')
INSERT [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (30, N'AA', N'Admin Approval')
INSERT [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (40, N'GF', N'Grandfathered')
INSERT [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (50, N'EX', N'Exempted Child (thru Grade 8)')
INSERT [lookup].[DiscoveryClassStatus] ([Id], [Code], [Description]) VALUES (99, N'UNK', N'Unknown')
/****** Object:  Table [lookup].[EntryPoint]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[EntryPoint](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_EntryPoint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (0, N'NSP', N'Not Specified')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (10, N'BFC', N'Bible Fellowship')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (15, N'ENROLL', N'Enrollment (non-BF)')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (20, N'WORSHIP', N'Worship')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (30, N'SCT', N'SCT')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (35, N'CA', N'Celebrate America')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (40, N'MPP', N'MPP')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (50, N'ACTIVITIES', N'Activities Ministry')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (60, N'VBS', N'VBS')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (70, N'MUSIC', N'Music')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (80, N'FAMFUN', N'Family Fun Festival')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (82, N'SML', N'SoulMate Live')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (83, N'SMEN', N'Sportsmen')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (98, N'OTHER', N'Other')
INSERT [lookup].[EntryPoint] ([Id], [Code], [Description]) VALUES (99, N'UNKNOWN', N'Unknown')
/****** Object:  Table [lookup].[EnvelopeOption]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[EnvelopeOption](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_EnvelopeOption] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[EnvelopeOption] ([Id], [Code], [Description]) VALUES (0, N'Null', N'Not Specified')
INSERT [lookup].[EnvelopeOption] ([Id], [Code], [Description]) VALUES (1, N'I', N'Individual')
INSERT [lookup].[EnvelopeOption] ([Id], [Code], [Description]) VALUES (2, N'J', N'Joint')
INSERT [lookup].[EnvelopeOption] ([Id], [Code], [Description]) VALUES (9, N'N', N'None')
/****** Object:  Table [lookup].[DecisionType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[DecisionType](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_DecisionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (0, N'UNK', N'Unknown')
INSERT [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (10, N'POF-MEM', N'POF for Membership')
INSERT [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (20, N'POF-NON', N'POF NOT for Membership')
INSERT [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (30, N'LETTER', N'Letter in from SBC')
INSERT [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (40, N'STATEMENT', N'Statement')
INSERT [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (50, N'BAP-REQD', N'Stmt requiring Baptism')
INSERT [lookup].[DecisionType] ([Id], [Code], [Description]) VALUES (60, N'CANCELLED', N'Cancelled')
/****** Object:  Table [lookup].[DropType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[DropType](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_DropType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[DropType] ([Id], [Code], [Description]) VALUES (0, N'NON', N'Non-Dropped')
INSERT [lookup].[DropType] ([Id], [Code], [Description]) VALUES (10, N'DUP', N'Duplicate')
INSERT [lookup].[DropType] ([Id], [Code], [Description]) VALUES (20, N'ADM', N'Administrative')
INSERT [lookup].[DropType] ([Id], [Code], [Description]) VALUES (30, N'DEC', N'Deceased')
INSERT [lookup].[DropType] ([Id], [Code], [Description]) VALUES (40, N'LET', N'Lettered Out')
INSERT [lookup].[DropType] ([Id], [Code], [Description]) VALUES (50, N'REQ', N'Requested Drop')
INSERT [lookup].[DropType] ([Id], [Code], [Description]) VALUES (60, N'AND', N'Another Denomination')
INSERT [lookup].[DropType] ([Id], [Code], [Description]) VALUES (98, N'OTH', N'Other')
/****** Object:  Table [lookup].[BaptismStatus]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[BaptismStatus](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_BaptismStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[BaptismStatus] ([Id], [Code], [Description]) VALUES (0, N'NSP', N'Not Specified')
INSERT [lookup].[BaptismStatus] ([Id], [Code], [Description]) VALUES (10, N'SCH', N'Scheduled')
INSERT [lookup].[BaptismStatus] ([Id], [Code], [Description]) VALUES (20, N'NSC', N'Not Scheduled')
INSERT [lookup].[BaptismStatus] ([Id], [Code], [Description]) VALUES (30, N'CMP', N'Completed')
INSERT [lookup].[BaptismStatus] ([Id], [Code], [Description]) VALUES (40, N'CAN', N'Cancelled')
/****** Object:  Table [lookup].[BaptismType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[BaptismType](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_BaptismType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[BaptismType] ([Id], [Code], [Description]) VALUES (0, N'NSP', N'Not Specified')
INSERT [lookup].[BaptismType] ([Id], [Code], [Description]) VALUES (10, N'ORI', N'Original')
INSERT [lookup].[BaptismType] ([Id], [Code], [Description]) VALUES (20, N'SUB', N'Subsequent')
INSERT [lookup].[BaptismType] ([Id], [Code], [Description]) VALUES (30, N'BIO', N'Biological')
INSERT [lookup].[BaptismType] ([Id], [Code], [Description]) VALUES (40, N'NON', N'Non-Member')
INSERT [lookup].[BaptismType] ([Id], [Code], [Description]) VALUES (50, N'RFM', N'Required')
/****** Object:  Table [lookup].[AddressType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[AddressType](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK__AddressType__148954CD] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[AddressType] ([Id], [Code], [Description]) VALUES (10, N'F1', N'Family')
INSERT [lookup].[AddressType] ([Id], [Code], [Description]) VALUES (20, N'F2', N'Family Alternate')
INSERT [lookup].[AddressType] ([Id], [Code], [Description]) VALUES (30, N'P1', N'Personal')
INSERT [lookup].[AddressType] ([Id], [Code], [Description]) VALUES (40, N'P2', N'Personal Alternate')
/****** Object:  UserDefinedFunction [dbo].[Age]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Age](@pid int)
RETURNS int
AS
	BEGIN
	
	  DECLARE
		@v_return int, 
		@v_end_date datetime,
		@m int,
		@d int,
		@y int,
		@p_deceased_date datetime,
		@p_drop_code_id int
		
select @m = BirthMonth, @d = BirthDay, @y = BirthYear, @p_deceased_date = DeceasedDate, @p_drop_code_id = DropCodeId from dbo.People where @pid = PeopleId


         SET @v_return = NULL

         IF @y IS NOT NULL AND NOT (@p_deceased_date IS NULL AND isnull(@p_drop_code_id, 0) = 30)
            /* 30=Deceased*/
            BEGIN

               SET @v_end_date = isnull(@p_deceased_date, getdate())

               SET @v_return = datepart(YEAR, @v_end_date) - @y

               IF isnull(@m, 1) > datepart(MONTH, @v_end_date)
                  SET @v_return = @v_return - 1
               ELSE 
                  IF isnull(@m, 1) = datepart(MONTH, @v_end_date) AND isnull(@d, 1) > datepart(DAY, @v_end_date)
                     SET @v_return = @v_return - 1

            END

	RETURN @v_return
	END
GO
/****** Object:  Table [dbo].[Program]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Program](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[BFProgram] [bit] NULL,
 CONSTRAINT [PK_Program] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Program] ON
INSERT [dbo].[Program] ([Id], [Name], [BFProgram]) VALUES (1, N'First Program', 1)
SET IDENTITY_INSERT [dbo].[Program] OFF
/****** Object:  Table [dbo].[Division]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Division](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[ProgId] [int] NULL,
 CONSTRAINT [PK_Division] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Division] ON
INSERT [dbo].[Division] ([Id], [Name], [ProgId]) VALUES (1, N'First Division', 1)
SET IDENTITY_INSERT [dbo].[Division] OFF
/****** Object:  UserDefinedFunction [dbo].[BibleFellowshipClassId]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[BibleFellowshipClassId]
	(
	@pid int
	)
RETURNS int
AS
	BEGIN
	declare @oid INT, @bfid INT
	SELECT TOP 1 @bfid = Id FROM dbo.Program WHERE BFProgram = 1

	select top 1 @oid = om.OrganizationId from dbo.OrganizationMembers AS om INNER JOIN
		dbo.Organizations AS o ON om.OrganizationId = o.OrganizationId inner JOIN
		dbo.DivOrg do ON o.OrganizationId = do.OrgId INNER JOIN
		dbo.Division d ON do.DivId = d.Id
		where d.ProgId = @bfid and om.PeopleId = @pid

	return @oid
	END
GO
/****** Object:  UserDefinedFunction [dbo].[BibleFellowshipTeacher]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[BibleFellowshipTeacher] (@pid int)
RETURNS varchar(100)
AS
	BEGIN
	declare @name varchar(100),	@oid int
	select @oid = dbo.BibleFellowshipClassId(@pid)
	SELECT  @name = dbo.OrganizationLeaderName(@oid)
	return @name
	END
GO
/****** Object:  UserDefinedFunction [dbo].[BibleFellowshipTeacherId]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[BibleFellowshipTeacherId]
	(
	@pid int
	)
RETURNS int
AS
	BEGIN
	declare @id varchar(100),	@oid int
	
	select @oid = dbo.BibleFellowshipClassId(@pid)

SELECT  TOP 1 @id = p.PeopleId
FROM         dbo.OrganizationMembers AS om INNER JOIN
             dbo.Organizations AS o ON om.OrganizationId = o.OrganizationId INNER JOIN
             dbo.People AS p ON om.PeopleId = p.PeopleId
WHERE     (om.MemberTypeId = o.LeaderMemberTypeId) AND (om.OrganizationId = @oid)
ORDER BY EnrollmentDate

	return @id
	END
GO
/****** Object:  Table [lookup].[NewContactReason]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[NewContactReason](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NOT NULL,
	[Description] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ContactReasons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (99, N'U', N'Unknown')
INSERT [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (100, N'B', N'Bereavement')
INSERT [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (110, N'H', N'Health')
INSERT [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (120, N'P', N'Personal')
INSERT [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (130, N'OR', N'Out-Reach')
INSERT [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (131, N'CS', N'Come and See')
INSERT [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (140, N'IR', N'In-Reach')
INSERT [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (150, N'I', N'Information')
INSERT [lookup].[NewContactReason] ([Id], [Code], [Description]) VALUES (160, N'O', N'Other')
/****** Object:  Table [lookup].[NewContactType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[NewContactType](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NOT NULL,
	[Description] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ContactTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (1, N'PV', N'Personal Visit')
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (2, N'PC', N'Phone Call')
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (3, N'L', N'Letter Sent')
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (4, N'C', N'Card Sent')
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (5, N'E', N'EMail Sent')
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (6, N'I', N'Info Pack Sent')
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (7, N'O', N'Other')
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (8, N'WV', N'Worship Visit')
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (9, N'SSV', N'BFC Visit')
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (10, N'EV', N'Event Visit')
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (11, N'PI', N'Phone In')
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (12, N'SV', N'SurveyEE')
INSERT [lookup].[NewContactType] ([Id], [Code], [Description]) VALUES (99, N'U', N'Unknown')
/****** Object:  Table [dbo].[Ministries]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ministries](
	[MinistryId] [int] IDENTITY(1,1) NOT NULL,
	[MinistryName] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[RecordStatus] [bit] NULL,
	[DepartmentId] [int] NULL,
	[MinistryDescription] [varchar](512) NULL,
	[MinistryContactId] [int] NULL,
	[ChurchId] [int] NULL,
 CONSTRAINT [PK_MINISTRIES_TBL] PRIMARY KEY CLUSTERED 
(
	[MinistryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [MINISTRIES_PK_IX] ON [dbo].[Ministries] 
(
	[MinistryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NewContact]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewContact](
	[ContactId] [int] IDENTITY(200000,1) NOT NULL,
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
	[Comments] [varchar](max) NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[LastContact]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[LastContact](@pid INT)
RETURNS DATETIME
AS
BEGIN
	DECLARE @dt DATETIME

	SELECT @dt = MAX(c.ContactDate) FROM dbo.NewContact c
	JOIN dbo.Contactees ce ON c.ContactId = ce.ContactId
	WHERE ce.PeopleId = @pid
	IF @dt IS NULL
		SELECT @dt = DATEADD(DAY,-5000,GETDATE())

	RETURN @dt

END
GO
/****** Object:  UserDefinedFunction [dbo].[HomePhone]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[HomePhone](@pid int)
RETURNS varchar(11)
AS
	BEGIN
	declare @homephone varchar(11)
	select @homephone = f.HomePhone from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

	RETURN @homephone
	END
GO
/****** Object:  UserDefinedFunction [dbo].[InBFClass]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[InBFClass]
	(
	@pid int
	)
RETURNS bit
AS
	BEGIN
	declare @mem BIT, @bfid INT
	SELECT TOP 1 @bfid = Id FROM dbo.Program WHERE BFProgram = 1

	select @mem = 1
	FROM dbo.OrganizationMembers  AS om
	join dbo.Organizations o on o.OrganizationId = om.OrganizationId
	join dbo.DivOrg AS do ON o.OrganizationId = do.OrgId
	join dbo.Division d ON do.DivId = d.Id
	where d.ProgId = @bfid and om.PeopleId = @pid

	RETURN isnull(@mem, 0)

	END
GO
/****** Object:  UserDefinedFunction [dbo].[PrimaryAddress]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[PrimaryAddress] ( @pid int )
RETURNS varchar(60)
AS
	BEGIN
declare @addr varchar(60)
select @addr =
	case AddressTypeId
			when 10 then f.AddressLineOne
			when 20 then f.AltAddressLineOne
			when 30 then p.AddressLineOne
			when 40 then p.AltAddressLineOne
	end
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

	RETURN @addr
	END
GO
/****** Object:  UserDefinedFunction [dbo].[PrimaryAddress2]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[PrimaryAddress2] ( @pid int )
RETURNS varchar(60)
AS
	BEGIN
declare @addr varchar(60)
select @addr =
	case AddressTypeId
			when 10 then f.AddressLineTwo
			when 20 then f.AltAddressLineTwo
			when 30 then p.AddressLineTwo
			when 40 then p.AltAddressLineTwo
	end
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

	RETURN @addr
	END
GO
/****** Object:  UserDefinedFunction [dbo].[PrimaryBadAddressFlag]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[PrimaryBadAddressFlag]( @pid int )
RETURNS int
AS
	BEGIN
declare @flag bit
select @flag =
	case AddressTypeId
		when 10 then f.BadAddressFlag
		when 20 then f.AltBadAddressFlag
		when 30 then p.BadAddressFlag
		when 40 then p.AltBadAddressFlag
	end
	
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

if (@flag is null)
	select @flag = 0

	RETURN @flag
	END
GO
/****** Object:  UserDefinedFunction [dbo].[PrimaryCity]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[PrimaryCity] ( @pid int )
RETURNS varchar(50)
AS
	BEGIN
declare @city varchar(50)
select @city =
	case AddressTypeId
			when 10 then f.CityName
			when 20 then f.AltCityName
			when 30 then p.CityName
			when 40 then p.AltCityName
	end
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

	RETURN @city
	END
GO
/****** Object:  UserDefinedFunction [dbo].[PrimaryResCode]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[PrimaryResCode]( @pid int )
RETURNS int
AS
	BEGIN
declare @rescodeid int
select @rescodeid =
	case AddressTypeId
		when 10 then f.ResCodeId
		when 20 then f.AltResCodeId
		when 30 then p.ResCodeId
		when 40 then p.AltResCodeId
	end
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

if @rescodeid is null
	select @rescodeid = 40

	RETURN @rescodeid
	END
GO
/****** Object:  UserDefinedFunction [dbo].[PrimaryState]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[PrimaryState] ( @pid int )
RETURNS varchar(5)
AS
	BEGIN
declare @st varchar(5)
select @st =
	case AddressTypeId
			when 10 then f.StateCode
			when 20 then f.AltStateCode
			when 30 then p.StateCode
			when 40 then p.AltStateCode
	end
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

	RETURN @st
	END
GO
/****** Object:  UserDefinedFunction [dbo].[PrimaryZip]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[PrimaryZip] ( @pid int )
RETURNS varchar(11)
AS
	BEGIN
declare @zip varchar(11)
select @zip =
	case AddressTypeId
			when 10 then f.ZipCode
			when 20 then f.AltZipCode
			when 30 then p.ZipCode
			when 40 then p.AltZipCode
	end
from dbo.People p join dbo.Families f on f.FamilyId = p.FamilyId
where PeopleId = @pid

	RETURN @zip
	END
GO
/****** Object:  UserDefinedFunction [dbo].[SchoolGrade]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[SchoolGrade] (@pid INT)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @g INT

SELECT TOP 1 @g = GradeRangeStart FROM dbo.Organizations o
WHERE EXISTS(
		SELECT NULL 
		FROM dbo.OrganizationMembers om 
		WHERE om.OrganizationId = o.OrganizationId
		AND om.PeopleId = @pid AND om.MemberTypeId = 220
		)
AND o.GradeRangeStart = o.GradeRangeEnd AND o.GradeRangeStart IS NOT NULL AND o.GradeRangeStart <> 0


	-- Return the result of the function
	RETURN @g

END
GO
/****** Object:  UserDefinedFunction [dbo].[SpouseId]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Michael Mayers
-- Create date: 
-- Description:	Determines Spouse people id
-- =============================================
CREATE FUNCTION [dbo].[SpouseId] 
(
	-- Add the parameters for the function here
	@peopleid int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int

	-- Add the T-SQL statements to compute the return value here
	SELECT TOP 1 @Result = s.PeopleId 
      FROM ( SELECT p1.FamilyId,
                    p1.MaritalStatusId,
                    p1.PositionInFamilyId
               FROM dbo.People p1
              WHERE p1.PeopleId =  @peopleid
                AND p1.MaritalStatusId = 20
                AND p1.DeceasedDate IS NULL
                AND p1.FirstName <> 'Duplicate'
           ) p
     INNER JOIN dbo.Families f
             ON f.FamilyId = p.FamilyId
     INNER JOIN dbo.People s
             ON s.FamilyId = f.FamilyId
     WHERE s.PeopleId <> @peopleid 
       AND p.MaritalStatusId = s.MaritalStatusId
       AND p.PositionInFamilyId = s.PositionInFamilyId
       AND s.DeceasedDate IS NULL
       AND s.FirstName <> 'Duplicate'

	-- Return the result of the function
	RETURN @Result

END
GO
/****** Object:  Table [dbo].[People]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[People](
	[PeopleId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[DropCodeId] [int] NOT NULL,
	[GenderId] [int] NOT NULL,
	[DoNotMailFlag] [bit] NOT NULL,
	[DoNotCallFlag] [bit] NOT NULL,
	[DoNotVisitFlag] [bit] NOT NULL,
	[AddressTypeId] [int] NOT NULL,
	[PhonePrefId] [int] NOT NULL,
	[MaritalStatusId] [int] NOT NULL,
	[PositionInFamilyId] [int] NOT NULL,
	[MemberStatusId] [int] NOT NULL,
	[FamilyId] [int] NOT NULL,
	[Grade]  AS ([dbo].[SchoolGrade]([PeopleId])),
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
	[JoinCodeId] [int] NOT NULL,
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
	[TitleCode] [varchar](10) NULL,
	[FirstName] [varchar](15) NOT NULL,
	[MiddleName] [varchar](15) NULL,
	[MaidenName] [varchar](20) NULL,
	[LastName] [varchar](20) NOT NULL,
	[SuffixCode] [varchar](10) NULL,
	[NickName] [varchar](15) NULL,
	[AddressLineOne] [varchar](40) NULL,
	[AddressLineTwo] [varchar](40) NULL,
	[CityName] [varchar](20) NULL,
	[StateCode] [varchar](20) NULL,
	[ZipCode] [varchar](15) NULL,
	[CountryName] [varchar](30) NULL,
	[StreetName] [varchar](40) NULL,
	[AltAddressLineOne] [varchar](40) NULL,
	[AltAddressLineTwo] [varchar](40) NULL,
	[AltCityName] [varchar](20) NULL,
	[AltStateCode] [varchar](20) NULL,
	[AltZipCode] [varchar](15) NULL,
	[AltCountryName] [varchar](30) NULL,
	[AltStreetName] [varchar](40) NULL,
	[CellPhone] [varchar](20) NULL,
	[WorkPhone] [varchar](20) NULL,
	[EmailAddress] [varchar](50) NULL,
	[OtherPreviousChurch] [varchar](60) NULL,
	[OtherNewChurch] [varchar](60) NULL,
	[SchoolOther] [varchar](60) NULL,
	[EmployerOther] [varchar](60) NULL,
	[OccupationOther] [varchar](40) NULL,
	[HobbyOther] [varchar](40) NULL,
	[SkillOther] [varchar](40) NULL,
	[InterestOther] [varchar](40) NULL,
	[LetterStatusNotes] [varchar](256) NULL,
	[Comments] [varchar](256) NULL,
	[ChristAsSavior] [bit] NOT NULL,
	[MemberAnyChurch] [bit] NULL,
	[InterestedInJoining] [bit] NOT NULL,
	[PleaseVisit] [bit] NOT NULL,
	[InfoBecomeAChristian] [bit] NOT NULL,
	[ContributionsStatement] [bit] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[PictureId] [int] NULL,
	[ContributionOptionsId] [int] NULL,
	[Age]  AS ([dbo].[Age]([PeopleId])),
	[PrimaryCity]  AS ([dbo].[PrimaryCity]([PeopleId])),
	[PrimaryZip]  AS ([dbo].[PrimaryZip]([PeopleId])),
	[PrimaryAddress]  AS ([dbo].[PrimaryAddress]([PeopleId])),
	[PrimaryState]  AS ([dbo].[PrimaryState]([PeopleId])),
	[HomePhone]  AS ([dbo].[HomePhone]([PeopleId])),
	[SpouseId]  AS ([dbo].[SpouseId]([PeopleId])),
	[PrimaryAddress2]  AS ([dbo].[PrimaryAddress2]([PeopleId])),
	[Name]  AS ((case when [Nickname]<>'' then [nickname] else [FirstName] end+' ')+[LastName]),
	[PrimaryResCode]  AS ([dbo].[PrimaryResCode]([PeopleId])),
	[PrimaryBadAddrFlag]  AS ([dbo].[PrimaryBadAddressFlag]([PeopleId])),
	[BibleFellowshipTeacher]  AS ([dbo].[BibleFellowshipTeacher]([PeopleId])),
	[PreferredName]  AS (case when [Nickname]<>'' then [nickname] else [FirstName] end+' '),
	[BibleFellowshipTeacherId]  AS ([dbo].[BibleFellowshipTeacherId]([PeopleId])),
	[BibleFellowshipClassId]  AS ([dbo].[BibleFellowshipClassId]([PeopleId])),
	[Name2]  AS (([LastName]+', ')+case when [Nickname]<>'' then [nickname] else [FirstName] end),
	[InBFClass]  AS ([dbo].[InBFClass]([PeopleId])),
	[LastContact]  AS ([dbo].[LastContact]([PeopleId])),
 CONSTRAINT [PEOPLE_PK] PRIMARY KEY NONCLUSTERED 
(
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PEOPLE_TBL] ON [dbo].[People] 
(
	[EmailAddress] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [PEOPLE_FAMILY_FK_IX] ON [dbo].[People] 
(
	[FamilyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[People] ON
INSERT [dbo].[People] ([PeopleId], [CreatedBy], [CreatedDate], [DropCodeId], [GenderId], [DoNotMailFlag], [DoNotCallFlag], [DoNotVisitFlag], [AddressTypeId], [PhonePrefId], [MaritalStatusId], [PositionInFamilyId], [MemberStatusId], [FamilyId], [BirthMonth], [BirthDay], [BirthYear], [OriginId], [EntryPointId], [InterestPointId], [BaptismTypeId], [BaptismStatusId], [DecisionTypeId], [DiscoveryClassStatusId], [NewMbrClassStatusId], [LetterStatusId], [JoinCodeId], [EnvelopeOptionsId], [BadAddressFlag], [AltBadAddressFlag], [ResCodeId], [AltResCodeId], [AddressFromDate], [AddressToDate], [AltAddressFromDate], [AltAddressToDate], [WeddingDate], [OriginDate], [BaptismSchedDate], [BaptismDate], [DecisionDate], [DiscoveryClassDate], [NewMbrClassDateCompleted], [LetterDateRequested], [LetterDateReceived], [JoinDate], [DropDate], [DeceasedDate], [TitleCode], [FirstName], [MiddleName], [MaidenName], [LastName], [SuffixCode], [NickName], [AddressLineOne], [AddressLineTwo], [CityName], [StateCode], [ZipCode], [CountryName], [StreetName], [AltAddressLineOne], [AltAddressLineTwo], [AltCityName], [AltStateCode], [AltZipCode], [AltCountryName], [AltStreetName], [CellPhone], [WorkPhone], [EmailAddress], [OtherPreviousChurch], [OtherNewChurch], [SchoolOther], [EmployerOther], [OccupationOther], [HobbyOther], [SkillOther], [InterestOther], [LetterStatusNotes], [Comments], [ChristAsSavior], [MemberAnyChurch], [InterestedInJoining], [PleaseVisit], [InfoBecomeAChristian], [ContributionsStatement], [ModifiedBy], [ModifiedDate], [PictureId], [ContributionOptionsId]) VALUES (1, 1, CAST(0x00009C0001776267 AS DateTime), 0, 0, 0, 0, 0, 10, 0, 0, 10, 50, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'The', NULL, NULL, N'Admin', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 0, 0, 0, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[People] OFF
/****** Object:  UserDefinedFunction [dbo].[UName]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[UName] (@pid int)
RETURNS varchar(100)
AS
BEGIN
	-- Declare the return variable here
	declare @name varchar(100)
	
	SELECT  @name = (case when [Nickname]<>'' then [nickname] else [FirstName] end) + ' ' + [LastName]
	FROM         dbo.People
	WHERE     PeopleId = @pid

	return @name

END
GO
/****** Object:  UserDefinedFunction [dbo].[UName2]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[UName2] (@pid int)
RETURNS varchar(100)
AS
BEGIN
	-- Declare the return variable here
	declare @name varchar(100)
	
	SELECT  @name = [LastName]+', '+(case when [Nickname]<>'' then [nickname] else [FirstName] end)
	FROM         dbo.People
	WHERE     PeopleId = @pid

	return @name

END
GO
/****** Object:  Table [dbo].[Users]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[PeopleId] [int] NULL,
	[Username] [varchar](50) NOT NULL,
	[Comment] [varchar](255) NULL,
	[Password] [varchar](128) NOT NULL,
	[PasswordQuestion] [varchar](255) NULL,
	[PasswordAnswer] [varchar](255) NULL,
	[IsApproved] [bit] NOT NULL,
	[LastActivityDate] [datetime] NULL,
	[LastLoginDate] [datetime] NULL,
	[LastPasswordChangedDate] [datetime] NULL,
	[CreationDate] [datetime] NULL,
	[IsLockedOut] [bit] NOT NULL,
	[LastLockedOutDate] [datetime] NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NULL,
	[EmailAddress] [varchar](50) NULL,
	[ItemsInGrid] [int] NULL,
	[CurrentCart] [varchar](100) NULL,
	[MustChangePassword] [bit] NOT NULL,
	[Host] [varchar](100) NULL,
	[TempPassword] [varchar](128) NULL,
	[Name]  AS ([dbo].[UName]([PeopleId])),
	[Name2]  AS ([dbo].[UName2]([PeopleId])),
 CONSTRAINT [PK_Users_1] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT [dbo].[Users] ([UserId], [PeopleId], [Username], [Comment], [Password], [PasswordQuestion], [PasswordAnswer], [IsApproved], [LastActivityDate], [LastLoginDate], [LastPasswordChangedDate], [CreationDate], [IsLockedOut], [LastLockedOutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [EmailAddress], [ItemsInGrid], [CurrentCart], [MustChangePassword], [Host], [TempPassword]) VALUES (1, 1, N'Admin', NULL, N'', NULL, NULL, 1, NULL, NULL, NULL, CAST(0x00009C000177624F AS DateTime), 0, NULL, 0, NULL, 0, NULL, N'bbcms01@bellevue.org', NULL, NULL, 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Table [dbo].[UserCanEmailFor]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserCanEmailFor](
	[UserId] [int] NOT NULL,
	[CanEmailFor] [int] NOT NULL,
 CONSTRAINT [PK_UserCanEmailFor] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[CanEmailFor] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TagType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagType](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TagTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[TagType] ([Id], [Name]) VALUES (1, N'Personal')
INSERT [dbo].[TagType] ([Id], [Name]) VALUES (3, N'CouplesHelper')
INSERT [dbo].[TagType] ([Id], [Name]) VALUES (4, N'AddSelected')
INSERT [dbo].[TagType] ([Id], [Name]) VALUES (5, N'OrgMembersOnly')
/****** Object:  Table [dbo].[Tag]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[TypeId] [int] NOT NULL,
	[Owner] [varchar](50) NULL,
	[Active] [bit] NULL,
	[PeopleId] [int] NULL,
	[OwnerName]  AS ([dbo].[UName]([PeopleId])),
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TagTag]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagTag](
	[Id] [int] NOT NULL,
	[ParentTagId] [int] NOT NULL,
 CONSTRAINT [PK_TagTag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[ParentTagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_TagTag] ON [dbo].[TagTag] 
(
	[ParentTagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Preferences]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Preferences](
	[UserId] [int] NOT NULL,
	[Preference] [varchar](20) NOT NULL,
	[Value] [varchar](50) NULL,
 CONSTRAINT [PK_UserPreferences] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[Preference] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [lookup].[WeeklySchedule]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[WeeklySchedule](
	[Id] [int] NOT NULL,
	[Description] [varchar](50) NULL,
	[Day] [int] NOT NULL,
	[MeetingTime] [datetime] NOT NULL,
	[Code] [varchar](10) NULL,
 CONSTRAINT [PK_WeeklySchedule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1001, N'Sunday  8:00 AM', 0, CAST(0x00009B540083D600 AS DateTime), N'Su0800')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1002, N'Sunday  9:30 AM', 0, CAST(0x00009B54009C8E20 AS DateTime), N'Su0930')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1003, N'Sunday  11:00 AM', 0, CAST(0x00009B5400B54640 AS DateTime), N'Su1100')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1006, N'Sunday  4:00 PM', 0, CAST(0x00009B540107AC00 AS DateTime), N'Su1600')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1009, N'Sunday  6:00 PM', 0, CAST(0x00009B540128A180 AS DateTime), N'Su1800')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1011, N'Monday 11:30 AM', 1, CAST(0x00009B5400BD83A0 AS DateTime), N'Mo1130')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1012, N'Monday 6:30 PM', 1, CAST(0x00009B540130DEE0 AS DateTime), N'Mo1830')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1014, N'Tuesday 9:00 AM', 2, CAST(0x00009B54009450C0 AS DateTime), N'Tu0900')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1015, N'Tuesday 6:00 PM', 2, CAST(0x00009B540128A180 AS DateTime), N'Tu1800')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1017, N'Tuesday 6:30 PM', 2, CAST(0x00009B540130DEE0 AS DateTime), N'Tu1830')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1018, N'Wednesday 9:30 AM', 3, CAST(0x00009B5401624F20 AS DateTime), N'We0930')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1019, N'Wednesday 10:00 AM', 3, CAST(0x00009B5400A4CB80 AS DateTime), N'We1000')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1020, N'Wednesday 4:30 PM', 3, CAST(0x00009B54010FE960 AS DateTime), N'We1630')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1021, N'Wednesday 6:00 PM', 3, CAST(0x00009B540128A180 AS DateTime), N'We1800')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1022, N'Wednesday 6:15 PM', 3, CAST(0x00009B54012CC030 AS DateTime), N'We1815')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1023, N'Wednesday 6:30 PM', 3, CAST(0x00009B540130DEE0 AS DateTime), N'We1830')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1024, N'Wednesday 7:00 PM', 3, CAST(0x00009B5401391C40 AS DateTime), N'We1900')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1025, N'Wednesday 7:30 PM', 3, CAST(0x00009B54007B98A0 AS DateTime), N'We1930')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1026, N'Thursday 9:30 AM', 4, CAST(0x00009B54009C8E20 AS DateTime), N'Th0930')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1027, N'Thursday 7:30 PM', 2, CAST(0x00009B54014159A0 AS DateTime), N'Th1930')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1028, N'Friday 10:00 AM', 5, CAST(0x00009B5400A4CB80 AS DateTime), N'Fr1000')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1029, N'Friday 1:30 PM', 5, CAST(0x00009B5400DE7920 AS DateTime), N'Fr1330')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1030, N'Friday 5:00 PM', 5, CAST(0x00009B54011826C0 AS DateTime), N'Fr1700')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1031, N'Friday 6:30 PM', 5, CAST(0x00009B540130DEE0 AS DateTime), N'Fr1830')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1032, N'Saturday 8:30 AM', 6, CAST(0x00009B54008C1360 AS DateTime), N'Sa0830')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1034, N'Saturday 6:00 PM', 6, CAST(0x00009B540128A180 AS DateTime), N'Sa1800')
INSERT [lookup].[WeeklySchedule] ([Id], [Description], [Day], [MeetingTime], [Code]) VALUES (1035, N'Sunday 2:00 PM', 0, CAST(0x00009B5400E6B680 AS DateTime), N'Su1400')
/****** Object:  Table [lookup].[OrganizationStatus]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[OrganizationStatus](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_OrganizationStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[OrganizationStatus] ([Id], [Code], [Description]) VALUES (10, N'C', N'Create')
INSERT [lookup].[OrganizationStatus] ([Id], [Code], [Description]) VALUES (20, N'R', N'Review')
INSERT [lookup].[OrganizationStatus] ([Id], [Code], [Description]) VALUES (30, N'A', N'Active')
INSERT [lookup].[OrganizationStatus] ([Id], [Code], [Description]) VALUES (40, N'I', N'Inactive')
/****** Object:  Table [lookup].[AttendTrackLevel]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[AttendTrackLevel](
	[Id] [int] NOT NULL,
	[Code] [varchar](10) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_AttendTrackLevel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[AttendTrackLevel] ([Id], [Code], [Description]) VALUES (0, N'N', N'None')
INSERT [lookup].[AttendTrackLevel] ([Id], [Code], [Description]) VALUES (10, N'HC', N'Headcount')
INSERT [lookup].[AttendTrackLevel] ([Id], [Code], [Description]) VALUES (20, N'I', N'Individual')
/****** Object:  UserDefinedFunction [dbo].[OrganizationLeaderId]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[OrganizationLeaderId](@orgid int)
RETURNS int
AS
BEGIN
	DECLARE @id int, @orgstatus int
	select @orgstatus = OrganizationStatusId 
	from dbo.Organizations
	where OrganizationId = @orgid
	if (@orgstatus = 40)
	SELECT top 1 @id = PeopleId from
                      dbo.EnrollmentTransaction et INNER JOIN
                      dbo.Organizations o ON 
                      et.OrganizationId = o.OrganizationId AND 
                      et.MemberTypeId = o.LeaderMemberTypeId
			where et.OrganizationId = @orgid
                      order by et.TransactionDate desc
	else
	SELECT top 1 @id = PeopleId from
                      dbo.OrganizationMembers om INNER JOIN
                      dbo.Organizations o ON 
                      om.OrganizationId = o.OrganizationId AND 
                      om.MemberTypeId = o.LeaderMemberTypeId
	where om.OrganizationId = @orgid
	ORDER BY om.EnrollmentDate
	RETURN @id

END
GO
/****** Object:  UserDefinedFunction [dbo].[OrganizationLeaderName]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[OrganizationLeaderName](@orgid int)
RETURNS varchar(100)
AS
BEGIN
	DECLARE @id int, @orgstatus int, @name varchar(100)
	select @orgstatus = OrganizationStatusId 
	from dbo.Organizations
	where OrganizationId = @orgid
	if (@orgstatus = 40)
	SELECT top 1 @id = PeopleId from
                      dbo.EnrollmentTransaction et INNER JOIN
                      dbo.Organizations o ON 
                      et.OrganizationId = o.OrganizationId AND 
                      et.MemberTypeId = o.LeaderMemberTypeId
			where et.OrganizationId = @orgid
                      order by et.TransactionDate desc
	else
	SELECT top 1 @id = PeopleId from
                      dbo.OrganizationMembers om INNER JOIN
                      dbo.Organizations o ON 
                      om.OrganizationId = o.OrganizationId AND 
                      om.MemberTypeId = o.LeaderMemberTypeId
	where om.OrganizationId = @orgid
	ORDER BY om.EnrollmentDate

	SELECT @name = (case when [Nickname]<>'' then [nickname] else [FirstName] end+' ')+[LastName] from dbo.People where PeopleId = @id
	RETURN @name
END
GO
/****** Object:  UserDefinedFunction [dbo].[OrganizationMemberCount]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[OrganizationMemberCount](@oid int) 
RETURNS int
AS
BEGIN
	DECLARE @c int
	SELECT @c = count(*) from dbo.OrganizationMembers 
	where OrganizationId = @oid
	RETURN @c
END
GO
/****** Object:  Table [dbo].[Organizations]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organizations](
	[OrganizationId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[OrganizationStatusId] [int] NOT NULL,
	[DivisionId] [int] NOT NULL,
	[LeaderMemberTypeId] [int] NULL,
	[RollSheetVisitorWks] [int] NULL,
	[AttendTrkLevelId] [int] NOT NULL,
	[SecurityTypeId] [int] NOT NULL,
	[FirstMeetingDate] [datetime] NULL,
	[LastMeetingDate] [datetime] NULL,
	[OrganizationClosedDate] [datetime] NULL,
	[Location] [varchar](40) NULL,
	[OrganizationName] [varchar](60) NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ScheduleId] [int] NULL,
	[EntryPointId] [int] NULL,
	[ParentOrgId] [int] NULL,
	[AllowAttendOverlap] [bit] NOT NULL,
	[MemberCount]  AS ([dbo].[OrganizationMemberCount]([OrganizationId])),
	[LeaderId]  AS ([dbo].[OrganizationLeaderId]([OrganizationId])),
	[LeaderName]  AS ([dbo].[organizationLeaderName]([OrganizationId])),
	[AttendClassificationId] [int] NOT NULL,
	[GradeRangeStart] [int] NULL,
	[GradeRangeEnd] [int] NULL,
 CONSTRAINT [ORGANIZATIONS_PK] PRIMARY KEY NONCLUSTERED 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ORGANIZATIONS_TBL] ON [dbo].[Organizations] 
(
	[ScheduleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Organizations] ON
INSERT [dbo].[Organizations] ([OrganizationId], [CreatedBy], [CreatedDate], [OrganizationStatusId], [DivisionId], [LeaderMemberTypeId], [RollSheetVisitorWks], [AttendTrkLevelId], [SecurityTypeId], [FirstMeetingDate], [LastMeetingDate], [OrganizationClosedDate], [Location], [OrganizationName], [ModifiedBy], [ModifiedDate], [ScheduleId], [EntryPointId], [ParentOrgId], [AllowAttendOverlap], [AttendClassificationId], [GradeRangeStart], [GradeRangeEnd]) VALUES (1, 1, CAST(0x00009C000177626B AS DateTime), 30, 1, NULL, NULL, 20, 0, NULL, NULL, NULL, NULL, N'First Organization', NULL, NULL, 1001, NULL, NULL, 0, 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Organizations] OFF
/****** Object:  Table [dbo].[TagOrg]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagOrg](
	[Id] [int] NOT NULL,
	[OrganizationId] [int] NOT NULL,
 CONSTRAINT [PK_TagOrg] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[OrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_TagOrg] ON [dbo].[TagOrg] 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [lookup].[MemberType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[MemberType](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
	[AttendanceTypeId] [int] NOT NULL,
 CONSTRAINT [PK_MemberType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (100, N'AD', N'Administrator', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (101, N'PR', N'President', 10)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (102, N'VP', N'Vice President', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (103, N'DR', N'Director', 10)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (104, N'C', N'Co-director', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (107, N'GO', N'General Officer', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (109, N'F', N'Facilitator', 10)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (110, N'RG', N'Registrar', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (111, N'CT', N'Staff Consultant', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (120, N'YT', N'Youth', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (122, N'PN', N'Pianist', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (125, N'SL', N'Section Leader', 10)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (130, N'CH', N'Chairman', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (131, N'SO', N'Social Chairman', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (132, N'VC', N'Vice-Chairman', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (135, N'IS', N'Instructor', 10)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (136, N'CC', N'Coach', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (140, N'L', N'Leader', 10)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (142, N'AL', N'Assistant Leader', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (143, N'GL', N'Group Leader', 10)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (145, N'CR', N'Coordinator', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (146, N'RL', N'Recreation Leader', 10)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (150, N'GR', N'Greeter', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (155, N'WK', N'Worker', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (160, N'T', N'Teacher', 10)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (161, N'AT', N'Assistant Teacher', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (162, N'SC', N'Secretary', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (163, N'AS', N'Assistant Secretary', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (166, N'CF', N'Craft Teacher', 10)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (167, N'CE', N'Co Teacher', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (170, N'IR', N'In Reach Leader', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (171, N'AI', N'Asst In Reach Ldr', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (172, N'OR', N'Outreach Leader', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (220, N'M', N'Member', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (230, N'IA', N'InActive', 40)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (280, N'AN', N'Deacon New', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (281, N'AR', N'Deacon Returning', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (300, N'VM', N'Visiting Member', 30)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (310, N'VS', N'Visitor', 60)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (400, N'IL', N'Independent Living', 100)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (405, N'LV', N'Assisted Living', 100)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (410, N'IH', N'In Home', 100)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (415, N'HB', N'Homebound', 100)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (420, N'NH', N'Nursing Home', 100)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (500, N'IM', N'In-Service Member', 70)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (700, N'VI', N'VIP', 20)
INSERT [lookup].[MemberType] ([Id], [Code], [Description], [AttendanceTypeId]) VALUES (710, N'VL', N'Volunteer', 20)
/****** Object:  Table [dbo].[OrganizationMembers]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationMembers](
	[OrganizationId] [int] NOT NULL,
	[PeopleId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[MemberTypeId] [int] NOT NULL,
	[EnrollmentDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[VipWeek1] [bit] NULL,
	[VipWeek2] [bit] NULL,
	[VipWeek3] [bit] NULL,
	[VipWeek4] [bit] NULL,
	[VipWeek5] [bit] NULL,
	[InactiveDate] [datetime] NULL,
	[AttendStr] [varchar](200) NULL,
	[AttendPct] [real] NULL,
	[LastAttended] [datetime] NULL,
 CONSTRAINT [ORGANIZATION_MEMBERS_PK] PRIMARY KEY NONCLUSTERED 
(
	[OrganizationId] ASC,
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ORGANIZATION_MEMBERS_PPL_FK_IX] ON [dbo].[OrganizationMembers] 
(
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[WasDeaconActive2008]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[WasDeaconActive2008](@pid INT, @dt DATETIME)
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	DECLARE @r BIT 

	IF EXISTS(SELECT NULL FROM dbo.OrganizationMembers o WHERE o.OrganizationId = 80063 AND @pid = o.PeopleId) AND @dt > '1/1/2008' AND @dt < '11/1/2008'
		SELECT @r = 0
	ELSE IF EXISTS(SELECT NULL FROM dbo.OrganizationMembers o2 WHERE o2.OrganizationId = 80092 AND o2.PeopleId = @pid)
		SELECT @r = 1
	ELSE
		SELECT @r = 0

	-- Return the result of the function
	RETURN @r

END
GO
/****** Object:  Table [lookup].[AttendType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[AttendType](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_AttendType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (10, N'L', N'Leader')
INSERT [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (20, N'VO', N'Volunteer')
INSERT [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (30, N'M', N'Member')
INSERT [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (40, N'VM', N'Visiting Member')
INSERT [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (50, N'RV', N'Recent Visitor')
INSERT [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (60, N'NV', N'New Visitor')
INSERT [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (70, N'ISM', N'In-Service')
INSERT [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (80, N'OFS', N'Offsite')
INSERT [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (90, N'GRP', N'Group')
INSERT [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (100, N'HMB', N'Homebound')
INSERT [lookup].[AttendType] ([Id], [Code], [Description]) VALUES (110, N'OC', N'Other Class')
/****** Object:  Table [dbo].[Meetings]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meetings](
	[MeetingId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[NumPresent] [int] NOT NULL,
	[NumMembers] [int] NOT NULL,
	[NumVstMembers] [int] NOT NULL,
	[NumRepeatVst] [int] NOT NULL,
	[NumNewVisit] [int] NOT NULL,
	[Location] [varchar](25) NULL,
	[MeetingDate] [datetime] NULL,
	[GroupMeetingFlag] [bit] NOT NULL,
 CONSTRAINT [MEETINGS_PK] PRIMARY KEY NONCLUSTERED 
(
	[MeetingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Meetings_MeetingDate] ON [dbo].[Meetings] 
(
	[MeetingDate] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_MEETINGS_ORG_ID] ON [dbo].[Meetings] 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Attend]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attend](
	[PeopleId] [int] NOT NULL,
	[MeetingId] [int] NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[MeetingDate] [datetime] NOT NULL,
	[AttendanceFlag] [bit] NOT NULL,
	[OtherOrgId] [int] NULL,
	[AttendanceTypeId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[MemberTypeId] [int] NOT NULL,
	[AttendId] [int] IDENTITY(1,1) NOT NULL,
	[OtherAttends] [int] NOT NULL,
	[BFCAttendance] [bit] NULL,
	[EffAttendFlag]  AS (CONVERT([bit],case when [AttendanceTypeId]=(90) then NULL when [AttendanceFlag]=(1) AND [OtherAttends]>(0) AND [BFCAttendance]=(1) then NULL when [AttendanceFlag]=(1) then (1) when [OtherAttends]>(0) then NULL else (0) end,(0))),
 CONSTRAINT [PK_Attend] PRIMARY KEY CLUSTERED 
(
	[AttendId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Attend] ON [dbo].[Attend] 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Attend_1] ON [dbo].[Attend] 
(
	[MeetingDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Attend_2] ON [dbo].[Attend] 
(
	[MeetingId] ASC,
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Attend_3] ON [dbo].[Attend] 
(
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Attend_4] ON [dbo].[Attend] 
(
	[MeetingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[UpdateAttendStr]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateAttendStr] @orgid INT, @pid INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-----------------------------------------------------------------
    DECLARE @yearago DATETIME
    DECLARE @lastmeet DATETIME 
    DECLARE @tct INT 
    DECLARE @act INT
    DECLARE @pct REAL
		
    SELECT @lastmeet = MAX(MeetingDate) FROM dbo.Meetings
    WHERE OrganizationId = @orgid
    
    SELECT @yearago = DATEADD(year,-1,@lastmeet)
    
	SELECT @tct = COUNT(*) FROM dbo.Attend
     WHERE PeopleId = @pid
       AND OrganizationId = @orgid
       AND EffAttendFlag IS NOT NULL
       AND MeetingDate >= @yearago
       
    SELECT @act = COUNT(*) FROM dbo.Attend
     WHERE PeopleId = @pid
       AND OrganizationId = @orgid
       AND EffAttendFlag = 1
       AND MeetingDate >= @yearago
       
       
	if @tct = 0
			select @pct = 0
		else
			SELECT @pct = @act * 100.0 / @tct
			
	-------------------------------------------------------
	DECLARE @a nvarchar(200) -- attendance string
	set @a = ''
			
	DECLARE @mindt DATETIME, @dt DATETIME 

	SELECT @mindt = MIN(MeetingDate), @dt = MAX(MeetingDate) FROM dbo.Meetings WHERE OrganizationId = @orgid
	SELECT @dt = DATEADD(yy, -1, @dt)
	IF (@dt > @mindt)
		SELECT @mindt = @dt
		FROM dbo.Attend
		WHERE OrganizationId = @orgid AND PeopleId = @pid

	SELECT @a = 
		CASE 
		WHEN a.EffAttendFlag IS NULL THEN
			CASE a.AttendanceTypeId
			WHEN 20 THEN 'V'
			WHEN 70 THEN 'I'
			WHEN 90 THEN 'G'
			WHEN 80 THEN 'O'
			WHEN 110 THEN '*'
			ELSE '*'
			END
		WHEN a.EffAttendFlag = 1 THEN 'P'
		ELSE '.'
		END + @a
	FROM dbo.Attend a
	WHERE a.MeetingDate >= @dt AND a.PeopleId = @pid AND a.OrganizationId = @orgid
	ORDER BY MeetingDate DESC
	
	----------------------------------------------------------------
	DECLARE @lastattend DATETIME
	SELECT @lastattend = MAX(a.MeetingDate) FROM dbo.Attend a
	WHERE a.AttendanceFlag = 1 AND a.OrganizationId = @orgid AND a.PeopleId = @pid

	
	--------------------------------------------	
	
		
	UPDATE dbo.OrganizationMembers SET
		AttendPct = @pct,
		AttendStr = @a,
		LastAttended = @lastattend
	WHERE OrganizationId = @orgid AND PeopleId = @pid

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateAllAttendStr]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateAllAttendStr]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		DECLARE cur CURSOR FOR SELECT OrganizationId, PeopleId FROM dbo.OrganizationMembers
		OPEN cur
		DECLARE @oid INT, @pid INT, @n INT
		SET @n = 0
		FETCH NEXT FROM cur INTO @oid, @pid
		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXECUTE dbo.UpdateAttendStr @oid, @pid
			SET @n = @n + 1
			IF (@n % 1000) = 0
				RAISERROR ('%d', 0, 1, @n) WITH NOWAIT
			FETCH NEXT FROM cur INTO @oid, @pid
		END
		CLOSE cur
		DEALLOCATE cur
END
GO
/****** Object:  Table [lookup].[VolApplicationStatus]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[VolApplicationStatus](
	[Id] [int] NOT NULL,
	[Code] [varchar](10) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_VolApplicationStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[VolApplicationStatus] ([Id], [Code], [Description]) VALUES (0, N'UK', N'(not specified)')
INSERT [lookup].[VolApplicationStatus] ([Id], [Code], [Description]) VALUES (10, N'Appr', N'Approved')
INSERT [lookup].[VolApplicationStatus] ([Id], [Code], [Description]) VALUES (20, N'WD', N'Withdrawn')
INSERT [lookup].[VolApplicationStatus] ([Id], [Code], [Description]) VALUES (30, N'Not', N'Not Approved')
INSERT [lookup].[VolApplicationStatus] ([Id], [Code], [Description]) VALUES (40, N'Pend', N'Pending')
/****** Object:  Table [dbo].[Volunteer]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Volunteer](
	[PeopleId] [int] NOT NULL,
	[StatusId] [int] NULL,
	[ProcessedDate] [datetime] NULL,
	[Standard] [bit] NOT NULL,
	[Children] [bit] NOT NULL,
	[Leader] [bit] NOT NULL,
	[Comments] [varchar](max) NULL,
 CONSTRAINT [PK_VolunteerApproval] PRIMARY KEY CLUSTERED 
(
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VolunteerForm]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VolunteerForm](
	[PeopleId] [int] NOT NULL,
	[AppDate] [datetime] NULL,
	[LargeId] [int] NULL,
	[MediumId] [int] NULL,
	[SmallId] [int] NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UploaderId] [int] NULL,
	[IsDocument] [bit] NULL,
 CONSTRAINT [PK_VolunteerForm] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_VolunteerForm] ON [dbo].[VolunteerForm] 
(
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VBSApp]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VBSApp](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PeopleId] [int] NULL,
	[ImgId] [int] NULL,
	[IsDocument] [bit] NULL,
	[Uploaded] [datetime] NULL,
	[Request] [varchar](50) NULL,
	[ActiveInAnotherChurch] [bit] NULL,
	[GradeCompleted] [varchar](15) NULL,
	[OrgId] [int] NULL,
	[DivId] [int] NULL,
	[Inactive] [bit] NULL,
	[PubPhoto] [bit] NULL,
	[UserInfo] [varchar](15) NULL,
	[MedAllergy] [bit] NULL,
 CONSTRAINT [PK_VBSApp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[UserName]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[UserName] (@pid int)
RETURNS varchar(100)
AS
	BEGIN
	declare @name varchar(100)
	
SELECT  @name = [LastName]+', '+(case when [Nickname]<>'' then [nickname] else [FirstName] end)
FROM         dbo.People
WHERE     PeopleId = @pid

	return @name
	END
GO
/****** Object:  Table [dbo].[TagShare]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagShare](
	[TagId] [int] NOT NULL,
	[PeopleId] [int] NOT NULL,
 CONSTRAINT [PK_TagShare] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC,
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TagPerson]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagPerson](
	[Id] [int] NOT NULL,
	[PeopleId] [int] NOT NULL,
 CONSTRAINT [PK_TagPeople] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_TagPerson] ON [dbo].[TagPerson] 
(
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[TaggedPeople]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[TaggedPeople](@tagid INT) 
RETURNS @t TABLE ( PeopleId INT)
AS
BEGIN
	INSERT INTO @t (PeopleId)
	SELECT p.PeopleId FROM dbo.People p
	WHERE EXISTS(
    SELECT NULL
    FROM dbo.TagPerson t
    WHERE (t.Id = @tagid) AND (t.PeopleId = p.PeopleId))
    RETURN
END
GO
/****** Object:  Table [dbo].[ActivityLog]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ActivityDate] [datetime] NULL,
	[UserId] [int] NULL,
	[Activity] [varchar](100) NULL,
	[PageUrl] [varchar](300) NULL,
 CONSTRAINT [PK_alog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[LinkEnrollmentTransaction]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[LinkEnrollmentTransaction] (@tid INT, @trandt DATETIME, @typeid INT, @orgid INT, @pid int)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @etid INT -- Find the original enrollment transaction
	SELECT TOP 1 @etid = TransactionId
	FROM dbo.EnrollmentTransaction
	WHERE TransactionTypeId <= 2
		AND PeopleId = @pid
		AND OrganizationId = @orgid
		AND TransactionId < @tid
		AND @typeid >= 3
	ORDER  BY  TransactionId DESC 

	-- point the current transction to the original enrollment
	UPDATE dbo.EnrollmentTransaction
	SET EnrollmentTransactionId = @etid
	WHERE TransactionId = @tid AND @etid IS NOT NULL

	DECLARE @previd INT -- find previous transaction
	
	SELECT TOP 1 @previd = TransactionId
	FROM dbo.EnrollmentTransaction
	WHERE TransactionTypeId <= 3
		AND @typeid >= 3
		AND PeopleId = @pid
		AND OrganizationId = @orgid
		AND TransactionId < @tid
	ORDER BY TransactionId DESC
	
	-- set the previous transaction's next tran date
	UPDATE dbo.EnrollmentTransaction
	SET NextTranChangeDate = @trandt
	WHERE TransactionId = @previd
	
END
GO
/****** Object:  Table [dbo].[EnrollmentTransaction]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnrollmentTransaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[TransactionStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[TransactionDate] [datetime] NOT NULL,
	[TransactionTypeId] [int] NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[OrganizationName] [varchar](60) NOT NULL,
	[PeopleId] [int] NOT NULL,
	[MemberTypeId] [int] NOT NULL,
	[EnrollmentDate] [datetime] NULL,
	[AttendancePercentage] [numeric](5, 3) NULL,
	[VipWeek1] [bit] NULL,
	[VipWeek2] [bit] NULL,
	[VipWeek3] [bit] NULL,
	[VipWeek4] [bit] NULL,
	[VipWeek5] [bit] NULL,
	[NextTranChangeDate] [datetime] NULL,
	[EnrollmentTransactionId] [int] NULL,
 CONSTRAINT [ENROLLMENT_TRANSACTION_PK] PRIMARY KEY NONCLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>] ON [dbo].[EnrollmentTransaction] 
(
	[TransactionDate] ASC,
	[TransactionTypeId] ASC
)
INCLUDE ( [OrganizationId],
[PeopleId],
[TransactionId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ENROLLMENT_TRANS_ORG_TC_TS_IX] ON [dbo].[EnrollmentTransaction] 
(
	[OrganizationId] ASC,
	[TransactionTypeId] ASC,
	[TransactionStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ENROLLMENT_TRANSACTION_ORG_IX] ON [dbo].[EnrollmentTransaction] 
(
	[OrganizationId] ASC,
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ENROLLMENT_TRANSACTION_PPL_IX] ON [dbo].[EnrollmentTransaction] 
(
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ENROLLMENT_TRANSACTION_TBL] ON [dbo].[EnrollmentTransaction] 
(
	[TransactionDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BadET]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BadET](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[PeopleId] [int] NOT NULL,
	[OrgId] [int] NULL,
	[TranId] [int] NOT NULL,
	[Flag] [int] NOT NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_BadET] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BadET] ON [dbo].[BadET] 
(
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_BadET_1] ON [dbo].[BadET] 
(
	[TranId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContributionFund]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContributionFund](
	[FundId] [int] IDENTITY(1,1) NOT NULL,
	[ChurchId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[RecordStatus] [bit] NOT NULL,
	[FundName] [varchar](40) NOT NULL,
	[FundDescription] [varchar](256) NULL,
	[FundStatusId] [int] NOT NULL,
	[FundTypeId] [int] NOT NULL,
	[FundPledgeFlag] [bit] NOT NULL,
	[FundTarget] [numeric](11, 2) NULL,
	[FundOpenDate] [datetime] NOT NULL,
	[FundCloseDate] [datetime] NULL,
	[FundReopenDate] [datetime] NULL,
	[FundDropDate] [datetime] NULL,
	[FundAccountCode] [int] NULL,
	[FundIncomeDept] [varchar](25) NOT NULL,
	[FundIncomeAccount] [varchar](25) NOT NULL,
	[FundIncomeFund] [varchar](25) NOT NULL,
	[FundCashDept] [varchar](25) NOT NULL,
	[FundCashAccount] [varchar](25) NOT NULL,
	[FundCashFund] [varchar](25) NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [FUND_PK] PRIMARY KEY NONCLUSTERED 
(
	[FundId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [FUND_CHURCH_FK_IX] ON [dbo].[ContributionFund] 
(
	[ChurchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [FUND_NAME_IX] ON [dbo].[ContributionFund] 
(
	[FundName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [lookup].[ContributionStatus]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[ContributionStatus](
	[Id] [int] NOT NULL,
	[Code] [varchar](5) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_ContributionStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[ContributionStatus] ([Id], [Code], [Description]) VALUES (0, N'C', N'Recorded')
INSERT [lookup].[ContributionStatus] ([Id], [Code], [Description]) VALUES (1, N'V', N'Reversed')
INSERT [lookup].[ContributionStatus] ([Id], [Code], [Description]) VALUES (2, N'R', N'Returned')
/****** Object:  Table [lookup].[ContributionType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[ContributionType](
	[Id] [int] NOT NULL,
	[Code] [varchar](5) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_ContributionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[ContributionType] ([Id], [Code], [Description]) VALUES (1, N'CC', N'Check/Cash')
INSERT [lookup].[ContributionType] ([Id], [Code], [Description]) VALUES (3, N'P', N'Brokered Property')
INSERT [lookup].[ContributionType] ([Id], [Code], [Description]) VALUES (4, N'G', N'Grave Site')
INSERT [lookup].[ContributionType] ([Id], [Code], [Description]) VALUES (6, N'RC', N'Returned Check')
INSERT [lookup].[ContributionType] ([Id], [Code], [Description]) VALUES (7, N'RV', N'Reversed')
INSERT [lookup].[ContributionType] ([Id], [Code], [Description]) VALUES (8, N'PL', N'Pledge')
/****** Object:  Table [dbo].[Contribution]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contribution](
	[ContributionId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[FundId] [int] NOT NULL,
	[ContributionTypeId] [int] NOT NULL,
	[PeopleId] [int] NULL,
	[ContributionDate] [datetime] NULL,
	[ContributionAmount] [numeric](11, 2) NULL,
	[ContributionDesc] [varchar](256) NULL,
	[ContributionStatusId] [int] NULL,
	[PledgeFlag] [bit] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[PostingDate] [datetime] NULL,
 CONSTRAINT [CONTRIBUTION_PK] PRIMARY KEY NONCLUSTERED 
(
	[ContributionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CONTRIBUTION_DATE_IX] ON [dbo].[Contribution] 
(
	[ContributionDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CONTRIBUTION_FUND_FK_IX] ON [dbo].[Contribution] 
(
	[FundId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CONTRIBUTION_PEOPLE_FK_IX] ON [dbo].[Contribution] 
(
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ContributionTypeId] ON [dbo].[Contribution] 
(
	[ContributionTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_INDIVIDUAL_CONTRIBUTION_TBL_1] ON [dbo].[Contribution] 
(
	[ContributionStatusId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_INDIVIDUAL_CONTRIBUTION_TBL_2] ON [dbo].[Contribution] 
(
	[PledgeFlag] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contactees]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contactees](
	[ContactId] [int] NOT NULL,
	[PeopleId] [int] NOT NULL,
	[ProfessionOfFaith] [bit] NULL,
	[PrayedForPerson] [bit] NULL,
 CONSTRAINT [PK_Contactees] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC,
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contactors]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contactors](
	[ContactId] [int] NOT NULL,
	[PeopleId] [int] NOT NULL,
 CONSTRAINT [PK_Contactors] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC,
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[CoupleFlag]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kenny
-- Create date: 4/17/2008
-- Description:	Finds the Couple Flag given a family ID
-- This flag is used for figuring out how mailings should
-- be addressed (see codes below)
-- =============================================

CREATE FUNCTION [dbo].[CoupleFlag] 
(
	@family_id int
)

-- Returns:
--  0 - Individual
--  1 - Couple
--  2 - Couple + Family
--  3 - Single Parent + Family
--  4 - No Primary Family

RETURNS int
AS
BEGIN
	DECLARE @Result int

    SELECT top 1 @Result = 
        case (sum(case PositionInFamilyId when 10 then 1 else 0 end))
            when 2 then (case count(*) when 2 then (case min(MaritalStatusId) when 30 then (CASE MAX(MaritalStatusId) WHEN 30 THEN 1 ELSE 4 end) else 4 end) else 2 end)
            when 1 then (case count(*) when 1 then 0 else 3 end)
            else (case count(*) when 1 then 0 else 4 end)
        end
      FROM dbo.People
     WHERE FamilyId = @family_id
       AND DeceasedDate IS NULL
       AND FirstName <> 'Duplicate'

	-- Return the result of the function
	RETURN @Result

END
GO
/****** Object:  UserDefinedFunction [dbo].[GetEldestFamilyMember]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kenny
-- Create date: 4/17/2008
-- Description:	Finds the eldest member given the family ID
-- =============================================
CREATE FUNCTION [dbo].[GetEldestFamilyMember]( @fid int )
RETURNS int
AS
BEGIN
    DECLARE @Result int

    select @Result = PeopleId
      from dbo.People
     where FamilyId = @fid
       and dbo.Age(PeopleId) = (select max(dbo.Age(PeopleId))
                    from dbo.People
                   where FamilyId = @fid)
     
	RETURN @Result
END
GO
/****** Object:  UserDefinedFunction [dbo].[HeadOfHouseholdId]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kenny
-- Create date: 4/17/2008
-- Description:	Finds the Head of Household given a family ID
-- =============================================
CREATE FUNCTION [dbo].[HeadOfHouseholdId] 
(
	@family_id int
)
RETURNS int
AS
BEGIN
	DECLARE @Result int

    SELECT top 1 @Result = 
        isnull(case (sum(case PositionInFamilyId when 10 then 1 else 0 end))
		          when 2 then isnull( min(case PositionInFamilyId when 10 then
									        case GenderId when 1 then PeopleId else null end
								          else
									        null
								          end),
							          min(case PositionInFamilyId when 10 then
                                            case GenderId when 2 then PeopleId else null end
								          else
                                            null
                                          end))
		          when 1 then isnull( min(case PositionInFamilyId when 10 then
									        case GenderId when 1 then PeopleId else null end
								          else
									        null
								          end),
							          isnull(min(case PositionInFamilyId when 10 then
											        case GenderId when 2 then PeopleId else null end
										         else
											        null
										         end),
								             min(case PositionInFamilyId when 10 then
											        case GenderId when 0 then PeopleId else null end
										         else
											        null
										         end)))
		        end, dbo.GetEldestFamilyMember(@family_id))
      FROM dbo.People
     WHERE FamilyId = @family_id
       AND DeceasedDate IS NULL
       AND FirstName <> 'Duplicate'

	-- Return the result of the function
	RETURN @Result

END
GO
/****** Object:  UserDefinedFunction [dbo].[HeadOfHouseHoldSpouseId]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kenny
-- Create date: 4/17/2008
-- Description:	Finds the Head of Household Spouse ID given a family ID
-- =============================================
CREATE FUNCTION [dbo].[HeadOfHouseHoldSpouseId] 
(
	@family_id int
)

RETURNS int
AS
BEGIN
	DECLARE @Result int

    SELECT top 1 @Result = 
           dbo.SpouseId(PeopleId)
      FROM dbo.People
     WHERE FamilyId = @family_id

	-- Return the result of the function
	RETURN @Result

END
GO
/****** Object:  Table [dbo].[Families]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Families](
	[FamilyId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[RecordStatus] [bit] NOT NULL,
	[BadAddressFlag] [bit] NULL,
	[AltBadAddressFlag] [bit] NULL,
	[ResCodeId] [int] NULL,
	[AltResCodeId] [int] NULL,
	[AddressFromDate] [datetime] NULL,
	[AddressToDate] [datetime] NULL,
	[AltAddressFromDate] [datetime] NULL,
	[AltAddressToDate] [datetime] NULL,
	[AddressLineOne] [varchar](40) NULL,
	[AddressLineTwo] [varchar](40) NULL,
	[CityName] [varchar](20) NULL,
	[StateCode] [varchar](20) NULL,
	[ZipCode] [varchar](15) NULL,
	[CountryName] [varchar](30) NULL,
	[StreetName] [varchar](40) NULL,
	[AltAddressLineOne] [varchar](40) NULL,
	[AltAddressLineTwo] [varchar](40) NULL,
	[AltCityName] [varchar](20) NULL,
	[AltStateCode] [varchar](20) NULL,
	[AltZipCode] [varchar](15) NULL,
	[AltCountryName] [varchar](30) NULL,
	[AltStreetName] [varchar](40) NULL,
	[HomePhone] [varchar](20) NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[HeadOfHouseholdId]  AS ([dbo].[HeadOfHouseholdId]([FamilyId])),
	[HeadOfHouseholdSpouseId]  AS ([dbo].[HeadOfHouseholdSpouseId]([FamilyId])),
	[CoupleFlag]  AS ([dbo].[CoupleFlag]([FamilyId])),
 CONSTRAINT [FAMILIES_PK] PRIMARY KEY NONCLUSTERED 
(
	[FamilyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Families] ON
INSERT [dbo].[Families] ([FamilyId], [CreatedBy], [CreatedDate], [RecordStatus], [BadAddressFlag], [AltBadAddressFlag], [ResCodeId], [AltResCodeId], [AddressFromDate], [AddressToDate], [AltAddressFromDate], [AltAddressToDate], [AddressLineOne], [AddressLineTwo], [CityName], [StateCode], [ZipCode], [CountryName], [StreetName], [AltAddressLineOne], [AltAddressLineTwo], [AltCityName], [AltStateCode], [AltZipCode], [AltCountryName], [AltStreetName], [HomePhone], [ModifiedBy], [ModifiedDate]) VALUES (1, 1, CAST(0x00009C0001776267 AS DateTime), 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Families] OFF
/****** Object:  Table [dbo].[RelatedFamilies]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RelatedFamilies](
	[FamilyId] [int] NOT NULL,
	[RelatedFamilyId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[FamilyRelationshipDesc] [varchar](256) NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [RELATED_FAMILIES_PK] PRIMARY KEY NONCLUSTERED 
(
	[FamilyId] ASC,
	[RelatedFamilyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [RELATED_FAMILIES_RELATED_FK_IX] ON [dbo].[RelatedFamilies] 
(
	[RelatedFamilyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskList]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_TaskList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[TaskList] ON
INSERT [dbo].[TaskList] ([Id], [CreatedBy], [Name]) VALUES (1, 0, N'InBox')
INSERT [dbo].[TaskList] ([Id], [CreatedBy], [Name]) VALUES (2, 0, N'Personal')
SET IDENTITY_INSERT [dbo].[TaskList] OFF
/****** Object:  Table [dbo].[TaskListOwners]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskListOwners](
	[TaskListId] [int] NOT NULL,
	[PeopleId] [int] NOT NULL,
 CONSTRAINT [PK_TaskListOwners] PRIMARY KEY CLUSTERED 
(
	[TaskListId] ASC,
	[PeopleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [lookup].[TaskStatus]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[TaskStatus](
	[Id] [int] NOT NULL,
	[Code] [varchar](50) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_TaskStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[TaskStatus] ([Id], [Code], [Description]) VALUES (10, N'A', N'Active')
INSERT [lookup].[TaskStatus] ([Id], [Code], [Description]) VALUES (20, N'W', N'Waiting For')
INSERT [lookup].[TaskStatus] ([Id], [Code], [Description]) VALUES (30, N'S', N'Someday')
INSERT [lookup].[TaskStatus] ([Id], [Code], [Description]) VALUES (40, N'C', N'Completed')
INSERT [lookup].[TaskStatus] ([Id], [Code], [Description]) VALUES (50, N'P', N'Pending Acceptance')
INSERT [lookup].[TaskStatus] ([Id], [Code], [Description]) VALUES (60, N'R', N'ReDelegated')
/****** Object:  Table [dbo].[Task]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OwnerId] [int] NOT NULL,
	[ListId] [int] NOT NULL,
	[CoOwnerId] [int] NULL,
	[CoListId] [int] NULL,
	[StatusId] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[SourceContactId] [int] NULL,
	[CompletedContactId] [int] NULL,
	[Notes] [varchar](max) NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[Project] [varchar](50) NULL,
	[Archive] [bit] NOT NULL,
	[Priority] [int] NULL,
	[WhoId] [int] NULL,
	[Due] [datetime] NULL,
	[Location] [varchar](50) NULL,
	[Description] [varchar](100) NULL,
	[CompletedOn] [datetime] NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Task] ON [dbo].[Task] 
(
	[OwnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[UpdateMeetingCounters]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateMeetingCounters](@mid INT)
AS
BEGIN
	DECLARE @gf BIT, @numPresent INT, @numNewVisit INT, @numMembers INT, @numVstMembers INT, @numRepeatVst INT
	SELECT @gf = GroupMeetingFlag FROM dbo.Meetings WHERE MeetingId = @mid
	IF @gf = 0
	BEGIN
		SELECT @numPresent = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1
		SELECT @numNewVisit = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId = 60
		SELECT @numMembers = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId IN (10,20,30)
		SELECT @numVstMembers = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId = 40
		SELECT @numRepeatVst = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId = 50
		UPDATE dbo.Meetings SET
			NumMembers = @numMembers,
			NumPresent = @numPresent,
			NumNewVisit = @numNewVisit,
			NumVstMembers = @numVstMembers,
			NumRepeatVst = @numRepeatVst
			WHERE MeetingId = @mid
	END
	ELSE
	BEGIN
		SELECT @numNewVisit = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId = 60
		SELECT @numRepeatVst = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId = 50
		SELECT @numVstMembers = COUNT(*) FROM dbo.Attend WHERE MeetingId = @mid AND AttendanceFlag = 1 AND AttendanceTypeId = 40
		UPDATE dbo.Meetings SET
			NumNewVisit = @numNewVisit,
			NumVstMembers = @numVstMembers,
			NumRepeatVst = @numRepeatVst
			WHERE MeetingId = @mid
	END
END
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleName] [varchar](50) NULL,
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Roles] ON [dbo].[Roles] 
(
	[RoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT [dbo].[Roles] ([RoleName], [RoleId]) VALUES (N'Admin', 1)
INSERT [dbo].[Roles] ([RoleName], [RoleId]) VALUES (N'Attendance', 3)
INSERT [dbo].[Roles] ([RoleName], [RoleId]) VALUES (N'Developer', 9)
INSERT [dbo].[Roles] ([RoleName], [RoleId]) VALUES (N'Edit', 4)
INSERT [dbo].[Roles] ([RoleName], [RoleId]) VALUES (N'Finance', 8)
INSERT [dbo].[Roles] ([RoleName], [RoleId]) VALUES (N'Membership', 5)
INSERT [dbo].[Roles] ([RoleName], [RoleId]) VALUES (N'NoRemoteAccess', 10)
INSERT [dbo].[Roles] ([RoleName], [RoleId]) VALUES (N'OrgMembersOnly', 7)
INSERT [dbo].[Roles] ([RoleName], [RoleId]) VALUES (N'OrgTagger', 6)
INSERT [dbo].[Roles] ([RoleName], [RoleId]) VALUES (N'Staff', 2)
SET IDENTITY_INSERT [dbo].[Roles] OFF
/****** Object:  Table [dbo].[UserRole]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (1, 1)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (1, 2)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (1, 3)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (1, 4)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (1, 5)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (1, 6)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (1, 8)
/****** Object:  StoredProcedure [dbo].[PurgePerson]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PurgePerson](@pid INT)
AS
BEGIN
	BEGIN TRY 
		BEGIN TRANSACTION 
		DECLARE @fid INT, @pic INT
		DELETE FROM dbo.OrganizationMembers WHERE PeopleId = @pid
		DELETE FROM dbo.BadET WHERE PeopleId = @pid
		DELETE FROM dbo.EnrollmentTransaction WHERE PeopleId = @pid
		
		DECLARE @t TABLE(id int)
		INSERT INTO @t(id) SELECT MeetingId FROM dbo.Attend a WHERE a.PeopleId = @pid
		
		DELETE FROM dbo.Attend WHERE PeopleId = @pid
		
		DECLARE cur CURSOR FOR SELECT id FROM @t
		OPEN cur
		DECLARE @mid int
		FETCH NEXT FROM cur INTO @mid
		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXECUTE dbo.UpdateMeetingCounters @mid
			FETCH NEXT FROM cur INTO @mid
		END
		CLOSE cur
		DEALLOCATE cur
		
		UPDATE dbo.Contribution SET PeopleId = NULL WHERE PeopleId = @pid
		DELETE FROM dbo.VolunteerForm WHERE PeopleId = @pid
		DELETE FROM dbo.Volunteer WHERE PeopleId = @pid
		DELETE FROM dbo.Contactees WHERE PeopleId = @pid
		DELETE FROM dbo.Contactors WHERE PeopleId = @pid
		DELETE FROM dbo.TagPerson WHERE PeopleId = @pid
		DELETE FROM dbo.Task WHERE WhoId = @pid
		DELETE FROM dbo.Task WHERE OwnerId = @pid
		DELETE FROM dbo.Task WHERE CoOwnerId = @pid
		DELETE FROM dbo.TaskListOwners WHERE PeopleId = @pid
		
		DELETE FROM dbo.Preferences WHERE UserId IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		DELETE FROM dbo.ActivityLog WHERE UserId IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		DELETE FROM dbo.UserRole WHERE UserId IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		DELETE FROM dbo.UserCanEmailFor WHERE UserId IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		DELETE FROM dbo.UserCanEmailFor WHERE CanEmailFor IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		DELETE FROM dbo.UserCanEmailFor WHERE CanEmailFor IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		UPDATE dbo.VolunteerForm SET UploaderId = NULL WHERE UploaderId IN (SELECT UserId FROM dbo.Users WHERE PeopleId = @pid)
		DELETE FROM dbo.Users WHERE PeopleId = @pid
		
		DELETE FROM dbo.TagOrg WHERE Id IN (SELECT Id FROM dbo.Tag WHERE PeopleId = @pid)
		DELETE FROM dbo.TagPerson WHERE id IN (SELECT Id FROM dbo.Tag WHERE PeopleId = @pid)
		DELETE FROM dbo.TagShare WHERE TagId IN (SELECT Id FROM dbo.Tag WHERE PeopleId = @pid)
		DELETE FROM dbo.TagShare WHERE PeopleId = @pid
		DELETE FROM dbo.TagTag WHERE Id IN (SELECT Id FROM dbo.Tag WHERE PeopleId = @pid)
		DELETE FROM dbo.TagTag WHERE ParentTagId IN (SELECT Id FROM dbo.Tag WHERE PeopleId = @pid)
		DELETE FROM dbo.Tag WHERE PeopleId = @pid
		
		SELECT @fid = FamilyId, @pic = PictureId FROM dbo.People WHERE PeopleId = @pid
		DELETE FROM dbo.People WHERE PeopleId = @pid
		IF (SELECT COUNT(*) FROM dbo.People WHERE FamilyId = @fid) = 0
		BEGIN
			DELETE FROM dbo.RelatedFamilies WHERE FamilyId = @fid OR RelatedFamilyId = @fid
			DELETE FROM dbo.Families WHERE FamilyId = @fid			
		END
		DELETE FROM dbo.Picture WHERE PictureId = @pic
		COMMIT
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH 
 
END
GO
/****** Object:  StoredProcedure [dbo].[PurgeAllPeople]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PurgeAllPeople]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		DECLARE pcur CURSOR FOR SELECT PeopleId FROM dbo.People
		OPEN pcur
		DECLARE @pid INT, @n INT
		SET @n = 0
		FETCH NEXT FROM pcur INTO @pid
		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXECUTE dbo.PurgePerson	@pid
			SET @n = @n + 1
			IF (@n % 50) = 0
				RAISERROR ('%d', 0, 1, @n) WITH NOWAIT
			IF (@n % 3) = 0
			BEGIN
				FETCH NEXT FROM pcur INTO @pid
				SET @n = @n + 1
				IF (@n % 50) = 0
					RAISERROR ('%d', 0, 1, @n) WITH NOWAIT
			END
			FETCH NEXT FROM pcur INTO @pid
		END
		CLOSE pcur
		DEALLOCATE pcur
END
GO
/****** Object:  StoredProcedure [dbo].[PrimaryAdult3]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PrimaryAdult3]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT PeopleId, v.cnt, v.FamilyId FROM dbo.People p
	JOIN (SELECT COUNT(*) cnt, FamilyId
	FROM dbo.People
	WHERE PositionInFamilyId = 10
	GROUP BY FamilyId
	HAVING COUNT(*) > 2) v
	ON p.FamilyId = v.FamilyId
END
GO
/****** Object:  View [dbo].[Nick]    Script Date: 05/06/2009 19:26:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Nick]
AS
SELECT NickName, COUNT(*) AS [count] FROM dbo.People GROUP BY NickName
GO
/****** Object:  UserDefinedFunction [dbo].[NextBirthday]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[NextBirthday](@pid int)
RETURNS datetime
AS
	BEGIN
	
	
	  DECLARE
		@today DATETIME,
		@date datetime, 
		@m int,
		@d int,
		@y int
		
SELECT @today = CONVERT(datetime, CONVERT(varchar, GETDATE(), 112))
select @date = null
select @m = BirthMonth, @d = BirthDay from dbo.People where @pid = PeopleId
if @m is null or @d is null
	return @date
select @y = DATEPART(year, @today) 
select @date = dateadd(mm,(@y-1900)* 12 + @m - 1,0) + (@d-1) 
if @date < @today
	select @date = dateadd(yy, 1, @date)
RETURN @date
	END
GO
/****** Object:  StoredProcedure [dbo].[UpdateLargeMeetingCounters]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateLargeMeetingCounters]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		DECLARE cur CURSOR FOR SELECT MeetingId FROM dbo.Meetings WHERE NumPresent > 4000
		OPEN cur
		DECLARE @mid int
		FETCH NEXT FROM cur INTO @mid
		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXECUTE dbo.UpdateMeetingCounters @mid
			FETCH NEXT FROM cur INTO @mid
		END
		CLOSE cur
		DEALLOCATE cur
END
GO
/****** Object:  UserDefinedFunction [dbo].[InBFClass2]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[InBFClass2]
	(
	@pid int
	)
RETURNS bit
AS
	BEGIN
	declare @mem bit

select @mem = 1
FROM dbo.OrganizationMembers  AS om
join dbo.Organizations o on o.OrganizationId = om.OrganizationId
join dbo.TagOrg AS tgo on tgo.organizationid = o.OrganizationId
join dbo.Tag t on tgo.id = t.id
join dbo.TagTag tt on tt.id = t.id
where tt.parenttagid = 101 
and o.RecordStatus = 0 
and om.RecordStatus = 0
and om.PeopleId = @pid

RETURN isnull(@mem, 0)
	END
GO
/****** Object:  View [dbo].[FirstNick]    Script Date: 05/06/2009 19:26:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FirstNick]
AS
SELECT FirstName, NickName, COUNT(*) AS [count] FROM dbo.People GROUP BY FirstName, NickName
GO
/****** Object:  View [dbo].[FirstName2]    Script Date: 05/06/2009 19:26:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FirstName2]
AS
SELECT     FirstName, GenderId, CA, COUNT(*) AS Expr1
FROM         (SELECT     FirstName, GenderId, CASE WHEN Age <= 18 THEN 'C' ELSE 'A' END AS CA
                       FROM          dbo.People) AS ttt
GROUP BY FirstName, GenderId, CA
GO
/****** Object:  View [dbo].[FirstName]    Script Date: 05/06/2009 19:26:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FirstName]
AS
SELECT     FirstName, COUNT(*) AS count
FROM         dbo.People
GROUP BY FirstName
GO
/****** Object:  View [dbo].[LastName]    Script Date: 05/06/2009 19:26:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[LastName]
AS
SELECT LastName, COUNT(*) AS [count] FROM dbo.People GROUP BY LastName
GO
/****** Object:  UserDefinedFunction [dbo].[EnrollmentTransactionId]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[EnrollmentTransactionId]
(
  @pid int
 ,@oid int
 ,@tid int
 ,@ttid int
)
RETURNS int
AS
	BEGIN
	  DECLARE @TransactionId int
	  SELECT @TransactionId = NULL
	  if @ttid >= 3
		  select top 1 @TransactionId = et.TransactionId
			from  dbo.EnrollmentTransaction et
		   where et.TransactionTypeId <= 2
			 and et.PeopleId = @pid
			 and et.OrganizationId = @oid
			 and et.TransactionId < @tid
	   order by et.TransactionId desc
	RETURN @TransactionId
	END
GO
/****** Object:  UserDefinedFunction [dbo].[NextTranChangeDate]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[NextTranChangeDate]
(
  @pid int
 ,@oid int
 ,@tid int
 ,@typeid int
)
RETURNS datetime
AS
	BEGIN
	  DECLARE @dt datetime 
		  select top 1 @dt = TransactionDate
			from dbo.EnrollmentTransaction
		   where TransactionTypeId >= 3
		     and @typeid <= 3
			 and PeopleId = @pid
			 and OrganizationId = @oid
			 and TransactionId > @tid
	   order by TransactionId
	RETURN @dt
	END
GO
/****** Object:  StoredProcedure [dbo].[ShowTransactions]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ShowTransactions](@pid INT, @orgid INT)
AS
BEGIN
	SELECT
		TransactionId, 
		TransactionDate, 
		TransactionTypeId, 
		OrganizationId, 
		PeopleId, 
		NextTranChangeDate,
		ClothingSizeInfo,
		dbo.NextTranChangeDate(PeopleId, OrganizationId, TransactionId, TransactionTypeId) NextTranChangeDate0,
		EnrollmentTransactionId,
		dbo.EnrollmentTransactionId(PeopleId, OrganizationId, TransactionId, TransactionTypeId) EnrollmentTransactionId0,
		CreatedDate
	FROM dbo.EnrollmentTransaction
	WHERE PeopleId = @pid AND (OrganizationId = @orgid OR @orgid = 0)
	ORDER BY TransactionId

END
GO
/****** Object:  StoredProcedure [dbo].[ShowOddTransactions]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ShowOddTransactions]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @t TABLE(PeopleId INT, OrgId INT)

	DECLARE @tid INT, @typeid INT, @orgid INT, @pid INT
	DECLARE @ptid INT = 0, @ptypeid INT = 0, @porgid INT = 0, @ppid INT = 0

	DECLARE c CURSOR FOR
	SELECT TransactionId, TransactionTypeId, OrganizationId, PeopleId 
	FROM dbo.EnrollmentTransaction
	ORDER BY PeopleId, OrganizationId, TransactionId

	OPEN c
	FETCH NEXT FROM c INTO @tid, @typeid, @orgid, @pid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @ppid = @pid AND @porgid = @orgid
		BEGIN
			IF (@typeid < 3 AND @ptypeid < 3) or (@typeid > 3 AND @ptypeid > 3)
				INSERT @t (PeopleId, OrgId) VALUES (@pid, @orgid)
		END
		
		SELECT @ptid = @tid, @ptypeid = @typeid, @porgid = @orgid, @ppid = @pid
		FETCH NEXT FROM c INTO @tid, @typeid, @orgid, @pid
	END
	CLOSE c
	DEALLOCATE c
	
	DECLARE cc CURSOR FOR
	SELECT DISTINCT * FROM @t
	OPEN cc
	FETCH NEXT FROM cc INTO @pid, @orgid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC dbo.ShowTransactions @pid, @orgid
		FETCH NEXT FROM cc INTO @pid, @orgid
	END
	CLOSE cc
	DEALLOCATE cc
	
END
GO
/****** Object:  StoredProcedure [dbo].[PopulateComputedEnrollmentTransactions]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PopulateComputedEnrollmentTransactions]
AS
BEGIN
	UPDATE dbo.EnrollmentTransaction
	SET NextTranChangeDate = dbo.NextTranChangeDate(PeopleId, OrganizationId, TransactionId, TransactionTypeId),
		EnrollmentTransactionId = dbo.EnrollmentTransactionId(PeopleId, OrganizationId, TransactionId, TransactionTypeId)
END
GO
/****** Object:  UserDefinedFunction [dbo].[NextChangeTransactionId2]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[NextChangeTransactionId2]
(
  @pid int
 ,@oid int
 ,@tid int
 ,@typeid int
)
RETURNS int
AS
	BEGIN
	  DECLARE @rtid int 
		  select top 1 @rtid = TransactionId
			from dbo.EnrollmentTransaction
		   where TransactionTypeId >= 3
		     and @typeid <= 3
			 and PeopleId = @pid
			 and OrganizationId = @oid
			 and TransactionId > @tid
			 AND TransactionStatus = 0
	   order by TransactionId
	RETURN @rtid
	END
GO
/****** Object:  UserDefinedFunction [dbo].[NextChangeTransactionId]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[NextChangeTransactionId]
(
  @pid int
 ,@oid int
 ,@tid int
 ,@typeid int
)
RETURNS int
AS
	BEGIN
	  DECLARE @rtid int 
		  select top 1 @rtid = TransactionId
			from dbo.EnrollmentTransaction
		   where TransactionTypeId >= 3
		     and @typeid <= 3
			 and PeopleId = @pid
			 and OrganizationId = @oid
			 and TransactionId > @tid
	   order by TransactionId
	RETURN @rtid
	END
GO
/****** Object:  StoredProcedure [dbo].[LinkEnrollmentTransactions]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[LinkEnrollmentTransactions] (@pid INT, @orgid INT)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @tid INT, @typeid INT, @tdt DATETIME

	DECLARE c CURSOR FOR
	SELECT TransactionId, TransactionTypeId, TransactionDate
	FROM dbo.EnrollmentTransaction et
	WHERE et.TransactionStatus = 0 AND et.PeopleId = @pid AND et.OrganizationId = @orgid
	ORDER BY TransactionDate, TransactionId

	OPEN c
	FETCH NEXT FROM c INTO @tid, @typeid, @tdt
	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC dbo.LinkEnrollmentTransaction @tid, @tdt, @typeid, @orgid, @pid
		FETCH NEXT FROM c INTO @tid, @typeid, @tdt
	END
	CLOSE c
	DEALLOCATE c
END
GO
/****** Object:  Trigger [insEnrollmentTransaction]    Script Date: 05/06/2009 19:26:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[insEnrollmentTransaction] 
   ON  [dbo].[EnrollmentTransaction] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @tid INT, @trandt DATETIME, @typeid INT, @orgid INT, @pid INT

	DECLARE c CURSOR FORWARD_ONLY FOR
	SELECT TransactionId, TransactionDate, TransactionTypeId, OrganizationId, PeopleId 
	FROM inserted 
	WHERE TransactionTypeId > 2

	OPEN c
	FETCH NEXT FROM c INTO @tid, @trandt, @typeid, @orgid, @pid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC dbo.LinkEnrollmentTransaction @tid, @trandt, @typeid, @orgid, @pid
		FETCH NEXT FROM c INTO @tid, @trandt, @typeid, @orgid, @pid
	END
	CLOSE c
	DEALLOCATE c
END
GO
/****** Object:  UserDefinedFunction [dbo].[EnrollmentTransactionId2]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[EnrollmentTransactionId2]
(
  @pid int
 ,@oid int
 ,@tid int
 ,@ttid int
)
RETURNS int
AS
	BEGIN
	  DECLARE @TransactionId int
	  SELECT @TransactionId = NULL
	  if @ttid >= 3
		  select top 1 @TransactionId = et.TransactionId
			from  dbo.EnrollmentTransaction et
		   where et.TransactionTypeId <= 2
			 and et.PeopleId = @pid
			 and et.OrganizationId = @oid
			 and et.TransactionId < @tid
			 AND et.TransactionStatus = 0
	   order by et.TransactionId desc
	RETURN @TransactionId
	END
GO
/****** Object:  View [dbo].[City]    Script Date: 05/06/2009 19:26:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[City]
AS
SELECT PrimaryCity AS City, PrimaryState AS State, PrimaryZip AS Zip, COUNT(*) AS [count] FROM dbo.People GROUP BY PrimaryCity, PrimaryState, PrimaryZip
GO
/****** Object:  UserDefinedFunction [dbo].[Contributors]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[Contributors](@fd DATETIME, @td DATETIME, @pid INT, @spid INT, @fid INT)
RETURNS TABLE 
AS
RETURN 
(
	select	p.PeopleId, 
			p.PositionInFamilyId, 
			p.Name, 
			f.HeadOfHouseholdId, 
			p.TitleCode AS Title, 
			p.SuffixCode AS Suffix,
			p.SpouseId,
			sp.[Name] AS SpouseName, 
			sp.TitleCode AS SpouseTitle,
			sp.ContributionOptionsId AS SpouseContributionOptionsId,
			p.ContributionOptionsId,
			p.PrimaryAddress,
			p.PrimaryAddress2,
			p.PrimaryCity,
			p.PrimaryState,
			p.PrimaryZip,
			p.DeceasedDate,
			p.FamilyId,
			p.Age,
			CASE WHEN f.HeadOfHouseholdId = p.PeopleId THEN 1 ELSE 0 END AS hohFlag
	from People p
	JOIN dbo.Families f ON p.FamilyId = f.FamilyId
	LEFT OUTER JOIN dbo.People sp ON p.SpouseId = sp.PeopleId
	WHERE EXISTS(
		SELECT NULL FROM Contribution c 
		WHERE c.PeopleId = p.PeopleId
		AND c.ContributionStatusId = 0
		AND c.ContributionTypeId NOT IN (6,7)
		AND c.ContributionDate >= @fd
		AND c.ContributionDate <= @td)
	AND p.ContributionOptionsId > 0
	AND (@pid = 0 OR @pid = p.PeopleId OR @spid = p.PeopleId)
	AND (@fid = 0 OR @fid = p.FamilyId)
)
GO
/****** Object:  UserDefinedFunction [dbo].[Birthday]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Birthday](@pid int)
RETURNS DATETIME
AS
BEGIN
	
	DECLARE
		@dt DATETIME, 
		@m int,
		@d int,
		@y int
    SET @dt = NULL
		
	select @m = BirthMonth, @d = BirthDay, @y = BirthYear from dbo.People where @pid = PeopleId
	IF NOT (@m IS NULL OR @y IS NULL OR @d IS NULL)
	    SET @dt = dateadd(month,((@y-1900)*12)+@m-1,@d-1)
	RETURN @dt
END
GO
/****** Object:  StoredProcedure [dbo].[FlagOddTransactions]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FlagOddTransactions]
AS
BEGIN
	SET NOCOUNT ON;

	TRUNCATE TABLE dbo.BadET

	DECLARE @tid INT, @typeid INT, @orgid INT, @pid INT, @tdt DATETIME
	DECLARE @ptid INT = 0, @ptypeid INT = 0, @porgid INT = 0, @ppid INT = 0, @ptdt DATETIME

	DECLARE c CURSOR FOR
	SELECT TransactionId, TransactionTypeId, OrganizationId, PeopleId, TransactionDate
	FROM dbo.EnrollmentTransaction et
	WHERE et.TransactionStatus = 0
	ORDER BY PeopleId, OrganizationId, TransactionDate, TransactionId

	OPEN c
	FETCH NEXT FROM c INTO @tid, @typeid, @orgid, @pid, @tdt
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @ptid > 0 AND (@ppid <> @pid OR @porgid <> @orgid)
		BEGIN
			IF (@ppid <> @pid OR @porgid <> @orgid) AND 
					@ptypeid < 3 AND NOT EXISTS(SELECT NULL FROM dbo.OrganizationMembers
							WHERE OrganizationId = @porgid AND PeopleId = @ppid)
				INSERT INTO dbo.BadET (PeopleId, OrgId,	TranId,	Flag) 
								VALUES ( @ppid, @porgid, @ptid, 10)
			SELECT @ptid = 0, @ptypeid = 0, @porgid = 0, @ppid = 0
		END
		IF @ptid > 0
		BEGIN
			IF @tdt = @ptdt
				INSERT INTO dbo.BadET (PeopleId, OrgId,	TranId,	Flag) 
								VALUES ( @pid, @orgid, @tid, 15)
			ELSE IF @typeid < 3 AND @ptypeid <= 3
				INSERT INTO dbo.BadET (PeopleId, OrgId,	TranId,	Flag) 
								VALUES ( @pid, @orgid, @tid, 11)
				
			ELSE IF @typeid > 3 AND @ptypeid > 3
				INSERT INTO dbo.BadET (PeopleId, OrgId,	TranId,	Flag) 
								VALUES ( @pid, @orgid, @tid, 55)
		END

		SELECT @ptid = @tid, @ptypeid = @typeid, @porgid = @orgid, @ppid = @pid, @ptdt = @tdt

		FETCH NEXT FROM c INTO @tid, @typeid, @orgid, @pid, @tdt
	END
	CLOSE c
	DEALLOCATE c
	
END
GO
/****** Object:  StoredProcedure [dbo].[FlagOddTransaction]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FlagOddTransaction] (@pid INT, @orgid INT)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM dbo.BadET WHERE PeopleId = @pid AND OrgId = @orgid

	DECLARE @tid INT, @typeid INT, @tdt DATETIME
	DECLARE @ptypeid INT = 0, @ptdt DATETIME

	DECLARE c CURSOR FOR
	SELECT TransactionId, TransactionTypeId, TransactionDate
	FROM dbo.EnrollmentTransaction et
	WHERE et.TransactionStatus = 0 AND et.PeopleId = @pid AND et.OrganizationId = @orgid
	ORDER BY TransactionDate, TransactionId

	OPEN c
	FETCH NEXT FROM c INTO @tid, @typeid, @tdt
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @ptypeid > 0
		BEGIN
			IF @tdt = @ptdt
				INSERT INTO dbo.BadET (PeopleId, OrgId,	TranId,	Flag) 
								VALUES ( @pid, @orgid, @tid, 15)
			ELSE IF @typeid < 3 AND @ptypeid <= 3
				INSERT INTO dbo.BadET (PeopleId, OrgId,	TranId,	Flag) 
								VALUES ( @pid, @orgid, @tid, 11)
				
			ELSE IF @typeid > 3 AND @ptypeid > 3
				INSERT INTO dbo.BadET (PeopleId, OrgId,	TranId,	Flag) 
								VALUES ( @pid, @orgid, @tid, 55)
		END

		SELECT @ptypeid = @typeid, @ptdt = @tdt

		FETCH NEXT FROM c INTO @tid, @typeid, @tdt
	END
	CLOSE c
	DEALLOCATE c

	IF @typeid < 3 AND NOT EXISTS(SELECT NULL FROM dbo.OrganizationMembers
					WHERE OrganizationId = @orgid AND PeopleId = @pid)
		INSERT INTO dbo.BadET (PeopleId, OrgId,	TranId,	Flag) 
						VALUES ( @pid, @orgid, @tid, 10)
	
END
GO
/****** Object:  View [dbo].[BadETView]    Script Date: 05/06/2009 19:26:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[BadETView]
AS
SELECT     et.id, et.Flag, et2.PeopleId, et2.OrganizationId, et2.TransactionId, et2.OrganizationName, p.Name2, et.Status, et2.TransactionDate, et2.TransactionTypeId, 
                      et2.TransactionStatus
FROM         dbo.People AS p INNER JOIN
                      dbo.EnrollmentTransaction AS et2 ON p.PeopleId = et2.PeopleId LEFT OUTER JOIN
                      dbo.BadET AS et ON et2.TransactionId = et.TranId
WHERE     EXISTS
                          (SELECT     NULL AS Expr1
                            FROM          dbo.BadET
                            WHERE      (OrgId = et2.OrganizationId) AND (PeopleId = et2.PeopleId))
GO
/****** Object:  UserDefinedFunction [dbo].[BadEtsList]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[BadEtsList](@flag INT) 
RETURNS TABLE 
AS
RETURN 
(
SELECT et.id, et.Flag, et2.PeopleId, et2.OrganizationId, et2.TransactionId, et2.OrganizationName, p.Name2, et.Status, et2.TransactionDate, et2.TransactionTypeId, 
                      et2.TransactionStatus
FROM dbo.People AS p 
INNER JOIN dbo.EnrollmentTransaction AS et2 ON p.PeopleId = et2.PeopleId 
LEFT OUTER JOIN dbo.BadET AS et ON et2.TransactionId = et.TranId
WHERE EXISTS (SELECT NULL FROM dbo.BadET
              WHERE OrgId = et2.OrganizationId AND PeopleId = et2.PeopleId)
AND (@flag = 0 
OR EXISTS(SELECT NULL FROM dbo.BadET
			WHERE OrgId = et2.OrganizationId AND PeopleId = et2.PeopleId
			AND Flag = @flag))
)
GO
/****** Object:  Table [dbo].[DivOrg]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DivOrg](
	[DivId] [int] NOT NULL,
	[OrgId] [int] NOT NULL,
 CONSTRAINT [PK_DivOrg] PRIMARY KEY CLUSTERED 
(
	[DivId] ASC,
	[OrgId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[DivOrg] ([DivId], [OrgId]) VALUES (1, 1)
/****** Object:  StoredProcedure [dbo].[PurgeOrganization]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PurgeOrganization](@oid INT)
AS
BEGIN
	BEGIN TRY 
		BEGIN TRANSACTION 
		DECLARE @fid INT, @pic INT
		DELETE FROM dbo.OrganizationMembers WHERE OrganizationId = @oid
		DELETE FROM dbo.BadET WHERE OrgId = @oid
		DELETE FROM dbo.EnrollmentTransaction WHERE OrganizationId = @oid
		DELETE FROM dbo.Attend WHERE OrganizationId = @oid
		DELETE FROM dbo.DivOrg WHERE OrgId = @oid
		DELETE FROM dbo.Meetings WHERE OrganizationId = @oid
		DELETE FROM dbo.TagOrg WHERE OrganizationId = @oid
		DELETE FROM dbo.Organizations WHERE OrganizationId = @oid
		COMMIT
	END TRY 
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH 
END
GO
/****** Object:  UserDefinedFunction [dbo].[PersonAttendCountOrg]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[PersonAttendCountOrg]
(@pid int, @oid int)
RETURNS int
AS
	BEGIN
	RETURN (SELECT COUNT(*)
	        FROM   dbo.Attend a INNER JOIN
	                   dbo.Meetings m ON a.MeetingId = m.MeetingId
	        WHERE (m.OrganizationId = @oid) AND (a.PeopleId = @pid)
              AND a.AttendanceFlag = 1)
	END
GO
/****** Object:  UserDefinedFunction [dbo].[LastAttended]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[LastAttended] (@orgid INT, @pid INT)
RETURNS DATETIME
AS
	BEGIN
	DECLARE @dt DATETIME
		SELECT @dt = MAX(m.MeetingDate) FROM dbo.Attend a
		JOIN dbo.Meetings m
		ON a.MeetingId = m.MeetingId
		WHERE a.AttendanceFlag = 1 AND m.OrganizationId = @orgid AND a.PeopleId = @pid
	RETURN @dt
	END
GO
/****** Object:  UserDefinedFunction [dbo].[LastAttend]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[LastAttend] (@orgid INT, @pid INT)
RETURNS DATETIME
AS
	BEGIN
	DECLARE @dt DATETIME
		SELECT @dt = MAX(m.MeetingDate) FROM dbo.Attend a
		JOIN dbo.Meetings m
		ON a.MeetingId = m.MeetingId
		WHERE a.AttendanceFlag = 1 AND m.OrganizationId = @orgid AND a.PeopleId = @pid
	RETURN @dt
	END
GO
/****** Object:  UserDefinedFunction [dbo].[DaysSinceAttend]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[DaysSinceAttend](@pid INT, @oid INT)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @days int

	-- Add the T-SQL statements to compute the return value here
	SELECT @days = MAX(DATEDIFF(D,a.MeetingDate,GETDATE())) FROM dbo.Attend a
	JOIN dbo.Meetings m
	ON a.MeetingId = m.MeetingId
	WHERE a.PeopleId = @pid AND m.OrganizationId = @oid
	

	-- Return the result of the function
	RETURN @days

END
GO
/****** Object:  UserDefinedFunction [dbo].[AttendStr_3]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[AttendStr_3](@orgid int, @pid int)
RETURNS varchar(100)
BEGIN
	DECLARE @a nvarchar(200) -- attendance string
	set @a = ''
			
	DECLARE @mindt DATETIME, @dt DATETIME 

	SELECT @mindt = MIN(MeetingDate), @dt = MAX(MeetingDate) FROM dbo.Attend WHERE OrganizationId = @orgid
	SELECT @dt = DATEADD(yy, -1, @dt)
	IF (@dt > @mindt)
		SELECT @mindt = @dt
	
	FROM dbo.Attend
	WHERE OrganizationId = @orgid AND PeopleId = @pid

	SELECT @a = 
	CASE a.AttendanceTypeId
	WHEN 70 THEN 'I'
	WHEN 90 THEN 'G'
	WHEN 80 THEN 'O'
	WHEN 110 THEN '*'
	WHEN 0 THEN '.'
	ELSE CASE a.AttendanceFlag
		WHEN NULL THEN '*'
		WHEN 1 THEN 'P'
		WHEN 0 THEN '.'
		END
	END + @a
	FROM dbo.Attend a
	WHERE a.MeetingDate >= @dt AND a.PeopleId = @pid AND a.OrganizationId = @orgid
	ORDER BY MeetingDate DESC

	RETURN @a
END
GO
/****** Object:  UserDefinedFunction [dbo].[AttendPct_2]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[AttendPct_2](@orgid int, @pid int) 
RETURNS Real
AS
BEGIN


--DECLARE @orgid INT, @pid INT
--SELECT @orgid = 80092, @pid = 828612

    DECLARE @pct REAL
    DECLARE @yearago DATETIME
    DECLARE @lastmeet DATETIME 
    DECLARE @tct INT 
    DECLARE @act INT
    
    SELECT @lastmeet = MAX(MeetingDate) FROM dbo.Meetings
    WHERE OrganizationId = @orgid
    
    SELECT @yearago = DATEADD(year,-1,@lastmeet)
    
	SELECT @tct = COUNT(*) FROM dbo.Attend
     WHERE PeopleId = @pid
       AND OrganizationId = @orgid
       AND AttendanceFlag IS NOT NULL
       AND MeetingDate >= @yearago
       
    SELECT @act = COUNT(*) FROM dbo.Attend
     WHERE PeopleId = @pid
       AND OrganizationId = @orgid
       AND AttendanceFlag = 1
       AND MeetingDate >= @yearago
       
       
	if @tct = 0
			select @pct = 0
		else
			SELECT @pct = @act * 100.0 / @tct

	RETURN @pct
    
END
GO
/****** Object:  StoredProcedure [dbo].[AddAbsents]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddAbsents](@meetid INT, @userid INT)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @oid INT, @meetdate DATETIME, @attendType INT, @offsiteType INT, @groupflag BIT, @dt DATETIME
	SELECT @oid = OrganizationId, @meetdate = MeetingDate, 
		@attendType = 30, @offsiteType = 80, 
		@groupflag = GroupMeetingFlag, @dt = GETDATE()
	FROM dbo.Meetings m
	WHERE MeetingId = @meetid

	IF @groupflag <> 0
		RETURN

BEGIN TRY
	BEGIN TRANSACTION
	INSERT dbo.Attend 
	(
		MeetingId,
		OrganizationId,
		MeetingDate,
		CreatedBy,
		CreatedDate,
		OtherOrgId,
		PeopleId,
		MemberTypeId,
		AttendanceFlag,
		AttendanceTypeId
	)
	SELECT
		@meetid, 
		@oid, 
		@meetdate, 
		@userid,
		@dt, 
		NULL, 
		o.PeopleId, 
		o.MemberTypeId,
		0,
		@attendType
	FROM dbo.OrganizationMembers o
	WHERE o.OrganizationId = @oid
		AND NOT EXISTS
		(
			SELECT NULL FROM dbo.Attend a 
			WHERE a.PeopleId = o.PeopleId AND a.MeetingId = @meetid
		)

	UPDATE dbo.Attend
	SET 
		AttendanceFlag = NULL, 
		AttendanceTypeId = @offsiteType
	FROM dbo.OrganizationMembers o
	JOIN dbo.Attend a ON o.OrganizationId = a.OrganizationId
	WHERE a.MeetingId = @meetid AND a.PeopleId = o.PeopleId
	AND EXISTS
	(
		SELECT NULL FROM dbo.OrganizationMembers om2
		JOIN dbo.Organizations o2 ON om2.OrganizationId = o2.OrganizationId
		WHERE o2.AttendClassificationId = 2 AND om2.PeopleId = o.PeopleId
		AND o2.FirstMeetingDate <= @meetdate AND @meetdate <= DATEADD(dd, 1, o2.LastMeetingDate)
	) 
	COMMIT
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK
	DECLARE @em VARCHAR(4000), @es INT
	SELECT @em=ERROR_MESSAGE(), @es=ERROR_SEVERITY()
	RAISERROR(@em, @es, 1)
END CATCH


END
GO
/****** Object:  Table [lookup].[BFCStatus]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[BFCStatus](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_BFCStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[BFCStatus] ([Id], [Code], [Description]) VALUES (0, N'NON', N'Non-member')
INSERT [lookup].[BFCStatus] ([Id], [Code], [Description]) VALUES (10, N'VST', N'Visitor')
INSERT [lookup].[BFCStatus] ([Id], [Code], [Description]) VALUES (20, N'RVT', N'Recent Visitor')
INSERT [lookup].[BFCStatus] ([Id], [Code], [Description]) VALUES (30, N'MEM', N'Member')
INSERT [lookup].[BFCStatus] ([Id], [Code], [Description]) VALUES (40, N'HMB', N'Homebound')
/****** Object:  Table [lookup].[ContactPreference]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[ContactPreference](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_ContactPreference] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[ContactPreference] ([Id], [Code], [Description]) VALUES (0, N'NO', N'Do Not Contact')
INSERT [lookup].[ContactPreference] ([Id], [Code], [Description]) VALUES (10, N'PST', N'Mail')
INSERT [lookup].[ContactPreference] ([Id], [Code], [Description]) VALUES (20, N'PHN', N'Phone')
INSERT [lookup].[ContactPreference] ([Id], [Code], [Description]) VALUES (30, N'EML', N'Email')
INSERT [lookup].[ContactPreference] ([Id], [Code], [Description]) VALUES (40, N'VST', N'Visit')
/****** Object:  Table [dbo].[Audits]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Audits](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Action] [varchar](20) NOT NULL,
	[TableName] [varchar](100) NOT NULL,
	[TableKey] [int] NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[AuditDate] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Audits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChurchAttReportIds]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChurchAttReportIds](
	[Name] [varchar](40) NOT NULL,
	[Id] [int] NOT NULL,
 CONSTRAINT [PK_ChurchAttReportIds] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [lookup].[BundleStatusTypes]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[BundleStatusTypes](
	[Id] [int] NOT NULL,
	[Code] [varchar](5) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_BundleStatusTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[BundleStatusTypes] ([Id], [Code], [Description]) VALUES (0, N'C', N'Closed')
INSERT [lookup].[BundleStatusTypes] ([Id], [Code], [Description]) VALUES (1, N'O', N'Open')
/****** Object:  Table [lookup].[BundleHeaderTypes]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[BundleHeaderTypes](
	[Id] [int] NOT NULL,
	[Code] [varchar](10) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_BundleHeaderTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[BundleHeaderTypes] ([Id], [Code], [Description]) VALUES (1, N'G', N'Generic Envelopes')
INSERT [lookup].[BundleHeaderTypes] ([Id], [Code], [Description]) VALUES (2, N'LC', N'Loose Checks and Cash')
INSERT [lookup].[BundleHeaderTypes] ([Id], [Code], [Description]) VALUES (3, N'PE', N'Preprinted Envelopes')
INSERT [lookup].[BundleHeaderTypes] ([Id], [Code], [Description]) VALUES (4, N'OL', N'Online')
/****** Object:  UserDefinedFunction [dbo].[DollarRange]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[DollarRange](@amt DECIMAL)
RETURNS int
AS
BEGIN
DECLARE @ret INT 
	SELECT @ret =
	CASE
		WHEN @amt IS NULL THEN 1
		WHEN @amt < 101 THEN 1
		WHEN @amt < 251 THEN 2
		WHEN @amt < 501 THEN 3
		WHEN @amt < 751 THEN 4
		WHEN @amt < 1001 THEN 5
		WHEN @amt < 2001 THEN 6
		WHEN @amt < 3001 THEN 7
		WHEN @amt < 4001 THEN 8
		WHEN @amt < 5001 THEN 9
		WHEN @amt < 10001 THEN 10
		WHEN @amt < 20001 THEN 11
		WHEN @amt < 30001 THEN 12
		WHEN @amt < 40001 THEN 13
		WHEN @amt < 50001 THEN 14
		WHEN @amt < 100001 THEN 15
		ELSE 16
	END 
	RETURN @ret
END
GO
/****** Object:  Table [lookup].[CountryLookup]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[CountryLookup](
	[CountryName] [varchar](30) NOT NULL,
	[CountryCode] [varchar](10) NOT NULL,
	[DisplayFlag] [int] NULL,
 CONSTRAINT [PK_COUNTRY_LOOKUP_TBL_1] PRIMARY KEY CLUSTERED 
(
	[CountryCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [COUNTRY_LOOKUP_NAME_IX] ON [lookup].[CountryLookup] 
(
	[CountryName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Afghanistan', N'AFG', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Angola', N'AGO', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Albania', N'ALB', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Algeria', N'ALG', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Argentina', N'ARG', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Antarctica', N'ATA', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Australia', N'AUS', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Austria', N'AUT', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Bangladesh', N'BGD', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Bulgaria', N'BGR', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Bahrain', N'BHR', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Bahamas', N'BHS', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Bermuda', N'BMU', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Brazil', N'BRA', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Brunei', N'BRN', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Bhutan', N'BTN', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Botswana', N'BWA', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Canada', N'CAN', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Chile', N'CHL', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Colombia', N'COL', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Cape Verde', N'CPV', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Cuba', N'CUB', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Cyprus', N'CYP', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Czech Republic', N'CZE', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Denmark', N'DNK', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Egypt', N'EGY', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'England', N'ENG', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Ethiopia', N'ETH', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Finland', N'FIN', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'France', N'FRA', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Germany', N'GER', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Ghana', N'GHA', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Guinea', N'GIN', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Greece', N'GRC', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Grenada', N'GRD', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Guyana', N'GUY', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Hongkong', N'HK', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Hungary', N'HUN', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Indonesia', N'IDN', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'India', N'IND', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Iran', N'IRN', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Iraq', N'IRQ', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Iceland', N'ISL', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Israel', N'ISR', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Italy', N'ITA', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Jamaica', N'JAM', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Jordan', N'JOR', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Japan', N'JPN', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Korea', N'KOR', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Lebanon', N'LBN', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Malta', N'MAL', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Maldives', N'MDV', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Mexico', N'MEX', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Mali', N'MLI', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Myanmar', N'MMR', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Mongolia', N'MNG', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Mozambique', N'MOZ', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Mauritius', N'MUS', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Malaysia', N'MYS', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Namibia', N'NAM', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Netherlands', N'NLD', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Norway', N'NOR', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Nepal', N'NPL', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'New Zealand', N'NZL', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Panama', N'PAN', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Poland', N'POL', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Portugal', N'PRT', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Qatar', N'QAT', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Russia', N'RUS', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Sudan ', N'SDN', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Singapore', N'SGP', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Srilanka', N'SRI', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Thailand', N'THA', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Taiwan', N'TWN', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'United Arab Emirates', N'UAE', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Uganda', N'UGA', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Ukraine', N'UKR', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Uruguay', N'URY', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'USA', N'USA', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Uzbekistan', N'UZB', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Venezuela', N'VEN', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Vietnam', N'VNM', 1)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Yugoslavia', N'YUG', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Zambia', N'ZMB', 0)
INSERT [lookup].[CountryLookup] ([CountryName], [CountryCode], [DisplayFlag]) VALUES (N'Zimbabwe', N'ZWE', 1)
/****** Object:  UserDefinedFunction [dbo].[DayAndTime]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[DayAndTime] (@dt DATETIME)
RETURNS VARCHAR(20)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @daytime VARCHAR(20)

SELECT @daytime =
	CASE DATEPART(dw,@dt)
    WHEN 1 THEN 'Sunday'
    WHEN 2 THEN 'Monday'
    WHEN 3 THEN 'Tuesday'
    WHEN 4 THEN 'Wednesday'
    WHEN 5 THEN 'Thursday'
    WHEN 6 THEN 'Friday'
    WHEN 7 THEN 'Saturday'
    END + ' ' + SUBSTRING(CONVERT(varchar,@dt,0),13,7)
    
	RETURN @daytime

END
GO
/****** Object:  UserDefinedFunction [dbo].[CsvTable]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[CsvTable](@csv VARCHAR(4000))
RETURNS 
@tbl TABLE (id int NOT NULL)
AS
BEGIN
	declare @pos int
	declare @val varchar(1000)

	set @csv = @csv + ','

WHILE PATINDEX('%,%',@csv) > 0
BEGIN

	SELECT @pos = PATINDEX('%,%', @csv)
	SELECT @val = left(@csv, @pos - 1)

	INSERT INTO @tbl SELECT @val
	SELECT @csv = STUFF(@csv, 1, @pos, '')
END

	RETURN 
END
GO
/****** Object:  UserDefinedFunction [dbo].[EntryPointId]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[EntryPointId] ( @pid INT )
RETURNS INT 
AS
BEGIN
	DECLARE @ret INT 

	SELECT TOP 1 @ret = e.Id FROM 
	dbo.Attend a 
	JOIN dbo.Meetings m ON a.MeetingId = m.MeetingId
	JOIN dbo.Organizations o ON m.OrganizationId = o.OrganizationId
	LEFT OUTER JOIN lookup.EntryPoint e ON o.EntryPointId = e.Id
	WHERE a.PeopleId = @pid
	ORDER BY a.MeetingDate
	
	RETURN @ret
END
GO
/****** Object:  StoredProcedure [dbo].[DisableForeignKeys]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DisableForeignKeys]
    @disable BIT = 1
AS
    DECLARE
        @sql VARCHAR(500),
        @tableName VARCHAR(128),
        @foreignKeyName VARCHAR(128),
		@schema varchar(50)

    -- A list of all foreign keys and table names
    DECLARE foreignKeyCursor CURSOR
    FOR SELECT
        ref.constraint_name AS FK_Name,
        fk.table_name AS FK_Table,
		ref.Constraint_schema as FK_Schema
    FROM
        INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS ref
        INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS fk 
    ON ref.constraint_name = fk.constraint_name
    ORDER BY
        fk.table_name,
        ref.constraint_name 

    OPEN foreignKeyCursor

    FETCH NEXT FROM foreignKeyCursor 
    INTO @foreignKeyName, @tableName, @schema

    WHILE ( @@FETCH_STATUS = 0 )
        BEGIN
            IF @disable = 1
                SET @sql = 'ALTER TABLE ' + @schema + '.[' 
                    + @tableName + '] NOCHECK CONSTRAINT ['
                    + @foreignKeyName + ']'
            ELSE
                SET @sql = 'ALTER TABLE ' + @schema + '.[' 
                    + @tableName + '] CHECK CONSTRAINT ['
                    + @foreignKeyName + ']'

        PRINT 'Executing Statement - ' + @sql

        EXECUTE(@sql)
        FETCH NEXT FROM foreignKeyCursor 
        INTO @foreignKeyName, @tableName, @schema
    END

    CLOSE foreignKeyCursor
    DEALLOCATE foreignKeyCursor
GO
/****** Object:  UserDefinedFunction [dbo].[FirstMondayOfMonth]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FirstMondayOfMonth] (@inputDate DATETIME)RETURNS DATETIME BEGIN     RETURN DATEADD(wk, DATEDIFF(wk, 0, dateadd(dd, 6 - datepart(day, @inputDate), @inputDate)), 0)  END
GO
/****** Object:  Table [lookup].[FamilyRelationship]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[FamilyRelationship](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_FamilyRelationship] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (100, N'HOH', N'Head of Household')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (110, N'SPS', N'Spouse')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (120, N'SEC', N'Secondary Adult')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (130, N'AUN', N'Aunt')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (135, N'UNC', N'Uncle')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (140, N'GRM', N'Grand Mother')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (145, N'GRF', N'Grand Father')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (200, N'CHI', N'Child')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (210, N'DTR', N'Daughter')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (215, N'SON', N'Son')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (220, N'NCE', N'Niece')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (225, N'NPH', N'Nephew')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (230, N'GRD', N'Grand Daughter')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (235, N'GRS', N'Grand Son')
INSERT [lookup].[FamilyRelationship] ([Id], [Code], [Description]) VALUES (980, N'OTH', N'Other')
/****** Object:  Table [lookup].[FamilyMemberType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[FamilyMemberType](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_FamilyMemberType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[FamilyMemberType] ([Id], [Code], [Description]) VALUES (0, N'ADU', N'Adult')
INSERT [lookup].[FamilyMemberType] ([Id], [Code], [Description]) VALUES (1, N'CHI', N'Child')
/****** Object:  Table [dbo].[GeoCodes]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeoCodes](
	[Address] [varchar](80) NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
 CONSTRAINT [PK_GeoCodes_1] PRIMARY KEY CLUSTERED 
(
	[Address] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [lookup].[GenderClass]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[GenderClass](
	[Id] [int] NOT NULL,
	[Code] [char](1) NULL,
	[Description] [varchar](20) NULL,
 CONSTRAINT [PK_GenderClass] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[GenderClass] ([Id], [Code], [Description]) VALUES (0, N'C', N'Coed')
INSERT [lookup].[GenderClass] ([Id], [Code], [Description]) VALUES (1, N'M', N'Male')
INSERT [lookup].[GenderClass] ([Id], [Code], [Description]) VALUES (2, N'F', N'Female')
/****** Object:  UserDefinedFunction [dbo].[LastNameCount]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[LastNameCount](@last VARCHAR(20))
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @r INT
	
	SELECT @r = [count] FROM dbo._LastName WHERE LastName = @last
	RETURN @r

END
GO
/****** Object:  Table [lookup].[NameTitle]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[NameTitle](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_NameTitle] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (0, N'', N'')
INSERT [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (100, N'Mr.', N'Mr.')
INSERT [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (110, N'Mrs.', N'Mrs.')
INSERT [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (120, N'Ms.', N'Ms.')
INSERT [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (130, N'Miss', N'Miss')
INSERT [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (140, N'Dr.', N'Dr.')
INSERT [lookup].[NameTitle] ([Id], [Code], [Description]) VALUES (150, N'Rev.', N'Rev.')
/****** Object:  Table [lookup].[NameSuffix]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[NameSuffix](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_NameSuffix] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (100, N'Jr.', N'Jr.')
INSERT [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (110, N'Sr.', N'Sr.')
INSERT [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (120, N'I', N'I')
INSERT [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (130, N'II', N'II')
INSERT [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (140, N'III', N'III')
INSERT [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (150, N'IV', N'IV')
INSERT [lookup].[NameSuffix] ([Id], [Code], [Description]) VALUES (160, N'V', N'V')
/****** Object:  Table [lookup].[MeetingType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[MeetingType](
	[Id] [int] NOT NULL,
	[Code] [char](1) NULL,
	[Description] [varchar](10) NULL,
 CONSTRAINT [PK_MeetingType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[MeetingType] ([Id], [Code], [Description]) VALUES (0, N'G', N'Group')
INSERT [lookup].[MeetingType] ([Id], [Code], [Description]) VALUES (1, N'R', N'Roster')
/****** Object:  Table [lookup].[AttendanceClassification]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[AttendanceClassification](
	[Id] [int] NOT NULL,
	[Code] [varchar](10) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_AttendanceClassification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[AttendanceClassification] ([Id], [Code], [Description]) VALUES (0, N'N', N'Normal')
INSERT [lookup].[AttendanceClassification] ([Id], [Code], [Description]) VALUES (1, N'IS', N'InService')
INSERT [lookup].[AttendanceClassification] ([Id], [Code], [Description]) VALUES (2, N'OS', N'OffSite')
INSERT [lookup].[AttendanceClassification] ([Id], [Code], [Description]) VALUES (3, N'B', N'Baptism')
INSERT [lookup].[AttendanceClassification] ([Id], [Code], [Description]) VALUES (4, N'DC', N'Step 1 Class')
/****** Object:  UserDefinedFunction [dbo].[MemberStatusDescription]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[MemberStatusDescription](@pid int)
RETURNS varchar(50)
AS
	BEGIN
	declare @desc varchar(50)
	select @desc = m.description from lookup.memberstatus m
	join dbo.People p on p.MemberStatusId = m.id
	where p.PeopleId = @pid
	return @desc
	END
GO
/****** Object:  Table [lookup].[PostalLookup]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[PostalLookup](
	[PostalCode] [varchar](15) NOT NULL,
	[CityName] [varchar](20) NULL,
	[StateCode] [varchar](20) NULL,
	[CountryName] [varchar](30) NULL,
	[ResCodeId] [int] NULL,
 CONSTRAINT [PK_POSTAL_LOOKUP_TBL] PRIMARY KEY CLUSTERED 
(
	[PostalCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [POSTAL_LOOKUP_CODE_IX] ON [lookup].[PostalLookup] 
(
	[PostalCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[SundayForWeek]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[SundayForWeek](@year INT, @week INT)
RETURNS datetime
AS
BEGIN

DECLARE @dt DATETIME 
SELECT @dt = DATEADD(MONTH,((@year-1900)*12),0) -- jan 1 for year
SELECT @dt = DATEADD(MONTH, 9, @dt) -- Oct 1 for year
SELECT @dt = DATEADD(d, -DATEPART(dw, @dt)+1, @dt) -- sunday of that week
IF DATEPART(MONTH, @dt) < 10 -- are we in september now?
	SELECT @dt = DATEADD(d, 7, @dt) -- next sunday (to get into october)
SELECT @dt = DATEADD(ww, @week - 1, @dt) -- sunday for week number

	-- Return the result of the function
	RETURN @dt

END
GO
/****** Object:  Table [lookup].[StateLookup]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[StateLookup](
	[StateCode] [varchar](2) NOT NULL,
	[StateName] [varchar](30) NULL,
 CONSTRAINT [PK_STATE_LOOKUP_TBL] PRIMARY KEY CLUSTERED 
(
	[StateCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [STATE_LOOKUP_CODE_IX] ON [lookup].[StateLookup] 
(
	[StateCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'AA', N'Armed Forces America')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'AE', N'Armed Forces East')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'AK', N'Alaska')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'AL', N'Alabama')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'AP', N'Armed Forces Pacific')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'AR', N'Arkansas')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'AZ', N'Arizona')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'CA', N'California')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'CO', N'Colorado')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'CT', N'Connecticut')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'CZ', N'Canal Zone')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'DC', N'District Of Columbia')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'DE', N'Delaware')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'FL', N'Florida')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'FR', N'Foreign Address')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'GA', N'Georgia')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'GU', N'Guam')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'HI', N'Hawaii')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'IA', N'Iowa')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'ID', N'Idaho')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'IL', N'Illinois')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'IN', N'Indiana')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'KS', N'Kansas')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'KY', N'Kentucky')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'LA', N'Louisiana')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'MA', N'Massachusetts')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'MD', N'Maryland')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'ME', N'Maine')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'MI', N'Michigan')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'MN', N'Minnesota')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'MO', N'Missouri')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'MS', N'Mississippi')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'MT', N'Montana')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'NC', N'North Carolina')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'ND', N'North Dakota')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'NE', N'Nebraska')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'NH', N'New Hampshire')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'NJ', N'New Jersey')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'NM', N'New Mexico')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'NV', N'Nevada')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'NY', N'New York')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'OH', N'Ohio')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'OK', N'Oklahoma')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'OR', N'Oregon')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'PA', N'Pennsylvania')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'PR', N'Puerto Rico')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'RI', N'Rhode Island')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'SC', N'South Carolina')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'SD', N'South Dakota')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'TN', N'Tennessee')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'TX', N'Texas')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'UT', N'Utah')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'VA', N'Virginia')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'VI', N'Virgin Islands')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'VT', N'Vermont')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'WA', N'Washington')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'WI', N'Wisconsin')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'WV', N'West Virginia')
INSERT [lookup].[StateLookup] ([StateCode], [StateName]) VALUES (N'WY', N'Wyoming')
/****** Object:  UserDefinedFunction [dbo].[StartsLower]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[StartsLower] (@s NVARCHAR) 
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ret bit

	SELECT @ret = 0
	-- Add the T-SQL statements to compute the return value here
	SELECT @ret = 1 WHERE @s COLLATE Latin1_General_BIN2 > 'Z'


	-- Return the result of the function
	RETURN @ret

END
GO
/****** Object:  Table [dbo].[RssFeed]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RssFeed](
	[Url] [varchar](150) NOT NULL,
	[Data] [varchar](max) NULL,
	[ETag] [varchar](150) NULL,
	[LastModified] [datetime] NULL,
 CONSTRAINT [PK_RssFeed] PRIMARY KEY CLUSTERED 
(
	[Url] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [lookup].[RollsheetType]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[RollsheetType](
	[Id] [int] NOT NULL,
	[Code] [varchar](10) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_RollsheetType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (0, N'N', N'None')
INSERT [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (1, N'S', N'Standard')
INSERT [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (2, N'SBD', N'Standard with birthday')
INSERT [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (4, N'YC', N'Younger Children (1-2 gr)')
INSERT [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (5, N'OP1', N'Older Preschool (4-5 yr)')
INSERT [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (6, N'OP2', N'Older Preschool (1-3 yr)')
INSERT [lookup].[RollsheetType] ([Id], [Code], [Description]) VALUES (7, N'BB', N'Bed Babies')
/****** Object:  Table [lookup].[ResidentCode]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[ResidentCode](
	[Id] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_ResidentCode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[ResidentCode] ([Id], [Code], [Description]) VALUES (10, N'M', N'Metro')
INSERT [lookup].[ResidentCode] ([Id], [Code], [Description]) VALUES (20, N'G', N'Marginal')
INSERT [lookup].[ResidentCode] ([Id], [Code], [Description]) VALUES (30, N'N', N'Non-Resident')
INSERT [lookup].[ResidentCode] ([Id], [Code], [Description]) VALUES (40, N'U', N'Unable to Locate')
/****** Object:  Table [dbo].[QueryBuilderClauses]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QueryBuilderClauses](
	[QueryId] [int] IDENTITY(1,1) NOT NULL,
	[ClauseOrder] [int] NOT NULL,
	[GroupId] [int] NULL,
	[Field] [varchar](32) NULL,
	[Comparison] [varchar](20) NULL,
	[TextValue] [varchar](100) NULL,
	[DateValue] [datetime] NULL,
	[CodeIdValue] [varchar](1000) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[DivOrg] [int] NOT NULL,
	[SubDivOrg] [int] NOT NULL,
	[Organization] [int] NOT NULL,
	[Days] [int] NOT NULL,
	[SavedBy] [varchar](50) NULL,
	[Description] [varchar](80) NULL,
	[IsPublic] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Quarters] [varchar](10) NULL,
	[SavedQueryIdDesc] [varchar](100) NULL,
	[Tags] [varchar](500) NULL,
	[Schedule] [int] NOT NULL,
 CONSTRAINT [PK_QueryBuilderClauses] PRIMARY KEY CLUSTERED 
(
	[QueryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_GroupId] ON [dbo].[QueryBuilderClauses] 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_QueryBuilderClauses] ON [dbo].[QueryBuilderClauses] 
(
	[SavedBy] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Promotions]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promotions](
	[PromotionId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[RecordStatus] [bit] NOT NULL,
	[ChurchId] [int] NOT NULL,
	[PromotionDate] [datetime] NULL,
	[OpenedDate] [datetime] NULL,
	[CompletedDate] [datetime] NULL,
	[PromotionName] [varchar](40) NOT NULL,
	[PromotionDescription] [varchar](256) NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PROMOTIONS_PK] PRIMARY KEY NONCLUSTERED 
(
	[PromotionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Zips]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Zips](
	[ZipCode] [varchar](10) NOT NULL,
	[MetroMarginalCode] [int] NULL,
 CONSTRAINT [PK_Zips] PRIMARY KEY CLUSTERED 
(
	[ZipCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[AttendMeetingInfo]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AttendMeetingInfo]
( @MeetingId INT, @PeopleId INT)
AS
BEGIN
	DECLARE @orgid INT
			,@meetingdate DATETIME
			,@meetdt DATE
			,@tm TIME
			,@dt DATETIME
			,@regularhour BIT
			,@membertypeid INT
			,@schedid INT
			,@attendedelsewhere INT
			,@allowoverlap BIT

	SELECT @regularhour = CASE WHEN EXISTS(
		SELECT null
			FROM dbo.Meetings m
			JOIN dbo.Organizations o ON m.OrganizationId = o.OrganizationId
			JOIN lookup.WeeklySchedule w ON o.ScheduleId = w.Id
			WHERE m.MeetingId = @MeetingId
				AND CONVERT(TIME, m.MeetingDate) = CONVERT(TIME, w.MeetingTime)
				AND w.[Day] = (DATEPART(weekday, m.MeetingDate) - 1))
		THEN 1 ELSE 0 END

	SELECT
		@orgid = m.OrganizationId,
		@schedid = o.ScheduleId,
		@dt = DATEADD(DAY, o.RollSheetVisitorWks * -7, m.MeetingDate),
		@meetingdate = m.MeetingDate,
		@allowoverlap = o.AllowAttendOverlap,
		@membertypeid = (SELECT om.MemberTypeId
					FROM dbo.OrganizationMembers om
					WHERE om.OrganizationId = o.OrganizationId
					AND om.PeopleId = @PeopleId)

	FROM dbo.Meetings m
	JOIN dbo.Organizations o ON m.OrganizationId = o.OrganizationId
	WHERE m.MeetingId = @MeetingId

	DECLARE @name VARCHAR(50), @bfclassid INT

	SELECT @name = p.[Name], @bfclassid = BibleFellowshipClassId
	FROM dbo.People p
	WHERE PeopleId = @PeopleId

	SELECT
		@meetdt = CONVERT(DATE, m.MeetingDate),
		@tm = CONVERT(TIME, m.MeetingDate)
	FROM dbo.Meetings m
	WHERE m.MeetingId = @MeetingId
		
	IF @dt IS NULL
		SELECT @dt = DATEADD(DAY, 3 * -7, @meetdt)

	DECLARE	@isrecentvisitor BIT
			--,@isinservice BIT
			,@isoffsite BIT
			--,@issamehour bit

	SELECT @isrecentvisitor = CASE WHEN exists(
				SELECT NULL FROM Attend
				WHERE PeopleId = @PeopleId
				AND AttendanceFlag = 1
				AND MeetingDate >= @dt
				AND MeetingDate <= @meetdt
				AND OrganizationId = @orgid
				AND AttendanceTypeId IN (50, 60)) -- new and recent
			THEN 1 ELSE 0 END

	--SELECT @isinservice = CASE WHEN exists(
	--			SELECT NULL FROM dbo.OrganizationMembers om
	--			JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
	--			WHERE om.PeopleId = @PeopleId
	--			AND om.OrganizationId <> @orgid
	--			AND om.MemberTypeId = 500 -- inservice member
	--			AND o.ScheduleId = @schedid)
	--		THEN 1 ELSE 0 END
			
	SELECT @isoffsite = CASE WHEN exists(
				SELECT NULL FROM dbo.OrganizationMembers om
				JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
				WHERE om.PeopleId = @PeopleId
				AND om.OrganizationId <> @orgid
				AND o.AttendClassificationId = 2 -- offsite
				AND o.FirstMeetingDate <= @meetdt
				AND @meetdt <= o.LastMeetingDate)
			THEN 1 ELSE 0 END

	DECLARE @otherattend INT, 
			@bfcattend INT, 
			@bfcid INT
	
	SELECT TOP 1 @otherattend = ae.AttendId
	FROM Attend ae
	JOIN dbo.Organizations o ON ae.OrganizationId = o.OrganizationId
	WHERE ae.PeopleId = @PeopleId
	AND ae.MeetingDate = @meetingdate
	AND ae.OrganizationId <> @orgid
	
	-- BFC class membership
	SELECT @bfcid = om.OrganizationId FROM dbo.OrganizationMembers om
	JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
	WHERE om.PeopleId = @PeopleId 
	AND om.OrganizationId <> @orgid
	AND o.ScheduleId = @schedid AND @regularhour = 1
	AND (om.OrganizationId = @bfclassid OR om.MemberTypeId = 500) -- regular or InSvc

	-- BFC Attendance at same time
	SELECT @bfcattend = a.AttendId FROM dbo.Attend a
	JOIN dbo.OrganizationMembers om ON a.OrganizationId = om.OrganizationId
	JOIN dbo.Organizations o ON om.OrganizationId = o.OrganizationId
	WHERE o.ScheduleId = @schedid AND @regularhour = 1
	AND a.MeetingDate = @meetingdate
	AND om.OrganizationId <> @orgid
	AND om.OrganizationId = @bfcid

	-- attended elsewhere at same time
	SELECT TOP 1 @attendedelsewhere = ae.AttendId 
	FROM Attend ae
	JOIN dbo.Organizations o ON ae.OrganizationId = o.OrganizationId
	WHERE ae.PeopleId = @PeopleId
	AND ae.AttendanceFlag = 1
	AND ae.MeetingDate = @meetingdate
	AND ae.OrganizationId NOT IN (@orgid, @bfcid)
	AND o.AllowAttendOverlap <> 1
	AND @allowoverlap <> 1

-- The returned records:
			
	SELECT
		 @attendedelsewhere AttendedElsewhere
		,@membertypeid MemberTypeId
		,@isoffsite IsOffSite
		,@isrecentvisitor IsRecentVisitor
		,@name Name
		,@bfclassid BFClassId
	
	-- Attend if any
	SELECT * FROM dbo.Attend
	WHERE MeetingId = @MeetingId AND PeopleId = @PeopleId
		
	-- the meeting
	SELECT * FROM dbo.Meetings
	WHERE MeetingId = @MeetingId
	
	-- Related VIP Attendance
	SELECT v.*
	FROM Attend v
	JOIN dbo.OrganizationMembers om ON v.OrganizationId = om.OrganizationId
	WHERE v.PeopleId = @PeopleId
	AND v.MeetingDate = @meetingdate
	AND v.OrganizationId <> @orgid
	AND om.MemberTypeId = 700 -- vip
	AND @orgid = @bfclassid
	
	-- BFC class membership 
	SELECT * FROM dbo.OrganizationMembers
	WHERE OrganizationId = @bfcid
	AND PeopleId = @PeopleId
	
	-- BFC Attendance at same time
	SELECT a.* FROM dbo.Attend a
	JOIN dbo.OrganizationMembers om ON a.OrganizationId = om.OrganizationId
	WHERE AttendId = @bfcattend

	-- BFC Meeting at same time
	SELECT m.* FROM dbo.Meetings m
	JOIN dbo.Attend a ON m.MeetingId = a.MeetingId
	WHERE AttendId = @bfcattend
	
END
GO
/****** Object:  Table [lookup].[VolunteerCodes]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [lookup].[VolunteerCodes](
	[Id] [int] NOT NULL,
	[Code] [varchar](10) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_VolunteerCodes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [lookup].[VolunteerCodes] ([Id], [Code], [Description]) VALUES (0, N'NA', N'None')
INSERT [lookup].[VolunteerCodes] ([Id], [Code], [Description]) VALUES (10, N'S', N'Standard')
INSERT [lookup].[VolunteerCodes] ([Id], [Code], [Description]) VALUES (30, N'L', N'Leader')
/****** Object:  StoredProcedure [dbo].[PopulateTagShares]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PopulateTagShares]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO dbo.TagShare (
		TagId,
		PeopleId
	) 
	SELECT DISTINCT t.Id, u.PeopleId 
	FROM dbo.TagShared t
	JOIN Users u ON t.SharedUser = u.Username
	
	UPDATE dbo.Tag
	SET PeopleId = u.PeopleId
	FROM dbo.Tag t
	JOIN dbo.Users u ON t.Owner = u.Username
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteTagForUser]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteTagForUser](@tag varchar, @user varchar)
AS
	/* SET NOCOUNT ON */
	declare @id int
	select @id = id from tag where name = @tag and @user = owner
	
	delete from tagorg where id = @id
	delete from tagtag where id = @id
	delete from tagperson where id = @id
	delete from tagshared where id = @id
	delete from tag where id = @id
	
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[ShowTableSizes]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ShowTableSizes]
AS
BEGIN
CREATE TABLE #temp (
       table_name sysname ,
       row_count int,
       reserved_size varchar(50),
       data_size varchar(50),
       index_size varchar(50),
       unused_size varchar(50))
SET NOCOUNT ON
INSERT     #temp
EXEC       sp_msforeachtable 'sp_spaceused ''?'''
SELECT     b.table_schema as owner,
		   a.table_name,
           a.row_count,
           count(*) as col_count,
           a.data_size
FROM       #temp a
INNER JOIN information_schema.columns b
           ON a.table_name collate database_default
                = b.table_name collate database_default
GROUP BY   b.table_schema, a.table_name, a.row_count, a.data_size
ORDER BY   a.row_count desc
DROP TABLE #temp
END
GO
/****** Object:  UserDefinedFunction [dbo].[Tool_VarbinaryToVarcharHex]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
<summary>
Based on ufn_VarbinaryToVarcharHex by Clay Beatty.
Used by Tool_ScriptDiagram2005

Function has two 'parts':

PART ONE: takes large VarbinaryValue chunks (greater than four bytes) 
and splits them into half, calling the function recursively with 
each half until the chunks are only four bytes long

PART TWO: notices the VarbinaryValue is four bytes or less, and 
starts actually processing these four byte chunks. It does this
by splitting the least-significant (rightmost) byte into two 
hexadecimal characters and recursively calling the function
with the more significant bytes until none remain (four recursive
calls in total).
</summary>
<author>Craig Dunn</author>
<remarks>
Clay Beatty's original function was written for Sql Server 2000.
Sql Server 2005 introduces the VARBINARY(max) datatype which this 
function now uses.

References
----------
1) MSDN: Using Large-Value Data Types
http://msdn2.microsoft.com/en-us/library/ms178158.aspx

2) Clay's "original" Script, Save, Export SQL 2000 Database Diagrams
http://www.thescripts.com/forum/thread81534.html or
http://groups-beta.google.com/group/comp.databases.ms-sqlserver/browse_frm/thread/ca9a9229d06a56f9?dq=&hl=en&lr=&ie=UTF-8&oe=UTF-8&prev=/groups%3Fdq%3D%26num%3D25%26hl%3Den%26lr%3D%26ie%3DUTF-8%26oe%3DUTF-8%26group%3Dcomp.databases.ms-sqlserver%26start%3D25
</remarks>
<param name="VarbinaryValue">binary data to be converted to Hexadecimal </param>
<returns>Hexadecimal representation of binary data, using chars [0-0a-f]</returns>
*/
CREATE FUNCTION [dbo].[Tool_VarbinaryToVarcharHex]
(
	@VarbinaryValue	VARBINARY(max)
)
RETURNS VARCHAR(max) AS
	BEGIN
	DECLARE @NumberOfBytes 	INT

	SET @NumberOfBytes = DATALENGTH(@VarbinaryValue)
	-- PART ONE --
	IF (@NumberOfBytes > 4)
	BEGIN
		DECLARE @FirstHalfNumberOfBytes INT
		DECLARE @SecondHalfNumberOfBytes INT
		SET @FirstHalfNumberOfBytes  = @NumberOfBytes/2
		SET @SecondHalfNumberOfBytes = @NumberOfBytes - @FirstHalfNumberOfBytes
		-- Call this function recursively with the two parts of the input split in half
		RETURN dbo.Tool_VarbinaryToVarcharHex(CAST(SUBSTRING(@VarbinaryValue, 1					        , @FirstHalfNumberOfBytes)  AS VARBINARY(max)))
			 + dbo.Tool_VarbinaryToVarcharHex(CAST(SUBSTRING(@VarbinaryValue, @FirstHalfNumberOfBytes+1 , @SecondHalfNumberOfBytes) AS VARBINARY(max)))
	END
	
	IF (@NumberOfBytes = 0)
	BEGIN
		RETURN ''	-- No bytes found, therefore no 'hex string' is returned
	END
	
	-- PART TWO --
	DECLARE @LowByte 		INT
	DECLARE @HighByte 		INT
	-- @NumberOfBytes <= 4 (four or less characters/8 hex digits were input)
	--						 eg. 88887777 66665555 44443333 22221111
	-- We'll process ONLY the right-most (least-significant) Byte, which consists
	-- of eight bits, or two hexadecimal values (eg. 22221111 --> XY) 
	-- where XY are two hex digits [0-f]

	-- 1. Carve off the rightmost four bits/single hex digit (ie 1111)
	--    BINARY AND 15 will result in a number with maxvalue of 15
	SET @LowByte = CAST(@VarbinaryValue AS INT) & 15
	-- Now determine which ASCII char value
	SET @LowByte = CASE 
	WHEN (@LowByte < 10)		-- 9 or less, convert to digits [0-9]
		THEN (48 + @LowByte)	-- 48 ASCII = 0 ... 57 ASCII = 9
		ELSE (87 + @LowByte)	-- else 10-15, convert to chars [a-f]
	END							-- (87+10)97 ASCII = a ... (87+15_102 ASCII = f

	-- 2. Carve off the rightmost eight bits/single hex digit (ie 22221111)
	--    Divide by 16 does a shift-left (now processing 2222)
	SET @HighByte = CAST(@VarbinaryValue AS INT) & 255
	SET @HighByte = (@HighByte / 16)
	-- Again determine which ASCII char value	
	SET @HighByte = CASE 
	WHEN (@HighByte < 10)		-- 9 or less, convert to digits [0-9]
		THEN (48 + @HighByte)	-- 48 ASCII = 0 ... 57 ASCII = 9
		ELSE (87 + @HighByte)	-- else 10-15, convert to chars [a-f]
	END							-- (87+10)97 ASCII = a ... (87+15)102 ASCII = f
	
	-- 3. Trim the byte (two hex values) from the right (least significant) input Binary
	--    in preparation for further parsing
	SET @VarbinaryValue = SUBSTRING(@VarbinaryValue, 1, (@NumberOfBytes-1))

	-- 4. Recursively call this method on the remaining Binary data, concatenating the two 
	--    hexadecimal 'values' we just decoded as their ASCII character representation
	--    ie. we pass 88887777 66665555 44443333 back to this function, adding XY to the result string
	RETURN dbo.Tool_VarbinaryToVarcharHex(@VarbinaryValue) + CHAR(@HighByte) + CHAR(@LowByte)
END
GO
/****** Object:  StoredProcedure [dbo].[Tool_ScriptDiagram2005]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
<summary>
Script Sql Server 2005 diagrams
(inspired by usp_ScriptDatabaseDiagrams for Sql Server 2000 by Clay Beatty)
</summary>
<example>
--NOTE: Scalar-valued Function [Tool_VarbinaryToVarcharHex] must exist before this script is run
EXEC Tool_ScriptDiagram2005 'DatabaseName'
</example>
<author>Craig Dunn</author>
<remarks>
Helpful Articles
----------------
1) Upload / Download to Sql 2005
http://staceyw.spaces.live.com/blog/cns!F4A38E96E598161E!404.entry

2) MSDN: Using Large-Value Data Types
http://msdn2.microsoft.com/en-us/library/ms178158.aspx

3) "original" Script, Save, Export SQL 2000 Database Diagrams
http://www.thescripts.com/forum/thread81534.html
http://groups-beta.google.com/group/comp.databases.ms-sqlserver/browse_frm/thread/ca9a9229d06a56f9?dq=&hl=en&lr=&ie=UTF-8&oe=UTF-8&prev=/groups%3Fdq%3D%26num%3D25%26hl%3Den%26lr%3D%26ie%3DUTF-8%26oe%3DUTF-8%26group%3Dcomp.databases.ms-sqlserver%26start%3D25
</remarks>
<param name="name">Name of the diagram in the Sql Server database instance</param>
*/
CREATE PROCEDURE [dbo].[Tool_ScriptDiagram2005]
(
	@name VARCHAR(128)
)
AS
BEGIN
	DECLARE @diagram_id		INT
	DECLARE @index			INT
	DECLARE @size			INT
	DECLARE @chunk			INT
	DECLARE @line			VARCHAR(max)
	-- Set start index, and chunk 'constant' value
	SET @index = 1  -- 
	SET @chunk = 32	-- values that work: 2, 6
					-- values that fail: 15,16, 64
	-- Get PK diagram_id using the diagram's name (which is what the user is familiar with)
	SELECT 
		@diagram_id=diagram_id	
	,	@size = DATALENGTH(definition) 
	FROM sysdiagrams 
	WHERE [name] = @name

	IF @diagram_id IS NULL
	BEGIN
		PRINT '/**<error>
Diagram name [' + @name + '] could not be found.
</error>*/' 
	END
	ELSE -- Diagram exists
	BEGIN
		-- Now with the diagram_id, do all the work
		PRINT '/**'
		PRINT '<summary>'
		PRINT 'Restore diagram ''' + @name + ''''
		PRINT '</summary>'
		PRINT '<remarks>'
		PRINT 'Generated by Tool_ScriptDiagram2005/Tool_VarbinaryToVarcharHex'
		PRINT 'Will attempt to create [sysdiagrams] table if it doesn''t already exist'
		PRINT '</remarks>'
		PRINT '<generated>' + LEFT(CONVERT(VARCHAR(23), GETDATE(), 121), 16) + '</generated>'
		PRINT '*/'
		PRINT 'PRINT ''=== Tool_ScriptDiagram2005 restore diagram [' + @name + '] ==='''
		PRINT '	-- If the sysdiagrams table has not been created in this database, create it!
				IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = ''sysdiagrams'')
				BEGIN
					-- Create table script generated by Sql Server Management Studio
					-- _Assume_ this is roughly equivalent to what Sql Server/Management Studio
					-- creates the first time you add a diagram to a 2005 database
					CREATE TABLE [dbo].[sysdiagrams](
						[name] [sysname] NOT NULL,
						[principal_id] [int] NOT NULL,
						[diagram_id] [int] IDENTITY(1,1) NOT NULL,
						[version] [int] NULL,
						[definition] [varbinary](max) NULL,
					PRIMARY KEY CLUSTERED 
					(
						[diagram_id] ASC
					)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ,
					 CONSTRAINT [UK_principal_name] UNIQUE NONCLUSTERED 
					(
						[principal_id] ASC,
						[name] ASC
					)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) 
					) 
					EXEC sys.sp_addextendedproperty @name=N''microsoft_database_tools_support'', @value=1 , @level0type=N''SCHEMA'',@level0name=N''dbo'', @level1type=N''TABLE'',@level1name=N''sysdiagrams''
					PRINT ''[sysdiagrams] table was created as it did not already exist''
				END
				-- Target table will now exist, if it didn''t before'
		PRINT 'SET NOCOUNT ON -- Hide (1 row affected) messages'
		PRINT 'DECLARE @newid INT'
		PRINT 'DECLARE @DiagramSuffix          varchar (50)'
		PRINT ''
		PRINT 'PRINT ''Suffix diagram name with date, to ensure uniqueness'''	
		PRINT 'SET @DiagramSuffix = '' '' + LEFT(CONVERT(VARCHAR(23), GETDATE(), 121), 16)'
		PRINT ''
		PRINT 'PRINT ''Create row for new diagram'''
		-- Output the INSERT that _creates_ the diagram record, with a non-NULL [definition],
		-- important because .WRITE *cannot* be called against a NULL value (in the WHILE loop)
		-- so we insert 0x so that .WRITE has 'something' to append to...
		PRINT 'BEGIN TRY'
		PRINT '    PRINT ''Write diagram ' + @name + ' into new row (and get [diagram_id])'''
		SELECT @line =  
			  '    INSERT INTO sysdiagrams ([name], [principal_id], [version], [definition])'
			+ ' VALUES (''' + [name] + '''+@DiagramSuffix, '+ CAST (principal_id AS VARCHAR(100))+', '+CAST (version AS VARCHAR(100))+', 0x)'
		FROM sysdiagrams WHERE diagram_id = @diagram_id
		PRINT @line
		PRINT '    SET @newid = SCOPE_IDENTITY()'
		PRINT 'END TRY'
		PRINT 'BEGIN CATCH'
		PRINT '    PRINT ''XxXxX '' + Error_Message() + '' XxXxX'''
		PRINT '    PRINT ''XxXxX END Tool_ScriptDiagram2005 - fix the error before running again XxXxX'''
		PRINT '    RETURN'
		PRINT 'END CATCH'
		PRINT ''
		PRINT 'PRINT ''Now add all the binary data...'''
		PRINT 'BEGIN TRY'
		WHILE @index < @size
		BEGIN
			-- Output as many UPDATE statements as required to append all the diagram binary
			-- data, represented as hexadecimal strings
			SELECT @line =  
				 '    UPDATE sysdiagrams SET [definition] .Write ('
				+ ' 0x' + UPPER(dbo.Tool_VarbinaryToVarcharHex (SUBSTRING (definition, @index, @chunk)))
				+ ', null, 0) WHERE diagram_id = @newid -- index:' + CAST(@index AS VARCHAR(100))
			FROM	sysdiagrams 
			WHERE	diagram_id = @diagram_id
			PRINT @line
			SET @index = @index + @chunk
		END
		PRINT ''
		PRINT '    PRINT ''=== Finished writing diagram id '' + CAST(@newid AS VARCHAR(100)) + ''  ==='''
		PRINT '    PRINT ''=== Refresh your Databases-[DbName]-Database Diagrams to see the new diagram ==='''
		PRINT 'END TRY'
		PRINT 'BEGIN CATCH'
		PRINT '    -- If we got here, the [definition] updates didn''t complete, so delete the diagram row'
		PRINT '    -- (and hope it doesn''t fail!)'
		PRINT '    DELETE FROM sysdiagrams WHERE diagram_id = @newid'
		PRINT '    PRINT ''XxXxX '' + Error_Message() + '' XxXxX'''
		PRINT '    PRINT ''XxXxX END Tool_ScriptDiagram2005 - fix the error before running again XxXxX'''
		PRINT '    RETURN'
		PRINT 'END CATCH'
	END
END
GO
/****** Object:  Table [dbo].[PromotionControl]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PromotionControl](
	[PromotionId] [int] NOT NULL,
	[PromotionControlId] [int] NOT NULL,
	[ScheduleId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[RecordStatus] [bit] NOT NULL,
	[DivisionId] [int] NOT NULL,
	[GenderControlId] [int] NOT NULL,
	[TeacherControl] [bit] NOT NULL,
	[MixControl] [bit] NOT NULL,
	[NbrOfClasses] [int] NOT NULL,
	[NbrOfFemaleClasses] [int] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[OldDiisionId] [int] NULL,
 CONSTRAINT [PROMOTION_CONTROL_PK] PRIMARY KEY NONCLUSTERED 
(
	[PromotionId] ASC,
	[PromotionControlId] ASC,
	[ScheduleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[DaysSinceContact]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[DaysSinceContact](@pid INT)
RETURNS int
AS
BEGIN
	DECLARE @days int

	SELECT @days = MIN(DATEDIFF(D,c.ContactDate,GETDATE())) FROM dbo.NewContact c
	JOIN dbo.Contactees ce ON c.ContactId = ce.ContactId
	WHERE ce.PeopleId = @pid

	RETURN @days

END
GO
/****** Object:  Table [dbo].[MemberTags]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberTags](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[ProgId] [int] NULL,
 CONSTRAINT [PK_MemberTags] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrgMemMemTags]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrgMemMemTags](
	[OrgId] [int] NOT NULL,
	[PeopleId] [int] NOT NULL,
	[MemberTagId] [int] NOT NULL,
 CONSTRAINT [PK_OrgMemMemTags] PRIMARY KEY CLUSTERED 
(
	[OrgId] ASC,
	[PeopleId] ASC,
	[MemberTagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[MemberDesc]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[MemberDesc](@id int) 
RETURNS VARCHAR(100)
AS
BEGIN
	DECLARE @ret VARCHAR(100)
	SELECT @ret =  Description FROM lookup.MemberType WHERE id = @id
	RETURN @ret
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteAllQueriesWithNoChildren]    Script Date: 05/06/2009 19:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAllQueriesWithNoChildren]
AS
BEGIN
	SET NOCOUNT ON;

delete from QueryBuilderClauses where queryid in (select q.queryid
FROM       QueryBuilderClauses q
where not exists (select null from QueryBuilderClauses where groupid = q.queryid))

END
GO
/****** Object:  UserDefinedFunction [dbo].[MembersAsOf]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[MembersAsOf]
(	
	@from DATETIME,
	@to DATETIME,
	@progid INT,
	@divid INT,
	@orgid INT
)
RETURNS @t TABLE ( PeopleId INT )
AS
BEGIN
	INSERT INTO @t (PeopleId) SELECT p.PeopleId FROM dbo.People p
	WHERE
	EXISTS (
		SELECT NULL FROM dbo.EnrollmentTransaction et
		WHERE et.PeopleId = p.PeopleId
		AND et.TransactionTypeId <= 3
		AND @from <= COALESCE(et.NextTranChangeDate, GETDATE())
		AND et.TransactionDate <= @to
		AND (et.OrganizationId = @orgid OR @orgid = 0)
		AND (EXISTS(SELECT NULL FROM DivOrg d1
				WHERE d1.OrgId = et.OrganizationId
				AND d1.DivId = @divid) OR @divid = 0)
		AND (EXISTS(SELECT NULL FROM DivOrg d2
				WHERE d2.OrgId = et.OrganizationId
				AND EXISTS(SELECT NULL FROM Division d
						WHERE d2.DivId = d.Id
						AND d.ProgId = @progid)) OR @progid = 0)
		)
	RETURN
END
GO
/****** Object:  Table [dbo].[BFCSummaryOrgTags]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BFCSummaryOrgTags](
	[SortOrder] [int] NOT NULL,
	[OrgTagId] [int] NOT NULL,
 CONSTRAINT [PK_BFCSummaryOrgTags_1] PRIMARY KEY CLUSTERED 
(
	[OrgTagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[AttendMemberTypeAsOf]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[AttendMemberTypeAsOf]
(	
	@from DATETIME,
	@to DATETIME,
	@progid INT,
	@divid INT,
	@orgid INT,
	@not BIT,
	@ids VARCHAR(4000)
)
RETURNS @t TABLE ( PeopleId INT )
AS
BEGIN
	INSERT INTO @t (PeopleId) SELECT p.PeopleId FROM dbo.People p
	WHERE EXISTS (
		SELECT NULL FROM dbo.Attend a
		WHERE a.PeopleId = p.PeopleId
		AND (@not = 1 OR a.MemberTypeId IN (SELECT id FROM CsvTable(@ids)))
		AND (@not = 0 OR a.MemberTypeId NOT IN (SELECT id FROM CsvTable(@ids)))
		AND a.AttendanceFlag = 1
		AND a.MeetingDate >= @from
		AND a.MeetingDate < @to
		AND (a.OrganizationId = @orgid OR @orgid = 0)
		AND (EXISTS(SELECT NULL FROM DivOrg d1
				WHERE d1.OrgId = a.OrganizationId
				AND d1.DivId = @divid) OR @divid = 0)
		AND (EXISTS(SELECT NULL FROM DivOrg d2
				WHERE d2.OrgId = a.OrganizationId
				AND EXISTS(SELECT NULL FROM Division d
						WHERE d2.DivId = d.Id
						AND d.ProgId = @progid)) OR @progid = 0)
		)
	RETURN
END
GO
/****** Object:  Table [dbo].[BundleHeader]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BundleHeader](
	[BundleHeaderId] [int] IDENTITY(1,1) NOT NULL,
	[ChurchId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[RecordStatus] [bit] NOT NULL,
	[BundleStatusId] [int] NOT NULL,
	[ContributionDate] [datetime] NOT NULL,
	[BundleHeaderTypeId] [int] NOT NULL,
	[DepositDate] [datetime] NULL,
	[BundleTotal] [numeric](10, 2) NULL,
	[TotalCash] [numeric](10, 2) NULL,
	[TotalChecks] [numeric](10, 2) NULL,
	[TotalEnvelopes] [numeric](10, 2) NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[FundId] [int] NULL,
 CONSTRAINT [BUNDLE_HEADER_PK] PRIMARY KEY NONCLUSTERED 
(
	[BundleHeaderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [BUNDLE_HEADER_CHURCH_FK_IX] ON [dbo].[BundleHeader] 
(
	[ChurchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BundleDetail]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BundleDetail](
	[BundleDetailId] [int] IDENTITY(1,1) NOT NULL,
	[BundleHeaderId] [int] NOT NULL,
	[ContributionId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [BUNDLE_DETAIL_PK] PRIMARY KEY NONCLUSTERED 
(
	[BundleDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [BUNDLE_DETAIL_BUNDLE_FK_IX] ON [dbo].[BundleDetail] 
(
	[BundleHeaderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [BUNDLE_DETAIL_CONTR_FK_IX] ON [dbo].[BundleDetail] 
(
	[ContributionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditValues]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditValues](
	[AuditId] [int] NOT NULL,
	[MemberName] [varchar](50) NOT NULL,
	[OldValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AuditValues] PRIMARY KEY CLUSTERED 
(
	[AuditId] ASC,
	[MemberName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[AttendDesc]    Script Date: 05/06/2009 19:26:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[AttendDesc](@id int) 
RETURNS VARCHAR(100)
AS
BEGIN
	DECLARE @ret VARCHAR(100)
	SELECT @ret =  Description FROM lookup.AttendType WHERE id = @id
	RETURN @ret
END
GO
/****** Object:  Default [DF_Attend_AttendanceFlag]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Attend] ADD  CONSTRAINT [DF_Attend_AttendanceFlag]  DEFAULT ((0)) FOR [AttendanceFlag]
GO
/****** Object:  Default [DF_Attend_OtherAttends]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Attend] ADD  CONSTRAINT [DF_Attend_OtherAttends]  DEFAULT ((0)) FOR [OtherAttends]
GO
/****** Object:  Default [DF_ENROLLMENT_TRANSACTION_TBL_TRANSACTION_STATUS]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[EnrollmentTransaction] ADD  CONSTRAINT [DF_ENROLLMENT_TRANSACTION_TBL_TRANSACTION_STATUS]  DEFAULT ((0)) FOR [TransactionStatus]
GO
/****** Object:  Default [DF_FAMILIES_TBL_RECORD_STATUS]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Families] ADD  CONSTRAINT [DF_FAMILIES_TBL_RECORD_STATUS]  DEFAULT ((0)) FOR [RecordStatus]
GO
/****** Object:  Default [DF_GeoCodes_Latitude]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[GeoCodes] ADD  CONSTRAINT [DF_GeoCodes_Latitude]  DEFAULT ((0)) FOR [Latitude]
GO
/****** Object:  Default [DF_GeoCodes_Longitude]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[GeoCodes] ADD  CONSTRAINT [DF_GeoCodes_Longitude]  DEFAULT ((0)) FOR [Longitude]
GO
/****** Object:  Default [DF__MEETINGS___NUM_P__4D4B3A2F]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Meetings] ADD  CONSTRAINT [DF__MEETINGS___NUM_P__4D4B3A2F]  DEFAULT ((0)) FOR [NumPresent]
GO
/****** Object:  Default [DF__MEETINGS___NUM_M__4F3382A1]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Meetings] ADD  CONSTRAINT [DF__MEETINGS___NUM_M__4F3382A1]  DEFAULT ((0)) FOR [NumMembers]
GO
/****** Object:  Default [DF__MEETINGS___NUM_V__5027A6DA]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Meetings] ADD  CONSTRAINT [DF__MEETINGS___NUM_V__5027A6DA]  DEFAULT ((0)) FOR [NumVstMembers]
GO
/****** Object:  Default [DF__MEETINGS___NUM_R__511BCB13]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Meetings] ADD  CONSTRAINT [DF__MEETINGS___NUM_R__511BCB13]  DEFAULT ((0)) FOR [NumRepeatVst]
GO
/****** Object:  Default [DF__MEETINGS___NUM_N__520FEF4C]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Meetings] ADD  CONSTRAINT [DF__MEETINGS___NUM_N__520FEF4C]  DEFAULT ((0)) FOR [NumNewVisit]
GO
/****** Object:  Default [DF__MEETINGS___GROUP__5AA5354D]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Meetings] ADD  CONSTRAINT [DF__MEETINGS___GROUP__5AA5354D]  DEFAULT ((0)) FOR [GroupMeetingFlag]
GO
/****** Object:  Default [DF_Organizations_AllowAttendOverlap]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Organizations] ADD  CONSTRAINT [DF_Organizations_AllowAttendOverlap]  DEFAULT ((0)) FOR [AllowAttendOverlap]
GO
/****** Object:  Default [DF_Organizations_AttendClassificationId]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Organizations] ADD  CONSTRAINT [DF_Organizations_AttendClassificationId]  DEFAULT ((0)) FOR [AttendClassificationId]
GO
/****** Object:  Default [DF_PEOPLE_TBL_GENDER_ID]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_PEOPLE_TBL_GENDER_ID]  DEFAULT ((0)) FOR [GenderId]
GO
/****** Object:  Default [DF_PEOPLE_TBL_DO_NOT_MAIL_FLAG]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_PEOPLE_TBL_DO_NOT_MAIL_FLAG]  DEFAULT ((0)) FOR [DoNotMailFlag]
GO
/****** Object:  Default [DF_PEOPLE_TBL_DO_NOT_CALL_FLAG]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_PEOPLE_TBL_DO_NOT_CALL_FLAG]  DEFAULT ((0)) FOR [DoNotCallFlag]
GO
/****** Object:  Default [DF_PEOPLE_TBL_DO_NOT_VISIT_FLAG]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_PEOPLE_TBL_DO_NOT_VISIT_FLAG]  DEFAULT ((0)) FOR [DoNotVisitFlag]
GO
/****** Object:  Default [DF_PEOPLE_TBL_ADDRESS_TYPE_ID]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_PEOPLE_TBL_ADDRESS_TYPE_ID]  DEFAULT ((10)) FOR [AddressTypeId]
GO
/****** Object:  Default [DF_PEOPLE_TBL_PHONE_PREF_ID]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_PEOPLE_TBL_PHONE_PREF_ID]  DEFAULT ((10)) FOR [PhonePrefId]
GO
/****** Object:  Default [DF_PEOPLE_TBL_JOIN_CODE_ID]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_PEOPLE_TBL_JOIN_CODE_ID]  DEFAULT ((0)) FOR [JoinCodeId]
GO
/****** Object:  Default [DF_PEOPLE_TBL_CHRIST_AS_SAVIOR]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_PEOPLE_TBL_CHRIST_AS_SAVIOR]  DEFAULT ((0)) FOR [ChristAsSavior]
GO
/****** Object:  Default [DF_PEOPLE_TBL_INTERESTED_IN_JOINING]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_PEOPLE_TBL_INTERESTED_IN_JOINING]  DEFAULT ((0)) FOR [InterestedInJoining]
GO
/****** Object:  Default [DF_PEOPLE_TBL_PLEASE_VISIT]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_PEOPLE_TBL_PLEASE_VISIT]  DEFAULT ((0)) FOR [PleaseVisit]
GO
/****** Object:  Default [DF_PEOPLE_TBL_INFO_BECOME_A_CHRISTIAN]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_PEOPLE_TBL_INFO_BECOME_A_CHRISTIAN]  DEFAULT ((0)) FOR [InfoBecomeAChristian]
GO
/****** Object:  Default [DF_PEOPLE_TBL_CONTRIBUTIONS_STATEMENT]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_PEOPLE_TBL_CONTRIBUTIONS_STATEMENT]  DEFAULT ((0)) FOR [ContributionsStatement]
GO
/****** Object:  Default [DF_QueryBuilderClauses_ClauseOrder]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[QueryBuilderClauses] ADD  CONSTRAINT [DF_QueryBuilderClauses_ClauseOrder]  DEFAULT ((0)) FOR [ClauseOrder]
GO
/****** Object:  Default [DF_QueryBuilderClauses_DivOrgs]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[QueryBuilderClauses] ADD  CONSTRAINT [DF_QueryBuilderClauses_DivOrgs]  DEFAULT ((0)) FOR [DivOrg]
GO
/****** Object:  Default [DF_QueryBuilderClauses_SubDivOrgs]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[QueryBuilderClauses] ADD  CONSTRAINT [DF_QueryBuilderClauses_SubDivOrgs]  DEFAULT ((0)) FOR [SubDivOrg]
GO
/****** Object:  Default [DF_QueryBuilderClauses_Organization]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[QueryBuilderClauses] ADD  CONSTRAINT [DF_QueryBuilderClauses_Organization]  DEFAULT ((0)) FOR [Organization]
GO
/****** Object:  Default [DF_QueryBuilderClauses_Days]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[QueryBuilderClauses] ADD  CONSTRAINT [DF_QueryBuilderClauses_Days]  DEFAULT ((0)) FOR [Days]
GO
/****** Object:  Default [QueryBuilderIsPublic]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[QueryBuilderClauses] ADD  CONSTRAINT [QueryBuilderIsPublic]  DEFAULT ((0)) FOR [IsPublic]
GO
/****** Object:  Default [DF_QueryBuilderClauses_ModifiedOn]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[QueryBuilderClauses] ADD  CONSTRAINT [DF_QueryBuilderClauses_ModifiedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
/****** Object:  Default [DF_QueryBuilderClauses_SavedQueryId]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[QueryBuilderClauses] ADD  CONSTRAINT [DF_QueryBuilderClauses_SavedQueryId]  DEFAULT ((0)) FOR [SavedQueryIdDesc]
GO
/****** Object:  Default [DF_QueryBuilderClauses_Schedule]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[QueryBuilderClauses] ADD  CONSTRAINT [DF_QueryBuilderClauses_Schedule]  DEFAULT ((0)) FOR [Schedule]
GO
/****** Object:  Default [DF_Tag_TypeId]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Tag] ADD  CONSTRAINT [DF_Tag_TypeId]  DEFAULT ((1)) FOR [TypeId]
GO
/****** Object:  Default [DF_Task_Archive]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_Archive]  DEFAULT ((0)) FOR [Archive]
GO
/****** Object:  Default [DF_Users_IsApproved]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
/****** Object:  Default [DF_Users_IsLockedOut]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsLockedOut]  DEFAULT ((0)) FOR [IsLockedOut]
GO
/****** Object:  Default [DF_Users_FailedPasswordAttemptCount]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_FailedPasswordAttemptCount]  DEFAULT ((0)) FOR [FailedPasswordAttemptCount]
GO
/****** Object:  Default [DF_Users_FailedPasswordAnswerAttemptCount]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_FailedPasswordAnswerAttemptCount]  DEFAULT ((0)) FOR [FailedPasswordAnswerAttemptCount]
GO
/****** Object:  Default [DF_Users_MustChangePassword]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_MustChangePassword]  DEFAULT ((0)) FOR [MustChangePassword]
GO
/****** Object:  Default [DF_Volunteer_Standard]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Volunteer] ADD  CONSTRAINT [DF_Volunteer_Standard]  DEFAULT ((0)) FOR [Standard]
GO
/****** Object:  Default [DF_Volunteer_Children]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Volunteer] ADD  CONSTRAINT [DF_Volunteer_Children]  DEFAULT ((0)) FOR [Children]
GO
/****** Object:  Default [DF_Volunteer_Leader]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Volunteer] ADD  CONSTRAINT [DF_Volunteer_Leader]  DEFAULT ((0)) FOR [Leader]
GO
/****** Object:  ForeignKey [FK_ActivityLog_Users]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[ActivityLog]  WITH CHECK ADD  CONSTRAINT [FK_ActivityLog_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[ActivityLog] CHECK CONSTRAINT [FK_ActivityLog_Users]
GO
/****** Object:  ForeignKey [FK_Attend_MemberType]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Attend]  WITH CHECK ADD  CONSTRAINT [FK_Attend_MemberType] FOREIGN KEY([MemberTypeId])
REFERENCES [lookup].[MemberType] ([Id])
GO
ALTER TABLE [dbo].[Attend] CHECK CONSTRAINT [FK_Attend_MemberType]
GO
/****** Object:  ForeignKey [FK_AttendWithAbsents_TBL_AttendType]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Attend]  WITH CHECK ADD  CONSTRAINT [FK_AttendWithAbsents_TBL_AttendType] FOREIGN KEY([AttendanceTypeId])
REFERENCES [lookup].[AttendType] ([Id])
GO
ALTER TABLE [dbo].[Attend] CHECK CONSTRAINT [FK_AttendWithAbsents_TBL_AttendType]
GO
/****** Object:  ForeignKey [FK_AttendWithAbsents_TBL_MEETINGS_TBL]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Attend]  WITH CHECK ADD  CONSTRAINT [FK_AttendWithAbsents_TBL_MEETINGS_TBL] FOREIGN KEY([MeetingId])
REFERENCES [dbo].[Meetings] ([MeetingId])
GO
ALTER TABLE [dbo].[Attend] CHECK CONSTRAINT [FK_AttendWithAbsents_TBL_MEETINGS_TBL]
GO
/****** Object:  ForeignKey [FK_AttendWithAbsents_TBL_ORGANIZATIONS_TBL]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Attend]  WITH CHECK ADD  CONSTRAINT [FK_AttendWithAbsents_TBL_ORGANIZATIONS_TBL] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Attend] CHECK CONSTRAINT [FK_AttendWithAbsents_TBL_ORGANIZATIONS_TBL]
GO
/****** Object:  ForeignKey [FK_AttendWithAbsents_TBL_PEOPLE_TBL]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Attend]  WITH CHECK ADD  CONSTRAINT [FK_AttendWithAbsents_TBL_PEOPLE_TBL] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Attend] CHECK CONSTRAINT [FK_AttendWithAbsents_TBL_PEOPLE_TBL]
GO
/****** Object:  ForeignKey [FK_AuditValues_Audits]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[AuditValues]  WITH CHECK ADD  CONSTRAINT [FK_AuditValues_Audits] FOREIGN KEY([AuditId])
REFERENCES [dbo].[Audits] ([Id])
GO
ALTER TABLE [dbo].[AuditValues] CHECK CONSTRAINT [FK_AuditValues_Audits]
GO
/****** Object:  ForeignKey [FK_BadET_EnrollmentTransaction]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[BadET]  WITH CHECK ADD  CONSTRAINT [FK_BadET_EnrollmentTransaction] FOREIGN KEY([TranId])
REFERENCES [dbo].[EnrollmentTransaction] ([TransactionId])
GO
ALTER TABLE [dbo].[BadET] CHECK CONSTRAINT [FK_BadET_EnrollmentTransaction]
GO
/****** Object:  ForeignKey [FK_BadET_Organizations]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[BadET]  WITH CHECK ADD  CONSTRAINT [FK_BadET_Organizations] FOREIGN KEY([OrgId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[BadET] CHECK CONSTRAINT [FK_BadET_Organizations]
GO
/****** Object:  ForeignKey [FK_BadET_People]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[BadET]  WITH CHECK ADD  CONSTRAINT [FK_BadET_People] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[BadET] CHECK CONSTRAINT [FK_BadET_People]
GO
/****** Object:  ForeignKey [FK_BFCSummaryOrgTags_Division]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[BFCSummaryOrgTags]  WITH CHECK ADD  CONSTRAINT [FK_BFCSummaryOrgTags_Division] FOREIGN KEY([OrgTagId])
REFERENCES [dbo].[Division] ([Id])
GO
ALTER TABLE [dbo].[BFCSummaryOrgTags] CHECK CONSTRAINT [FK_BFCSummaryOrgTags_Division]
GO
/****** Object:  ForeignKey [FK_BFCSummaryOrgTags_Tag]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[BFCSummaryOrgTags]  WITH CHECK ADD  CONSTRAINT [FK_BFCSummaryOrgTags_Tag] FOREIGN KEY([OrgTagId])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[BFCSummaryOrgTags] CHECK CONSTRAINT [FK_BFCSummaryOrgTags_Tag]
GO
/****** Object:  ForeignKey [BUNDLE_DETAIL_BUNDLE_FK]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[BundleDetail]  WITH NOCHECK ADD  CONSTRAINT [BUNDLE_DETAIL_BUNDLE_FK] FOREIGN KEY([BundleHeaderId])
REFERENCES [dbo].[BundleHeader] ([BundleHeaderId])
GO
ALTER TABLE [dbo].[BundleDetail] CHECK CONSTRAINT [BUNDLE_DETAIL_BUNDLE_FK]
GO
/****** Object:  ForeignKey [BUNDLE_DETAIL_CONTR_FK]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[BundleDetail]  WITH NOCHECK ADD  CONSTRAINT [BUNDLE_DETAIL_CONTR_FK] FOREIGN KEY([ContributionId])
REFERENCES [dbo].[Contribution] ([ContributionId])
GO
ALTER TABLE [dbo].[BundleDetail] CHECK CONSTRAINT [BUNDLE_DETAIL_CONTR_FK]
GO
/****** Object:  ForeignKey [BundleHeaders__Fund]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[BundleHeader]  WITH CHECK ADD  CONSTRAINT [BundleHeaders__Fund] FOREIGN KEY([FundId])
REFERENCES [dbo].[ContributionFund] ([FundId])
GO
ALTER TABLE [dbo].[BundleHeader] CHECK CONSTRAINT [BundleHeaders__Fund]
GO
/****** Object:  ForeignKey [FK_BUNDLE_HEADER_TBL_BundleHeaderTypes]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[BundleHeader]  WITH CHECK ADD  CONSTRAINT [FK_BUNDLE_HEADER_TBL_BundleHeaderTypes] FOREIGN KEY([BundleHeaderTypeId])
REFERENCES [lookup].[BundleHeaderTypes] ([Id])
GO
ALTER TABLE [dbo].[BundleHeader] CHECK CONSTRAINT [FK_BUNDLE_HEADER_TBL_BundleHeaderTypes]
GO
/****** Object:  ForeignKey [FK_BUNDLE_HEADER_TBL_BundleStatusTypes]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[BundleHeader]  WITH CHECK ADD  CONSTRAINT [FK_BUNDLE_HEADER_TBL_BundleStatusTypes] FOREIGN KEY([BundleStatusId])
REFERENCES [lookup].[BundleStatusTypes] ([Id])
GO
ALTER TABLE [dbo].[BundleHeader] CHECK CONSTRAINT [FK_BUNDLE_HEADER_TBL_BundleStatusTypes]
GO
/****** Object:  ForeignKey [contactees__contact]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Contactees]  WITH CHECK ADD  CONSTRAINT [contactees__contact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[NewContact] ([ContactId])
GO
ALTER TABLE [dbo].[Contactees] CHECK CONSTRAINT [contactees__contact]
GO
/****** Object:  ForeignKey [contactsHad__person]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Contactees]  WITH CHECK ADD  CONSTRAINT [contactsHad__person] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Contactees] CHECK CONSTRAINT [contactsHad__person]
GO
/****** Object:  ForeignKey [contactsMade__person]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Contactors]  WITH CHECK ADD  CONSTRAINT [contactsMade__person] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Contactors] CHECK CONSTRAINT [contactsMade__person]
GO
/****** Object:  ForeignKey [contactsMakers__contact]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Contactors]  WITH CHECK ADD  CONSTRAINT [contactsMakers__contact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[NewContact] ([ContactId])
GO
ALTER TABLE [dbo].[Contactors] CHECK CONSTRAINT [contactsMakers__contact]
GO
/****** Object:  ForeignKey [FK_Contribution_ContributionFund]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Contribution]  WITH CHECK ADD  CONSTRAINT [FK_Contribution_ContributionFund] FOREIGN KEY([FundId])
REFERENCES [dbo].[ContributionFund] ([FundId])
GO
ALTER TABLE [dbo].[Contribution] CHECK CONSTRAINT [FK_Contribution_ContributionFund]
GO
/****** Object:  ForeignKey [FK_Contribution_ContributionStatus]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Contribution]  WITH CHECK ADD  CONSTRAINT [FK_Contribution_ContributionStatus] FOREIGN KEY([ContributionStatusId])
REFERENCES [lookup].[ContributionStatus] ([Id])
GO
ALTER TABLE [dbo].[Contribution] CHECK CONSTRAINT [FK_Contribution_ContributionStatus]
GO
/****** Object:  ForeignKey [FK_Contribution_ContributionType]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Contribution]  WITH CHECK ADD  CONSTRAINT [FK_Contribution_ContributionType] FOREIGN KEY([ContributionTypeId])
REFERENCES [lookup].[ContributionType] ([Id])
GO
ALTER TABLE [dbo].[Contribution] CHECK CONSTRAINT [FK_Contribution_ContributionType]
GO
/****** Object:  ForeignKey [FK_Contribution_People]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Contribution]  WITH CHECK ADD  CONSTRAINT [FK_Contribution_People] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Contribution] CHECK CONSTRAINT [FK_Contribution_People]
GO
/****** Object:  ForeignKey [FK_Division_Program]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Division]  WITH CHECK ADD  CONSTRAINT [FK_Division_Program] FOREIGN KEY([ProgId])
REFERENCES [dbo].[Program] ([Id])
GO
ALTER TABLE [dbo].[Division] CHECK CONSTRAINT [FK_Division_Program]
GO
/****** Object:  ForeignKey [FK_DivOrg_Division]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[DivOrg]  WITH CHECK ADD  CONSTRAINT [FK_DivOrg_Division] FOREIGN KEY([DivId])
REFERENCES [dbo].[Division] ([Id])
GO
ALTER TABLE [dbo].[DivOrg] CHECK CONSTRAINT [FK_DivOrg_Division]
GO
/****** Object:  ForeignKey [FK_DivOrg_Organizations]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[DivOrg]  WITH CHECK ADD  CONSTRAINT [FK_DivOrg_Organizations] FOREIGN KEY([OrgId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[DivOrg] CHECK CONSTRAINT [FK_DivOrg_Organizations]
GO
/****** Object:  ForeignKey [ENROLLMENT_TRANSACTION_ORG_FK]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[EnrollmentTransaction]  WITH NOCHECK ADD  CONSTRAINT [ENROLLMENT_TRANSACTION_ORG_FK] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[EnrollmentTransaction] CHECK CONSTRAINT [ENROLLMENT_TRANSACTION_ORG_FK]
GO
/****** Object:  ForeignKey [ENROLLMENT_TRANSACTION_PPL_FK]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[EnrollmentTransaction]  WITH NOCHECK ADD  CONSTRAINT [ENROLLMENT_TRANSACTION_PPL_FK] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[EnrollmentTransaction] CHECK CONSTRAINT [ENROLLMENT_TRANSACTION_PPL_FK]
GO
/****** Object:  ForeignKey [FK_ENROLLMENT_TRANSACTION_TBL_MemberType]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[EnrollmentTransaction]  WITH NOCHECK ADD  CONSTRAINT [FK_ENROLLMENT_TRANSACTION_TBL_MemberType] FOREIGN KEY([MemberTypeId])
REFERENCES [lookup].[MemberType] ([Id])
GO
ALTER TABLE [dbo].[EnrollmentTransaction] CHECK CONSTRAINT [FK_ENROLLMENT_TRANSACTION_TBL_MemberType]
GO
/****** Object:  ForeignKey [FK_MEETINGS_TBL_ORGANIZATIONS_TBL]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Meetings]  WITH NOCHECK ADD  CONSTRAINT [FK_MEETINGS_TBL_ORGANIZATIONS_TBL] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Meetings] CHECK CONSTRAINT [FK_MEETINGS_TBL_ORGANIZATIONS_TBL]
GO
/****** Object:  ForeignKey [FK_MemberTags_Program]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[MemberTags]  WITH CHECK ADD  CONSTRAINT [FK_MemberTags_Program] FOREIGN KEY([ProgId])
REFERENCES [dbo].[Program] ([Id])
GO
ALTER TABLE [dbo].[MemberTags] CHECK CONSTRAINT [FK_MemberTags_Program]
GO
/****** Object:  ForeignKey [FK_Contacts_ContactTypes]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[NewContact]  WITH CHECK ADD  CONSTRAINT [FK_Contacts_ContactTypes] FOREIGN KEY([ContactTypeId])
REFERENCES [lookup].[NewContactType] ([Id])
GO
ALTER TABLE [dbo].[NewContact] CHECK CONSTRAINT [FK_Contacts_ContactTypes]
GO
/****** Object:  ForeignKey [FK_Contacts_Ministries]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[NewContact]  WITH CHECK ADD  CONSTRAINT [FK_Contacts_Ministries] FOREIGN KEY([MinistryId])
REFERENCES [dbo].[Ministries] ([MinistryId])
GO
ALTER TABLE [dbo].[NewContact] CHECK CONSTRAINT [FK_Contacts_Ministries]
GO
/****** Object:  ForeignKey [FK_NewContacts_ContactReasons]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[NewContact]  WITH CHECK ADD  CONSTRAINT [FK_NewContacts_ContactReasons] FOREIGN KEY([ContactReasonId])
REFERENCES [lookup].[NewContactReason] ([Id])
GO
ALTER TABLE [dbo].[NewContact] CHECK CONSTRAINT [FK_NewContacts_ContactReasons]
GO
/****** Object:  ForeignKey [FK_ORGANIZATION_MEMBERS_TBL_MemberType]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[OrganizationMembers]  WITH NOCHECK ADD  CONSTRAINT [FK_ORGANIZATION_MEMBERS_TBL_MemberType] FOREIGN KEY([MemberTypeId])
REFERENCES [lookup].[MemberType] ([Id])
GO
ALTER TABLE [dbo].[OrganizationMembers] CHECK CONSTRAINT [FK_ORGANIZATION_MEMBERS_TBL_MemberType]
GO
/****** Object:  ForeignKey [ORGANIZATION_MEMBERS_ORG_FK]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[OrganizationMembers]  WITH NOCHECK ADD  CONSTRAINT [ORGANIZATION_MEMBERS_ORG_FK] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[OrganizationMembers] CHECK CONSTRAINT [ORGANIZATION_MEMBERS_ORG_FK]
GO
/****** Object:  ForeignKey [ORGANIZATION_MEMBERS_PPL_FK]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[OrganizationMembers]  WITH NOCHECK ADD  CONSTRAINT [ORGANIZATION_MEMBERS_PPL_FK] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrganizationMembers] CHECK CONSTRAINT [ORGANIZATION_MEMBERS_PPL_FK]
GO
/****** Object:  ForeignKey [ChildOrgs__ParentOrg]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Organizations]  WITH NOCHECK ADD  CONSTRAINT [ChildOrgs__ParentOrg] FOREIGN KEY([ParentOrgId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Organizations] CHECK CONSTRAINT [ChildOrgs__ParentOrg]
GO
/****** Object:  ForeignKey [FK_ORGANIZATIONS_TBL_AttendTrackLevel]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Organizations]  WITH CHECK ADD  CONSTRAINT [FK_ORGANIZATIONS_TBL_AttendTrackLevel] FOREIGN KEY([AttendTrkLevelId])
REFERENCES [lookup].[AttendTrackLevel] ([Id])
GO
ALTER TABLE [dbo].[Organizations] CHECK CONSTRAINT [FK_ORGANIZATIONS_TBL_AttendTrackLevel]
GO
/****** Object:  ForeignKey [FK_ORGANIZATIONS_TBL_EntryPoint]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Organizations]  WITH CHECK ADD  CONSTRAINT [FK_ORGANIZATIONS_TBL_EntryPoint] FOREIGN KEY([EntryPointId])
REFERENCES [lookup].[EntryPoint] ([Id])
GO
ALTER TABLE [dbo].[Organizations] CHECK CONSTRAINT [FK_ORGANIZATIONS_TBL_EntryPoint]
GO
/****** Object:  ForeignKey [FK_ORGANIZATIONS_TBL_OrganizationStatus]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Organizations]  WITH CHECK ADD  CONSTRAINT [FK_ORGANIZATIONS_TBL_OrganizationStatus] FOREIGN KEY([OrganizationStatusId])
REFERENCES [lookup].[OrganizationStatus] ([Id])
GO
ALTER TABLE [dbo].[Organizations] CHECK CONSTRAINT [FK_ORGANIZATIONS_TBL_OrganizationStatus]
GO
/****** Object:  ForeignKey [FK_ORGANIZATIONS_TBL_WeeklySchedule]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Organizations]  WITH CHECK ADD  CONSTRAINT [FK_ORGANIZATIONS_TBL_WeeklySchedule] FOREIGN KEY([ScheduleId])
REFERENCES [lookup].[WeeklySchedule] ([Id])
GO
ALTER TABLE [dbo].[Organizations] CHECK CONSTRAINT [FK_ORGANIZATIONS_TBL_WeeklySchedule]
GO
/****** Object:  ForeignKey [FK_OrgMemMemTags_MemberTags]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[OrgMemMemTags]  WITH CHECK ADD  CONSTRAINT [FK_OrgMemMemTags_MemberTags] FOREIGN KEY([MemberTagId])
REFERENCES [dbo].[MemberTags] ([Id])
GO
ALTER TABLE [dbo].[OrgMemMemTags] CHECK CONSTRAINT [FK_OrgMemMemTags_MemberTags]
GO
/****** Object:  ForeignKey [FK_OrgMemMemTags_OrganizationMembers]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[OrgMemMemTags]  WITH CHECK ADD  CONSTRAINT [FK_OrgMemMemTags_OrganizationMembers] FOREIGN KEY([OrgId], [PeopleId])
REFERENCES [dbo].[OrganizationMembers] ([OrganizationId], [PeopleId])
GO
ALTER TABLE [dbo].[OrgMemMemTags] CHECK CONSTRAINT [FK_OrgMemMemTags_OrganizationMembers]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_AddressType]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH NOCHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_AddressType] FOREIGN KEY([AddressTypeId])
REFERENCES [lookup].[AddressType] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_AddressType]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_BaptismStatus]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_BaptismStatus] FOREIGN KEY([BaptismStatusId])
REFERENCES [lookup].[BaptismStatus] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_BaptismStatus]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_BaptismType]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_BaptismType] FOREIGN KEY([BaptismTypeId])
REFERENCES [lookup].[BaptismType] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_BaptismType]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_DecisionType]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_DecisionType] FOREIGN KEY([DecisionTypeId])
REFERENCES [lookup].[DecisionType] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_DecisionType]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_DiscoveryClassStatus]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_DiscoveryClassStatus] FOREIGN KEY([DiscoveryClassStatusId])
REFERENCES [lookup].[DiscoveryClassStatus] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_DiscoveryClassStatus]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_DropStatus]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_DropStatus] FOREIGN KEY([DropCodeId])
REFERENCES [lookup].[DropType] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_DropStatus]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_EntryPoint]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_EntryPoint] FOREIGN KEY([EntryPointId])
REFERENCES [lookup].[EntryPoint] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_EntryPoint]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_EnvelopeOption]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_EnvelopeOption] FOREIGN KEY([EnvelopeOptionsId])
REFERENCES [lookup].[EnvelopeOption] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_EnvelopeOption]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_FamilyPosition]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_FamilyPosition] FOREIGN KEY([PositionInFamilyId])
REFERENCES [lookup].[FamilyPosition] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_FamilyPosition]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_Gender]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_Gender] FOREIGN KEY([GenderId])
REFERENCES [lookup].[Gender] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_Gender]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_InterestPoint]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_InterestPoint] FOREIGN KEY([InterestPointId])
REFERENCES [lookup].[InterestPoint] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_InterestPoint]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_JoinType]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_JoinType] FOREIGN KEY([JoinCodeId])
REFERENCES [lookup].[JoinType] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_JoinType]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_MaritalStatus]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH NOCHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_MaritalStatus] FOREIGN KEY([MaritalStatusId])
REFERENCES [lookup].[MaritalStatus] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_MaritalStatus]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_MemberLetterStatus]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_MemberLetterStatus] FOREIGN KEY([LetterStatusId])
REFERENCES [lookup].[MemberLetterStatus] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_MemberLetterStatus]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_MemberStatus]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_MemberStatus] FOREIGN KEY([MemberStatusId])
REFERENCES [lookup].[MemberStatus] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_MemberStatus]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_Origin]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH NOCHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_Origin] FOREIGN KEY([OriginId])
REFERENCES [lookup].[Origin] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_Origin]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_PhonePreference]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_PhonePreference] FOREIGN KEY([PhonePrefId])
REFERENCES [lookup].[PhonePreference] ([Id])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_PhonePreference]
GO
/****** Object:  ForeignKey [FK_PEOPLE_TBL_Picture]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_PEOPLE_TBL_Picture] FOREIGN KEY([PictureId])
REFERENCES [dbo].[Picture] ([PictureId])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_PEOPLE_TBL_Picture]
GO
/****** Object:  ForeignKey [PEOPLE_FAMILY_FK]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[People]  WITH NOCHECK ADD  CONSTRAINT [PEOPLE_FAMILY_FK] FOREIGN KEY([FamilyId])
REFERENCES [dbo].[Families] ([FamilyId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [PEOPLE_FAMILY_FK]
GO
/****** Object:  ForeignKey [FK_UserPreferences_Users]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Preferences]  WITH CHECK ADD  CONSTRAINT [FK_UserPreferences_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Preferences] CHECK CONSTRAINT [FK_UserPreferences_Users]
GO
/****** Object:  ForeignKey [PROMOTION_CONTROL_PROMO_FK]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[PromotionControl]  WITH NOCHECK ADD  CONSTRAINT [PROMOTION_CONTROL_PROMO_FK] FOREIGN KEY([PromotionId])
REFERENCES [dbo].[Promotions] ([PromotionId])
GO
ALTER TABLE [dbo].[PromotionControl] CHECK CONSTRAINT [PROMOTION_CONTROL_PROMO_FK]
GO
/****** Object:  ForeignKey [Clauses__Parent]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[QueryBuilderClauses]  WITH NOCHECK ADD  CONSTRAINT [Clauses__Parent] FOREIGN KEY([GroupId])
REFERENCES [dbo].[QueryBuilderClauses] ([QueryId])
GO
ALTER TABLE [dbo].[QueryBuilderClauses] CHECK CONSTRAINT [Clauses__Parent]
GO
/****** Object:  ForeignKey [RelatedFamilies1__RelatedFamily1]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[RelatedFamilies]  WITH NOCHECK ADD  CONSTRAINT [RelatedFamilies1__RelatedFamily1] FOREIGN KEY([FamilyId])
REFERENCES [dbo].[Families] ([FamilyId])
GO
ALTER TABLE [dbo].[RelatedFamilies] CHECK CONSTRAINT [RelatedFamilies1__RelatedFamily1]
GO
/****** Object:  ForeignKey [RelatedFamilies2__RelatedFamily2]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[RelatedFamilies]  WITH NOCHECK ADD  CONSTRAINT [RelatedFamilies2__RelatedFamily2] FOREIGN KEY([RelatedFamilyId])
REFERENCES [dbo].[Families] ([FamilyId])
GO
ALTER TABLE [dbo].[RelatedFamilies] CHECK CONSTRAINT [RelatedFamilies2__RelatedFamily2]
GO
/****** Object:  ForeignKey [Tags__TagType]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Tag]  WITH NOCHECK ADD  CONSTRAINT [Tags__TagType] FOREIGN KEY([TypeId])
REFERENCES [dbo].[TagType] ([Id])
GO
ALTER TABLE [dbo].[Tag] CHECK CONSTRAINT [Tags__TagType]
GO
/****** Object:  ForeignKey [TagsOwned__PersonOwner]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Tag]  WITH CHECK ADD  CONSTRAINT [TagsOwned__PersonOwner] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Tag] CHECK CONSTRAINT [TagsOwned__PersonOwner]
GO
/****** Object:  ForeignKey [OrgTags__Tag]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[TagOrg]  WITH NOCHECK ADD  CONSTRAINT [OrgTags__Tag] FOREIGN KEY([Id])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[TagOrg] CHECK CONSTRAINT [OrgTags__Tag]
GO
/****** Object:  ForeignKey [Tags__Organization]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[TagOrg]  WITH NOCHECK ADD  CONSTRAINT [Tags__Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[TagOrg] CHECK CONSTRAINT [Tags__Organization]
GO
/****** Object:  ForeignKey [PersonTags__Tag]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[TagPerson]  WITH NOCHECK ADD  CONSTRAINT [PersonTags__Tag] FOREIGN KEY([Id])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[TagPerson] CHECK CONSTRAINT [PersonTags__Tag]
GO
/****** Object:  ForeignKey [Tags__Person]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[TagPerson]  WITH NOCHECK ADD  CONSTRAINT [Tags__Person] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[TagPerson] CHECK CONSTRAINT [Tags__Person]
GO
/****** Object:  ForeignKey [FK_TagShare_People]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[TagShare]  WITH CHECK ADD  CONSTRAINT [FK_TagShare_People] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[TagShare] CHECK CONSTRAINT [FK_TagShare_People]
GO
/****** Object:  ForeignKey [FK_TagShare_Tag]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[TagShare]  WITH CHECK ADD  CONSTRAINT [FK_TagShare_Tag] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[TagShare] CHECK CONSTRAINT [FK_TagShare_Tag]
GO
/****** Object:  ForeignKey [Tags__ParentTag]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[TagTag]  WITH NOCHECK ADD  CONSTRAINT [Tags__ParentTag] FOREIGN KEY([ParentTagId])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[TagTag] CHECK CONSTRAINT [Tags__ParentTag]
GO
/****** Object:  ForeignKey [TagTags__Tag]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[TagTag]  WITH NOCHECK ADD  CONSTRAINT [TagTags__Tag] FOREIGN KEY([Id])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[TagTag] CHECK CONSTRAINT [TagTags__Tag]
GO
/****** Object:  ForeignKey [CoTasks__CoTaskList]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [CoTasks__CoTaskList] FOREIGN KEY([CoListId])
REFERENCES [dbo].[TaskList] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [CoTasks__CoTaskList]
GO
/****** Object:  ForeignKey [FK_Task_TaskStatus]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_TaskStatus] FOREIGN KEY([StatusId])
REFERENCES [lookup].[TaskStatus] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_TaskStatus]
GO
/****** Object:  ForeignKey [Tasks__Owner]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [Tasks__Owner] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [Tasks__Owner]
GO
/****** Object:  ForeignKey [Tasks__TaskList]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [Tasks__TaskList] FOREIGN KEY([ListId])
REFERENCES [dbo].[TaskList] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [Tasks__TaskList]
GO
/****** Object:  ForeignKey [TasksAboutPerson__AboutWho]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [TasksAboutPerson__AboutWho] FOREIGN KEY([WhoId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [TasksAboutPerson__AboutWho]
GO
/****** Object:  ForeignKey [TasksAssigned__SourceContact]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [TasksAssigned__SourceContact] FOREIGN KEY([SourceContactId])
REFERENCES [dbo].[NewContact] ([ContactId])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [TasksAssigned__SourceContact]
GO
/****** Object:  ForeignKey [TasksCompleted__CompletedContact]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [TasksCompleted__CompletedContact] FOREIGN KEY([CompletedContactId])
REFERENCES [dbo].[NewContact] ([ContactId])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [TasksCompleted__CompletedContact]
GO
/****** Object:  ForeignKey [TasksCoOwned__CoOwner]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [TasksCoOwned__CoOwner] FOREIGN KEY([CoOwnerId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [TasksCoOwned__CoOwner]
GO
/****** Object:  ForeignKey [FK_TaskListOwners_PEOPLE_TBL]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[TaskListOwners]  WITH CHECK ADD  CONSTRAINT [FK_TaskListOwners_PEOPLE_TBL] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[TaskListOwners] CHECK CONSTRAINT [FK_TaskListOwners_PEOPLE_TBL]
GO
/****** Object:  ForeignKey [FK_TaskListOwners_TaskList]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[TaskListOwners]  WITH CHECK ADD  CONSTRAINT [FK_TaskListOwners_TaskList] FOREIGN KEY([TaskListId])
REFERENCES [dbo].[TaskList] ([Id])
GO
ALTER TABLE [dbo].[TaskListOwners] CHECK CONSTRAINT [FK_TaskListOwners_TaskList]
GO
/****** Object:  ForeignKey [UsersICanEmailFor__Assistant]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[UserCanEmailFor]  WITH CHECK ADD  CONSTRAINT [UsersICanEmailFor__Assistant] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserCanEmailFor] CHECK CONSTRAINT [UsersICanEmailFor__Assistant]
GO
/****** Object:  ForeignKey [UsersWhoCanEmailForMe__Boss]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[UserCanEmailFor]  WITH CHECK ADD  CONSTRAINT [UsersWhoCanEmailForMe__Boss] FOREIGN KEY([CanEmailFor])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserCanEmailFor] CHECK CONSTRAINT [UsersWhoCanEmailForMe__Boss]
GO
/****** Object:  ForeignKey [FK_UserRole_Roles]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_Roles]
GO
/****** Object:  ForeignKey [FK_UserRole_Users]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_Users]
GO
/****** Object:  ForeignKey [FK_Users_PEOPLE_TBL]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_PEOPLE_TBL] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_PEOPLE_TBL]
GO
/****** Object:  ForeignKey [FK_VBSApp_Organizations]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[VBSApp]  WITH CHECK ADD  CONSTRAINT [FK_VBSApp_Organizations] FOREIGN KEY([OrgId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[VBSApp] CHECK CONSTRAINT [FK_VBSApp_Organizations]
GO
/****** Object:  ForeignKey [FK_VBSApp_People]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[VBSApp]  WITH CHECK ADD  CONSTRAINT [FK_VBSApp_People] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[VBSApp] CHECK CONSTRAINT [FK_VBSApp_People]
GO
/****** Object:  ForeignKey [FK_Volunteer_PEOPLE_TBL]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Volunteer]  WITH CHECK ADD  CONSTRAINT [FK_Volunteer_PEOPLE_TBL] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Volunteer] CHECK CONSTRAINT [FK_Volunteer_PEOPLE_TBL]
GO
/****** Object:  ForeignKey [FK_Volunteer_VolApplicationStatus]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[Volunteer]  WITH CHECK ADD  CONSTRAINT [FK_Volunteer_VolApplicationStatus] FOREIGN KEY([StatusId])
REFERENCES [lookup].[VolApplicationStatus] ([Id])
GO
ALTER TABLE [dbo].[Volunteer] CHECK CONSTRAINT [FK_Volunteer_VolApplicationStatus]
GO
/****** Object:  ForeignKey [FK_VolunteerForm_PEOPLE_TBL]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[VolunteerForm]  WITH CHECK ADD  CONSTRAINT [FK_VolunteerForm_PEOPLE_TBL] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[VolunteerForm] CHECK CONSTRAINT [FK_VolunteerForm_PEOPLE_TBL]
GO
/****** Object:  ForeignKey [FK_VolunteerForm_Volunteer1]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[VolunteerForm]  WITH CHECK ADD  CONSTRAINT [FK_VolunteerForm_Volunteer1] FOREIGN KEY([PeopleId])
REFERENCES [dbo].[Volunteer] ([PeopleId])
GO
ALTER TABLE [dbo].[VolunteerForm] CHECK CONSTRAINT [FK_VolunteerForm_Volunteer1]
GO
/****** Object:  ForeignKey [VolunteerFormsUploaded__Uploader]    Script Date: 05/06/2009 19:26:36 ******/
ALTER TABLE [dbo].[VolunteerForm]  WITH CHECK ADD  CONSTRAINT [VolunteerFormsUploaded__Uploader] FOREIGN KEY([UploaderId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[VolunteerForm] CHECK CONSTRAINT [VolunteerFormsUploaded__Uploader]
GO
