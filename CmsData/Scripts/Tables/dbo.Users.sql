CREATE TABLE [dbo].[Users]
(
[UserId] [int] NOT NULL IDENTITY(1, 1),
[PeopleId] [int] NULL,
[Username] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Comment] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Password] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PasswordQuestion] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PasswordAnswer] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsApproved] [bit] NOT NULL CONSTRAINT [DF_Users_IsApproved] DEFAULT ((0)),
[LastActivityDate] [datetime] NULL,
[LastLoginDate] [datetime] NULL,
[LastPasswordChangedDate] [datetime] NULL,
[CreationDate] [datetime] NULL,
[IsLockedOut] [bit] NOT NULL CONSTRAINT [DF_Users_IsLockedOut] DEFAULT ((0)),
[LastLockedOutDate] [datetime] NULL,
[FailedPasswordAttemptCount] [int] NOT NULL CONSTRAINT [DF_Users_FailedPasswordAttemptCount] DEFAULT ((0)),
[FailedPasswordAttemptWindowStart] [datetime] NULL,
[FailedPasswordAnswerAttemptCount] [int] NOT NULL CONSTRAINT [DF_Users_FailedPasswordAnswerAttemptCount] DEFAULT ((0)),
[FailedPasswordAnswerAttemptWindowStart] [datetime] NULL,
[EmailAddress] AS ([dbo].[UEmail]([PeopleId])),
[ItemsInGrid] [int] NULL,
[CurrentCart] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MustChangePassword] [bit] NOT NULL CONSTRAINT [DF_Users_MustChangePassword] DEFAULT ((0)),
[Host] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TempPassword] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Name2] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResetPasswordCode] [uniqueidentifier] NULL,
[DefaultGroup] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE TRIGGER [dbo].[updUser] 
   ON  [dbo].[Users]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF UPDATE(PeopleId)
	BEGIN
		UPDATE Users
		SET Name = dbo.UName(PeopleId),
		Name2 = dbo.UName2(PeopleId)
		WHERE PeopleId IN (SELECT PeopleId FROM INSERTED)
	END
END
GO



ALTER TABLE [dbo].[Users] ADD
CONSTRAINT [FK_Users_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [PK_Users_1] PRIMARY KEY CLUSTERED  ([UserId]) ON [PRIMARY]
GO
