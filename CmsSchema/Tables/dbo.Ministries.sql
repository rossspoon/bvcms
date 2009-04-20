CREATE TABLE [dbo].[Ministries]
(
[MinistryId] [int] NOT NULL IDENTITY(1, 1),
[MinistryName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedBy] [int] NULL,
[CreatedDate] [datetime] NULL,
[ModifiedBy] [int] NULL,
[ModifiedDate] [datetime] NULL,
[RecordStatus] [bit] NULL,
[DepartmentId] [int] NULL,
[MinistryDescription] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MinistryContactId] [int] NULL,
[ChurchId] [int] NULL
)
ALTER TABLE [dbo].[Ministries] ADD 
CONSTRAINT [PK_MINISTRIES_TBL] PRIMARY KEY CLUSTERED ([MinistryId])
CREATE NONCLUSTERED INDEX [MINISTRIES_PK_IX] ON [dbo].[Ministries] ([MinistryId])

GO
