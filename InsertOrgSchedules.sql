CREATE TABLE [lookup].[AttendCredit](
	[Id] [int] NOT NULL,
	[Code] [varchar](10) NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_AttendCredit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO        

INSERT INTO [lookup].[AttendCredit] ([Id], [Code], [Description]) VALUES (1, 'E', 'Every Meeting')
INSERT INTO [lookup].[AttendCredit] ([Id], [Code], [Description]) VALUES (2, 'D', 'Once Per Day')
INSERT INTO [lookup].[AttendCredit] ([Id], [Code], [Description]) VALUES (3, 'W', 'Once Per Week')

GO

CREATE TABLE [dbo].[OrgSchedule](
	[OrganizationId] [int] NOT NULL,
	[Id] [int] NOT NULL,
	[ScheduleId] [int] NULL,
	[SchedTime] [datetime] NULL,
	[SchedDay] [int] NULL,
	[MeetingTime] [datetime] NULL,
	[AttendCredit] [int] NULL,
 CONSTRAINT [PK_OrgSchedule] PRIMARY KEY CLUSTERED 
(
	[OrganizationId] ASC,
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO dbo.OrgSchedule
        ( OrganizationId ,
          Id ,
          SchedTime ,
          SchedDay ,
          AttendCredit
        )
        SELECT 
        OrganizationId,
        1,
        SchedTime,
        SchedDay,
        1
        FROM dbo.Organizations o
        WHERE NOT EXISTS(SELECT NULL FROM dbo.OrgSchedule WHERE OrganizationId = o.OrganizationId)
        
GO

