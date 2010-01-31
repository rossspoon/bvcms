CREATE TABLE [disc].[VerseCategoryXref]
(
[VerseCategoryId] [int] NOT NULL,
[VerseId] [int] NOT NULL
)
GO
ALTER TABLE [disc].[VerseCategoryXref] ADD CONSTRAINT [PK_VerseCategoryXref_1] PRIMARY KEY CLUSTERED  ([VerseCategoryId], [VerseId])
GO
CREATE NONCLUSTERED INDEX [iVerseCategories_btea_VerseCategoriesVerses_5CB9F1B2] ON [disc].[VerseCategoryXref] ([VerseCategoryId])
GO
CREATE UNIQUE NONCLUSTERED INDEX [iVerseCategoriesVerses_btea_VerseCategoriesVerses_5FC63DDC] ON [disc].[VerseCategoryXref] ([VerseCategoryId], [VerseId])
GO
CREATE NONCLUSTERED INDEX [iVerses_btea_VerseCategoriesVerses_1161F2C4] ON [disc].[VerseCategoryXref] ([VerseId])
GO
ALTER TABLE [disc].[VerseCategoryXref] ADD CONSTRAINT [FK_VerseCategoryXref_Verse] FOREIGN KEY ([VerseId]) REFERENCES [disc].[Verse] ([id])
GO
ALTER TABLE [disc].[VerseCategoryXref] ADD CONSTRAINT [FK_VerseCategoryXref_VerseCategory] FOREIGN KEY ([VerseCategoryId]) REFERENCES [disc].[VerseCategory] ([id])
GO