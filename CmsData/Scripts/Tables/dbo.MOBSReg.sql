CREATE TABLE [dbo].[MOBSReg]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[PeopleId] [int] NULL,
[Created] [datetime] NULL,
[NumTickets] [int] NOT NULL,
[FeePaid] [bit] NULL,
[TransactionId] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MeetingId] [int] NULL,
[email] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[phone] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[homecell] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MOBSReg] ADD CONSTRAINT [PK_MOBS_Id] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MOBSReg] ADD CONSTRAINT [FK_Attender_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[MOBSReg] ADD CONSTRAINT [FK_MOBSReg_Meeting] FOREIGN KEY ([MeetingId]) REFERENCES [dbo].[Meetings] ([MeetingId])
GO
