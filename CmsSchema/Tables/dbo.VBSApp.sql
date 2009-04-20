CREATE TABLE [dbo].[VBSApp]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[PeopleId] [int] NULL,
[ImgId] [int] NULL,
[IsDocument] [bit] NULL,
[Uploaded] [datetime] NULL,
[Request] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ActiveInAnotherChurch] [bit] NULL,
[GradeCompleted] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OrgId] [int] NULL,
[DivId] [int] NULL,
[Inactive] [bit] NULL,
[PubPhoto] [bit] NULL,
[UserInfo] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
ALTER TABLE [dbo].[VBSApp] ADD
CONSTRAINT [FK_VBSApp_Organizations] FOREIGN KEY ([OrgId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[VBSApp] ADD CONSTRAINT [PK_VBSApp] PRIMARY KEY CLUSTERED ([Id])
GO
ALTER TABLE [dbo].[VBSApp] ADD CONSTRAINT [FK_VBSApp_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
