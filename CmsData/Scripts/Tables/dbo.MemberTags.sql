CREATE TABLE [dbo].[MemberTags]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OrgId] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MemberTags] ADD CONSTRAINT [PK_MemberTags] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MemberTags] ADD CONSTRAINT [FK_MemberTags_Organizations] FOREIGN KEY ([OrgId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
