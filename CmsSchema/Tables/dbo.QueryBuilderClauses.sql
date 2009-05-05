CREATE TABLE [dbo].[QueryBuilderClauses]
(
[QueryId] [int] NOT NULL IDENTITY(1, 1),
[ClauseOrder] [int] NOT NULL CONSTRAINT [DF_QueryBuilderClauses_ClauseOrder] DEFAULT ((0)),
[GroupId] [int] NULL,
[Field] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Comparison] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TextValue] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DateValue] [datetime] NULL,
[CodeIdValue] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StartDate] [datetime] NULL,
[EndDate] [datetime] NULL,
[DivOrg] [int] NOT NULL CONSTRAINT [DF_QueryBuilderClauses_DivOrgs] DEFAULT ((0)),
[SubDivOrg] [int] NOT NULL CONSTRAINT [DF_QueryBuilderClauses_SubDivOrgs] DEFAULT ((0)),
[Organization] [int] NOT NULL CONSTRAINT [DF_QueryBuilderClauses_Organization] DEFAULT ((0)),
[Days] [int] NOT NULL CONSTRAINT [DF_QueryBuilderClauses_Days] DEFAULT ((0)),
[SavedBy] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsPublic] [bit] NOT NULL CONSTRAINT [QueryBuilderIsPublic] DEFAULT ((0)),
[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_QueryBuilderClauses_ModifiedOn] DEFAULT (getdate()),
[Quarters] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SavedQueryIdDesc] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [DF_QueryBuilderClauses_SavedQueryId] DEFAULT ((0)),
[Tags] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Schedule] [int] NOT NULL CONSTRAINT [DF_QueryBuilderClauses_Schedule] DEFAULT ((0))
)


CREATE NONCLUSTERED INDEX [IX_GroupId] ON [dbo].[QueryBuilderClauses] ([GroupId])






GO
ALTER TABLE [dbo].[QueryBuilderClauses] ADD CONSTRAINT [PK_QueryBuilderClauses] PRIMARY KEY CLUSTERED  ([QueryId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_QueryBuilderClauses] ON [dbo].[QueryBuilderClauses] ([SavedBy]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[QueryBuilderClauses] WITH NOCHECK ADD CONSTRAINT [Clauses__Parent] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[QueryBuilderClauses] ([QueryId])
GO
