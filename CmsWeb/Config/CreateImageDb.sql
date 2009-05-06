/* Note: run this script in a freshly created database (ImageData for example)
 */
CREATE TABLE [dbo].[Image](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[bits] [varbinary](max) NULL,
	[length] [int] NULL,
	[mimetype] [varchar](20) NULL,
 CONSTRAINT [PK_ImageData] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY]
