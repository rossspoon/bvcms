CREATE TABLE [dbo].[Division]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ProgId] [int] NULL,
[SortOrder] [int] NULL
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
CREATE TRIGGER [dbo].[updDivision] 
   ON  [dbo].[Division] 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	IF UPDATE(ProgId)
	BEGIN
		UPDATE dbo.People
		SET BibleFellowshipTeacherId = dbo.BibleFellowshipTeacherId(p.PeopleId),
		BibleFellowshipClassId = dbo.BibleFellowshipClassId(p.PeopleId),
		BibleFellowshipTeacher = dbo.BibleFellowshipTeacher(p.PeopleId),
		InBFClass = dbo.InBFClass(p.PeopleId)
		FROM dbo.People p
		JOIN dbo.OrganizationMembers m ON p.PeopleId = m.PeopleId
		JOIN dbo.Organizations o ON m.OrganizationId = o.OrganizationId
		JOIN dbo.DivOrg x ON o.OrganizationId = x.OrgId
		JOIN INSERTED i ON i.Id = x.DivId
		JOIN DELETED d ON d.Id = x.DivId
		JOIN Program pr ON i.ProgId = pr.Id OR d.ProgId = pr.Id
		WHERE pr.BFProgram = 1	
	END
END

GO


ALTER TABLE [dbo].[Division] ADD CONSTRAINT [PK_Division] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Division] ADD CONSTRAINT [FK_Division_Program] FOREIGN KEY ([ProgId]) REFERENCES [dbo].[Program] ([Id])
GO
