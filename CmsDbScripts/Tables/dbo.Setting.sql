CREATE TABLE [dbo].[Setting]
(
[Id] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Setting] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Setting] ADD CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
