CREATE TABLE [lookup].[MemberType]
(
[Id] [int] NOT NULL,
[Code] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AttendanceTypeId] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [lookup].[MemberType] ADD CONSTRAINT [PK_MemberType] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [lookup].[MemberType] ADD CONSTRAINT [FK_MemberType_AttendType] FOREIGN KEY ([AttendanceTypeId]) REFERENCES [lookup].[AttendType] ([Id])
GO
