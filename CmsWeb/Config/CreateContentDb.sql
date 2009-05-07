/****** Object:  Table [dbo].[Content]    Script Date: 05/06/2009 18:33:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Content](
	[Name] [varchar](50) NOT NULL,
	[Title] [varchar](500) NULL,
	[Body] [varchar](max) NULL,
	[DateCreated] [datetime] NULL,
 CONSTRAINT [PK_Content_1] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES (N'Header', N'Header', N'
<div style="float: left"><a href="/">
<img style="border-right-width: 0px; margin: 0px 20px 0px 5px; display: inline; border-top-width: 0px; border-bottom-width: 0px; border-left-width: 0px" title="bvcms" border="0" alt="bvcms" width="240" height="74" src="/Content/images/bvcms_8.png" /></a></div>
<div style="float: left">
<h1 style="color=#004080;margin-bottom:10px">Demo Baptist Church</h1>
<font color="#004080" size="4" face="Arial">
<em>A church where nothing is real</em></font></div>
', NULL)
INSERT [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES (N'Settings', N'Settings', N'<dl>
    <dt>BFClassOrgTagId</dt>
    <dd>1</dd>
    <dt>TaskHost</dt>
    <dd>http://demo.bvcms.com</dd>
    <dt>CheckRemoteAccessRole</dt>
    <dd>false</dd>
    <dt>NewPeopleManagerId</dt>
    <dd>1</dd>
    <dt>SystemEmailAddress</dt>
    <dd>david@davidcarroll.name</dd>
    <dt>MaxExcelRows</dt>
    <dd>10000</dd>
    <dt>ChangePasswordDays</dt>
    <dd>360</dd>
    <dt>BlogAppUrl</dt>
    <dd>http://www.bvcms.com/blog/</dd>
    <dt>BlogFeedUrl</dt>
    <dd>http://feeds2.feedburner.com/Bvcms-Blog</dd>
    <dt>QAServer</dt>
    <dd></dd>
</dl>', NULL)
INSERT [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES (N'TermsOfUse', N'Terms Of Use', N'
<div style="width: 300px">
<p><span style="font-size: medium">Access to this site is given by request only. Please visit </span><a href="http://www.bvcms.com"><span style="font-size: medium">http://www.bvcms.com</span></a><span style="font-size: medium">&nbsp;and read the Demo Site page for more information about how to request credentials.</span></p>
<p>This web site is for demonstration purposes. Any resemblance to real people on this site is coincidental and unintended.</p>
<p>Copyright (c) 2009, Bellevue Baptist Church.</p>
<p>The source code is licensed under the GPL (see <a href="http://bvcms.codeplex.com/license">license</a>)</p>
<p>By logging in below, you agree that you understand these terms.</p>
</div>
', NULL)
