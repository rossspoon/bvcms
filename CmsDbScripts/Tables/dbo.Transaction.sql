CREATE TABLE [dbo].[Transaction]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[TransactionDate] [datetime] NULL,
[TransactionGateway] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DatumId] [int] NULL,
[testing] [bit] NULL,
[amt] [money] NULL,
[ApprovalCode] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Approved] [bit] NULL,
[TransactionId] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Message] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AuthCode] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[amtdue] [money] NULL,
[URL] [varchar] (180) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (180) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[City] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[State] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Zip] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Phone] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Emails] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Participants] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OrgId] [int] NULL,
[OriginalId] [int] NULL,
[regfees] [money] NULL,
[donate] [money] NULL,
[fund] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Transaction] ADD CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
