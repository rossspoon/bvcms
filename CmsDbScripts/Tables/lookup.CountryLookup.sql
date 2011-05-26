CREATE TABLE [lookup].[CountryLookup]
(
[CountryName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DisplayFlag] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[CountryLookup] ADD CONSTRAINT [PK_COUNTRY_LOOKUP_TBL_1] PRIMARY KEY CLUSTERED  ([CountryCode]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [COUNTRY_LOOKUP_NAME_IX] ON [lookup].[CountryLookup] ([CountryName]) ON [PRIMARY]
GO
