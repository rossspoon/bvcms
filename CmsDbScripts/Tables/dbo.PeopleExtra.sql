CREATE TABLE [dbo].[PeopleExtra]
(
[PeopleId] [int] NOT NULL,
[Field] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StrValue] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DateValue] [datetime] NULL,
[TransactionTime] [datetime] NOT NULL CONSTRAINT [DF_PeopleExtra_TransactionTime] DEFAULT (((1)/(1))/(1900)),
[Data] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IntValue] [int] NULL,
[IntValue2] [int] NULL,
[FieldValue] AS (([Field]+':')+[StrValue])
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[PeopleExtra] ADD CONSTRAINT [PK_PeopleExtra] PRIMARY KEY CLUSTERED  ([PeopleId], [TransactionTime], [Field]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PeopleExtra_2] ON [dbo].[PeopleExtra] ([Field], [DateValue]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PeopleExtra] ON [dbo].[PeopleExtra] ([Field], [StrValue]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PeopleExtra] ADD CONSTRAINT [FK_PeopleExtra_People] FOREIGN KEY ([PeopleId]) REFERENCES [dbo].[People] ([PeopleId])
GO
