CREATE TABLE [dbo].[Picture]
(
[PictureId] [int] NOT NULL IDENTITY(1, 1),
[CreatedDate] [datetime] NULL,
[CreatedBy] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LargeId] [int] NULL,
[MediumId] [int] NULL,
[SmallId] [int] NULL
)


ALTER TABLE [dbo].[Picture] ADD 
CONSTRAINT [PK_Picture] PRIMARY KEY CLUSTERED  ([PictureId])


GO
