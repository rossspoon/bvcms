CREATE TABLE [dbo].[PersonalPage]
(
[Id] [int] NOT NULL,
[UserId] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Picture] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Birthday] [datetime] NULL,
[Spouse] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Mobile] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Work] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Home] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CSZ] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Activities] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Interests] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Music] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TVShows] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Employer] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[WorkPosition] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[WorkLocation] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[WorkDescription] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsPublic] [bit] NULL
)

GO
ALTER TABLE [dbo].[PersonalPage] ADD CONSTRAINT [PK_PersonalPage] PRIMARY KEY CLUSTERED ([Id])
GO
CREATE NONCLUSTERED INDEX [IX_PersonalPage] ON [dbo].[PersonalPage] ([UserId])
GO
