SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[NextSecurityCode] (@dt DATETIME)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ct INT
	SELECT @ct = COUNT(*) FROM SecurityCodes
	IF (@ct = 0)
	BEGIN
		DECLARE @n int = 45000
		DECLARE @v VARCHAR(3)
		WHILE @n > 0
		BEGIN
			SELECT @v = dbo.DecToBase(RAND() * 45000, 36)
			IF @v LIKE '%[0-9]%' AND LEN(@v) = 3
				INSERT SecurityCodes (Code, DateUsed) VALUES(@v, '1/1/80')
			SET @n = @n -1
		END
	END

	DECLARE @id INT
	DECLARE @midnight DATETIME = DATEADD(dd, DATEDIFF(dd,0,getdate()), 0)
	BEGIN TRANSACTION
		SELECT TOP 1 @id = id FROM SecurityCodes WHERE DateUsed < @midnight
		UPDATE SecurityCodes SET DateUsed = @dt WHERE id = @id
	COMMIT TRANSACTION
	
	SELECT TOP 1 * FROM dbo.SecurityCodes WHERE id = @id
END
GO
EXEC sp_addextendedproperty N'ReturnType', N'SecurityCode', 'SCHEMA', N'dbo', 'PROCEDURE', N'NextSecurityCode', NULL, NULL
GO
