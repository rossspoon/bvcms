CREATE TABLE [dbo].[RecReg]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[PeopleId] [int] NULL,
[ImgId] [int] NULL,
[IsDocument] [bit] NULL,
[ActiveInAnotherChurch] [bit] NULL,
[ShirtSize] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MedAllergy] [bit] NULL,
[email] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MedicalDescription] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[fname] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[mname] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[coaching] [bit] NULL,
[member] [bit] NULL,
[emcontact] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[emphone] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[doctor] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[docphone] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[insurance] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[policy] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Comments] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)

ALTER TABLE [dbo].[RecReg] ADD
CONSTRAINT [FK_RecReg_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[RecReg] ADD CONSTRAINT [PK_Participant] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
