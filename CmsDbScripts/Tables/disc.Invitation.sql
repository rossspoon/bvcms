CREATE TABLE [disc].[Invitation]
(
[password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[expires] [datetime] NULL,
[GroupId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [disc].[Invitation] ADD CONSTRAINT [PK_Invitation] PRIMARY KEY CLUSTERED  ([password], [GroupId]) ON [PRIMARY]
GO
ALTER TABLE [disc].[Invitation] ADD CONSTRAINT [FK_Invitation_Group] FOREIGN KEY ([GroupId]) REFERENCES [disc].[Group] ([Id])
GO
