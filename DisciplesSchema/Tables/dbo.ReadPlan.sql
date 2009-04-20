CREATE TABLE [dbo].[ReadPlan]
(
[Day] [int] NOT NULL,
[Section] [int] NOT NULL,
[StartBook] [int] NULL,
[StartChap] [int] NULL,
[StartVerse] [int] NULL,
[EndBook] [int] NULL,
[EndChap] [int] NULL,
[EndVerse] [int] NULL
)

GO
ALTER TABLE [dbo].[ReadPlan] ADD CONSTRAINT [PK_ReadPlan_1] PRIMARY KEY CLUSTERED ([Day], [Section])
GO
