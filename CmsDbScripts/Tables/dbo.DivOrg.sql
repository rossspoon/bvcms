CREATE TABLE [dbo].[DivOrg]
(
[DivId] [int] NOT NULL,
[OrgId] [int] NOT NULL
) ON [PRIMARY]
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
CREATE TRIGGER [dbo].[delDivOrg]
   ON  [dbo].[DivOrg]
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Organizations
	SET DivisionId = NULL
	FROM dbo.Organizations o
	JOIN DELETED i ON i.OrgId = o.OrganizationId
	WHERE o.DivisionId = i.DivId
END
GO
ALTER TABLE [dbo].[DivOrg] ADD CONSTRAINT [PK_DivOrg] PRIMARY KEY CLUSTERED  ([DivId], [OrgId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DivOrg] ADD CONSTRAINT [FK_DivOrg_Division] FOREIGN KEY ([DivId]) REFERENCES [dbo].[Division] ([Id])
GO
ALTER TABLE [dbo].[DivOrg] ADD CONSTRAINT [FK_DivOrg_Organizations] FOREIGN KEY ([OrgId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
