CREATE TABLE [dbo].[Program]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BFProgram] [bit] NULL
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
CREATE TRIGGER [dbo].[updProgram] 
   ON  [dbo].[Program] 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	IF UPDATE(BFProgram)
	BEGIN
		UPDATE dbo.People
		SET BibleFellowshipClassId = dbo.BibleFellowshipClassId(p.PeopleId)
		FROM dbo.People p
		JOIN dbo.OrganizationMembers m ON p.PeopleId = m.PeopleId
		JOIN dbo.Organizations o ON m.OrganizationId = o.OrganizationId
		JOIN dbo.DivOrg x ON o.OrganizationId = x.OrgId
		JOIN dbo.Division d ON x.DivId = d.Id
		JOIN INSERTED pr ON pr.Id = d.ProgId
		WHERE pr.BFProgram = 1
	END
END

GO



ALTER TABLE [dbo].[Program] ADD CONSTRAINT [PK_Program] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
