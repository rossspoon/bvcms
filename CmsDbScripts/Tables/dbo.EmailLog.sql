CREATE TABLE [dbo].[EmailLog]
(
[id] [int] NOT NULL IDENTITY(1, 1),
[fromaddr] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[toaddr] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[time] [datetime] NULL,
[subject] [varchar] (180) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmailLog] ADD CONSTRAINT [PK_EmailLog] PRIMARY KEY CLUSTERED  ([id]) ON [PRIMARY]
GO
