CREATE TABLE [dbo].[Attend]
(
[PeopleId] [int] NOT NULL,
[MeetingId] [int] NOT NULL,
[OrganizationId] [int] NOT NULL,
[MeetingDate] [datetime] NOT NULL,
[AttendanceFlag] [bit] NOT NULL CONSTRAINT [DF_Attend_AttendanceFlag] DEFAULT ((0)),
[OtherOrgId] [int] NULL,
[AttendanceTypeId] [int] NULL,
[CreatedBy] [int] NULL,
[CreatedDate] [datetime] NULL,
[MemberTypeId] [int] NOT NULL,
[OtherAttends] [int] NOT NULL CONSTRAINT [DF_Attend_OtherAttends] DEFAULT ((0)),
[AttendId] [int] NOT NULL IDENTITY(1, 1),
[BFCAttendance] [bit] NULL,
[EffAttendFlag] AS (CONVERT([bit],case when [AttendanceTypeId]=(90) then NULL when [AttendanceFlag]=(1) AND [OtherAttends]>(0) AND [BFCAttendance]=(1) then NULL when [AttendanceFlag]=(1) then (1) when [OtherAttends]>(0) then NULL else (0) end,(0)))
)
CREATE NONCLUSTERED INDEX [IX_Attend_4] ON [dbo].[Attend] ([MeetingId])

GO


ALTER TABLE [dbo].[Attend] ADD
CONSTRAINT [FK_AttendWithAbsents_TBL_ORGANIZATIONS_TBL] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
ALTER TABLE [dbo].[Attend] ADD
CONSTRAINT [FK_AttendWithAbsents_TBL_PEOPLE_TBL] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
CREATE UNIQUE NONCLUSTERED INDEX [IX_Attend_2] ON [dbo].[Attend] ([MeetingId], [PeopleId])

ALTER TABLE [dbo].[Attend] ADD
CONSTRAINT [FK_AttendWithAbsents_TBL_MEETINGS_TBL] FOREIGN KEY ([MeetingId]) REFERENCES [dbo].[Meetings] ([MeetingId])
ALTER TABLE [dbo].[Attend] ADD
CONSTRAINT [FK_Attend_MemberType] FOREIGN KEY ([MemberTypeId]) REFERENCES [lookup].[MemberType] ([Id])
ALTER TABLE [dbo].[Attend] ADD
CONSTRAINT [FK_AttendWithAbsents_TBL_AttendType] FOREIGN KEY ([AttendanceTypeId]) REFERENCES [lookup].[AttendType] ([Id])

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO








ALTER TABLE [dbo].[Attend] ADD 
CONSTRAINT [PK_Attend] PRIMARY KEY CLUSTERED ([AttendId])
CREATE NONCLUSTERED INDEX [IX_Attend_3] ON [dbo].[Attend] ([PeopleId])




CREATE NONCLUSTERED INDEX [IX_Attend_1] ON [dbo].[Attend] ([MeetingDate])


CREATE NONCLUSTERED INDEX [IX_Attend] ON [dbo].[Attend] ([OrganizationId])


GO
