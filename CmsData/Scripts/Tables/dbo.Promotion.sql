CREATE TABLE [dbo].[Promotion]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[FromDivId] [int] NULL,
[ToDivId] [int] NULL,
[Description] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Sort] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Promotion] ADD CONSTRAINT [PK_Promotion] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Promotion] ADD CONSTRAINT [FromPromotions__FromDivision] FOREIGN KEY ([FromDivId]) REFERENCES [dbo].[Division] ([Id])
GO
ALTER TABLE [dbo].[Promotion] ADD CONSTRAINT [ToPromotions__ToDivision] FOREIGN KEY ([ToDivId]) REFERENCES [dbo].[Division] ([Id])
GO
