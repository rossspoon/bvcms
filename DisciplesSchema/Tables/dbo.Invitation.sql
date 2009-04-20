CREATE TABLE [dbo].[Invitation]
(
[password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[expires] [datetime] NULL,
[GroupId] [int] NOT NULL
)

GO
ALTER TABLE [dbo].[Invitation] ADD CONSTRAINT [PK_Invitation] PRIMARY KEY CLUSTERED ([password], [GroupId])
GO
ALTER TABLE [dbo].[Invitation] ADD CONSTRAINT [FK_Invitation_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id])
GO
