CREATE TABLE [dbo].[VBSApp]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[PeopleId] [int] NULL,
[ImgId] [int] NULL,
[IsDocument] [bit] NULL,
[Uploaded] [datetime] NULL,
[Request] [varchar] (140) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ActiveInAnotherChurch] [bit] NULL,
[GradeCompleted] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OrgId] AS ([dbo].[VBSOrg]([PeopleId])),
[Inactive] [bit] NULL,
[PubPhoto] [bit] NULL,
[UserInfo] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MedAllergy] [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VBSApp] ADD CONSTRAINT [PK_VBSApp] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
