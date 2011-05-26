CREATE TABLE [dbo].[ChangeLog]
(
[PeopleId] [int] NOT NULL,
[FamilyId] [int] NULL,
[UserPeopleId] [int] NOT NULL,
[Created] [datetime] NOT NULL,
[Field] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Data] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Id] [int] NOT NULL IDENTITY(1, 1)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ChangeLog] ADD CONSTRAINT [PK_ChangeLog_1] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
