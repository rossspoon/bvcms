CREATE TABLE [dbo].[OrgMemMemTags]
(
[OrgId] [int] NOT NULL,
[PeopleId] [int] NOT NULL,
[MemberTagId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrgMemMemTags] ADD CONSTRAINT [PK_OrgMemMemTags] PRIMARY KEY CLUSTERED  ([OrgId], [PeopleId], [MemberTagId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrgMemMemTags] ADD CONSTRAINT [FK_OrgMemMemTags_MemberTags] FOREIGN KEY ([MemberTagId]) REFERENCES [dbo].[MemberTags] ([Id])
GO
ALTER TABLE [dbo].[OrgMemMemTags] ADD CONSTRAINT [FK_OrgMemMemTags_OrganizationMembers] FOREIGN KEY ([OrgId], [PeopleId]) REFERENCES [dbo].[OrganizationMembers] ([OrganizationId], [PeopleId])
GO
