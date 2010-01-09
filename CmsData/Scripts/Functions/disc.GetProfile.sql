SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION disc.GetProfile
(
    @Username VARCHAR(50),
    @PropertyName varchar(100)
)
RETURNS VARCHAR(100)
AS
BEGIN

    DECLARE @StartPosition INT
    DECLARE @PropertyLength INT
    DECLARE @Return VARCHAR(100)

    DECLARE @RowData nvarchar(2000)
    DECLARE @SplitOn nvarchar(5)

    DECLARE @PropertyTable table 
    (
        Id int identity(1,1),
        Property nvarchar(100),
        DataType CHAR(1),
        Start INT,
        Length INT
    ) 

    SELECT @RowData = PropertyNames
    FROM aspnet_Profile
    JOIN dbo.aspnet_Users ON dbo.aspnet_Profile.UserId = dbo.aspnet_Users.UserId
    WHERE Username = @Username
    AND PropertyNames LIKE '%'+@PropertyName+'%'

    SET @SplitOn = ':'

    While (Charindex(@SplitOn,@RowData)>0)
    Begin
        Insert Into @PropertyTable (Property)
        Select ltrim(rtrim(Substring(@RowData,1,Charindex(@SplitOn,@RowData)-1)))
        
        Set @RowData = Substring(@RowData,Charindex(@SplitOn,@RowData)+1,len(@RowData))
        
        UPDATE @PropertyTable SET DataType = (
            Select ltrim(rtrim(Substring(@RowData,1,Charindex(@SplitOn,@RowData)-1))))
            WHERE Id = @@IDENTITY
        
        Set @RowData = Substring(@RowData,Charindex(@SplitOn,@RowData)+1,len(@RowData))
        
        UPDATE @PropertyTable SET Start = (
            Select ltrim(rtrim(Substring(@RowData,1,Charindex(@SplitOn,@RowData)-1))))
            WHERE Id = @@IDENTITY
        
        Set @RowData = Substring(@RowData,Charindex(@SplitOn,@RowData)+1,len(@RowData))
        
        UPDATE @PropertyTable SET Length = (
            Select ltrim(rtrim(Substring(@RowData,1,Charindex(@SplitOn,@RowData)-1))))
            WHERE Id = @@IDENTITY
        
        Set @RowData = Substring(@RowData,Charindex(@SplitOn,@RowData)+1,len(@RowData))
    END

    SELECT @StartPosition = Start, @PropertyLength = Length FROM @PropertyTable WHERE Property = @PropertyName

    SELECT @Return = SUBSTRING(PropertyValuesString, @StartPosition + 1, @PropertyLength)
    FROM aspnet_Profile
    JOIN dbo.aspnet_Users ON dbo.aspnet_Profile.UserId = dbo.aspnet_Users.UserId
    WHERE Username = @Username
    AND PropertyNames LIKE '%'+@PropertyName+'%'

    RETURN @Return
END
GO
