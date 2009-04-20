
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[InBFClass2]
	(
	@pid int
	)
RETURNS bit
AS
	BEGIN
	declare @mem bit

select @mem = 1
FROM dbo.OrganizationMembers  AS om
join dbo.Organizations o on o.OrganizationId = om.OrganizationId
join dbo.TagOrg AS tgo on tgo.organizationid = o.OrganizationId
join dbo.Tag t on tgo.id = t.id
join dbo.TagTag tt on tt.id = t.id
where tt.parenttagid = 101 
and o.RecordStatus = 0 
and om.RecordStatus = 0
and om.PeopleId = @pid

RETURN isnull(@mem, 0)
	END

GO
