CREATE TABLE [dbo].[FamilyCheckinLock]
(
[FamilyId] [int] NOT NULL,
[Locked] [bit] NOT NULL,
[Created] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FamilyCheckinLock] ADD CONSTRAINT [PK_FamilyCheckinLock] PRIMARY KEY CLUSTERED  ([FamilyId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FamilyCheckinLock] ADD CONSTRAINT [FK_FamilyCheckinLock_FamilyCheckinLock1] FOREIGN KEY ([FamilyId]) REFERENCES [dbo].[Families] ([FamilyId])
GO
