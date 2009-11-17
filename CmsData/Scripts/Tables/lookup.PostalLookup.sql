CREATE TABLE [lookup].[PostalLookup]
(
[PostalCode] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CityName] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateCode] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CountryName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResCodeId] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[PostalLookup] ADD CONSTRAINT [PK_POSTAL_LOOKUP_TBL] PRIMARY KEY CLUSTERED  ([PostalCode]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [POSTAL_LOOKUP_CODE_IX] ON [lookup].[PostalLookup] ([PostalCode]) ON [PRIMARY]
GO
