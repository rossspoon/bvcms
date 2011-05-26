CREATE TABLE [lookup].[StateLookup]
(
[StateCode] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[StateLookup] ADD CONSTRAINT [PK_STATE_LOOKUP_TBL] PRIMARY KEY CLUSTERED  ([StateCode]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [STATE_LOOKUP_CODE_IX] ON [lookup].[StateLookup] ([StateCode]) ON [PRIMARY]
GO
