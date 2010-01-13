CREATE TABLE [dbo].[DivOrg]
(
[DivId] [int] NOT NULL,
[OrgId] [int] NOT NULL
)


GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[updDivOrg] 
   ON  [dbo].[DivOrg] 
   AFTER INSERT, DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.People
	SET BibleFellowshipClassId = dbo.BibleFellowshipClassId(p.PeopleId)
	FROM dbo.People p
	JOIN dbo.OrganizationMembers m ON p.PeopleId = m.PeopleId
	JOIN dbo.Organizations o ON m.OrganizationId = o.OrganizationId
	JOIN INSERTED x ON o.OrganizationId = x.OrgId
	JOIN dbo.Division d ON d.Id = x.DivId
	JOIN Program pr ON d.ProgId = pr.Id
	WHERE pr.BFProgram = 1

	UPDATE dbo.People
	SET BibleFellowshipClassId = dbo.BibleFellowshipClassId(p.PeopleId)
	FROM dbo.People p
	JOIN dbo.OrganizationMembers m ON p.PeopleId = m.PeopleId
	JOIN dbo.Organizations o ON m.OrganizationId = o.OrganizationId
	JOIN DELETED x ON o.OrganizationId = x.OrgId
	JOIN dbo.Division d ON d.Id = x.DivId
	JOIN Program pr ON d.ProgId = pr.Id
	WHERE pr.BFProgram = 1

END
GO



ALTER TABLE [dbo].[DivOrg] ADD CONSTRAINT [PK_DivOrg] PRIMARY KEY CLUSTERED  ([DivId], [OrgId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DivOrg] ADD CONSTRAINT [FK_DivOrg_Division] FOREIGN KEY ([DivId]) REFERENCES [dbo].[Division] ([Id])
GO
ALTER TABLE [dbo].[DivOrg] ADD CONSTRAINT [FK_DivOrg_Organizations] FOREIGN KEY ([OrgId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
