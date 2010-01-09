CREATE TABLE [disc].[UploadAuthenticationXref]
(
[postinguser] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[postsfor] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
ALTER TABLE [disc].[UploadAuthenticationXref] ADD CONSTRAINT [PK_UploadAuthenticationXref] PRIMARY KEY CLUSTERED  ([postinguser], [postsfor])
GO
