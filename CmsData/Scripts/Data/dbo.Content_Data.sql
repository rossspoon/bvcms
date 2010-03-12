INSERT INTO [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES ('GODisciplesConfirm', 'GO Disciples Welcome', '<p>
	Hi {first},</p>
<p>
	Welcome to GO Disciples and thank you for joining a discipleship group. Your account at {disciplesurl} is&nbsp;now set up for you to visit. You will be able to read your leader&#39;s&nbsp;blog, comment and&nbsp;discuss with&nbsp;your group members. Note that all access to your group&#39;s blog will be for your fellow group members only and&nbsp;<u>not</u> visible to the public.&nbsp;Your group name is:</p>
<blockquote>
	Groupname: <strong>{groupname}</strong><br />
	Disciples site: <a href="{disciplesurl}">{disciplesurl}</a></blockquote>
<p>
	Here are your credentials which you will use to access the site&nbsp;and the blog.</p>
<blockquote>
	<p>
		Username: <strong>{username}</strong><br />
		Password: <strong>{password}</strong></p>
</blockquote>
<p>
	Note that if you were a user on&nbsp;disciples.bellevue.org we have kept your account but have changed your credentials to match these.</p>
<p>
	Blessings,<br />
	{minister}</p>
', NULL)
INSERT INTO [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES ('GODisciplesDisabled', 'GO Disciples Disabled', '<p>
	&nbsp;</p>
<p>
	GO Disciples Registration is disabled temporarily for maintenance.</p>
<p>
	Please check back later today.</p>
<p>
	Thank you!</p>
', NULL)
INSERT INTO [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES ('GODisciplesFirstPost', 'Welcome', '<p>
	This is your discipleship group blog. And this is an automatically generated first post so that you won&#39;t be looking at a blank screen when you first come here!</p>
<p>
	Be sure to check out the verse memorization utility under Bible Tools on the menu.</p>
<p>
	Also you have a Chronological Bible Reading plan on that menu too. You can choose the reading you want to start with (Gen 1-3 for example) and click the &quot;current&quot; link. This will associate that reading for today&#39;s date and all the remaining readings will be dated appropriately. This way you can start your one year reading plan on any day. No need to wait till January 1st! Also, this way, if you get a bit behind, you can reset the calendar at any point. No excuses! If you fall off your horse, just dust yourself off and get back on again.</p>
<p>
	Note, that the members of this group can comment on these posts too. This way, you can use it as a discussion forum. So throw a jump question up there and let the commenting begin. Of course, you can use these posts for announcements too. Your members will all receive an email notice whenever a new post is available or someone comments. They can opt out too by clicking on a link in the email.</p>
<p>
	Finally, remember that this group and blog are private, visible to your fellow group members only.</p>
<p>
	Blessings,<br />
	The GO Disciples Team</p>
', NULL)
INSERT INTO [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES ('GODisciplesGroupWelcome', 'Welcome to the {name}', '<p>
	Hello GO Disciples,</p>
<p>
	&nbsp;</p>
<p>
	We will be doing a chronological study through the entire Bible with the intent of becoming a disciple maker and serving the body of Christ and walking with Him.</p>
<p>
	Check out the Bible Tools, Chronological Reading Plan and the Verse Memorization utility.</p>
<p>
	You should notice under the Blogs Menu, the name of our blog too. This is where we will be having discussions and I&#39;ll be posting notices there too.</p>
<p>
	Blessings,<br />
	{leader}</p>
', NULL)
INSERT INTO [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES ('GODisciplesIndex', 'GO Disciples Index', '<h2>
	GO Disciples Leader Registration (leaders only please)</h2>
<p>
	Click the link for the church with which you are associated.</p>
<ul>
	<li>
		<a href="http://cms.bellevue.org/GODisciples/Leader">Bellevue</a></li>
	<li>
		<a href="http://cms.bellevue.org/GODisciples/Leader/11">Colonial Heights - </a><a href="http://cms.bellevue.org/GODisciples/Leader/11">Jimmy Meek</a></li>
	<li>
		<a href="http://cms.bellevue.org/GODisciples/Leader/12">Faith Baptist - </a><a href="http://cms.bellevue.org/GODisciples/Leader/12">David Smith</a></li>
	<li>
		<a href="http://cms.bellevue.org/GODisciples/Leader/13">Kirby Woods - Casey Pearson</a></li>
	<li>
		<a href="http://cms.bellevue.org/GODisciples/Leader/14">First Baptist Orlando - Jack Fiscus</a></li>
	<li>
		<a href="http://cms.bellevue.org/GODisciples/Leader/15">First Baptist Basehor - Duane McCracken</a></li>
</ul>
', NULL)
INSERT INTO [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES ('GODisciplesIndividualConfirm', 'GO Disciples Individual Confirm', '<p>
	Hi {first},</p>
<p>
	Welcome to GO Disciples and thank you for registering. Your account at {disciplesurl} is now set up for you to visit. You will be able to read the public blogs and resources. And use the Scripture Memory utility.</p>
<p>
	Here are your credentials which you will use to access the site.</p>
<blockquote>
	<p>
		Username: <strong>{username}</strong><br />
		Password: <strong>{password}</strong></p>
</blockquote>
', NULL)
INSERT INTO [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES ('GODisciplesLeaderConfirm', 'GO Disciples Welcome', '<p>
	Hi {first},</p>
<p>
	Welcome to GO Disciples and thank you for being a Leader. Everything should now be set up for you to invite your disciples to join your group. You will be able to blog, discuss, and track attendance of your group members. Note that all access to your group blog will be for your group members only and&nbsp;<u>not</u> visible to the public. &nbsp;Here are your credentials which you will use to access both the attendance tracking site and the blog site.</p>
<blockquote>
	<p>
		Username:&nbsp;<strong>{username}<br />
		Password:&nbsp;<strong>{password}</strong></strong></p>
</blockquote>
<p>
	Your group name is:</p>
<blockquote>
	Groupname: <strong>{groupname}</strong><br />
	Disciples site: <strong><a href="{disciplesurl}">{disciplesurl}</a></strong></blockquote>
<p>
	Please keep this email&nbsp;in a safe place&nbsp;since it has links and instructions for you to use the GO Disciples facilities. Note that if you were a user on either disciples.bellevue.org or on cms.bellevue.org we have kept your account but have changed your credentials to match these.</p>
<p>
	Use the following link to send to your disciples so they can sign-up and join your group online:</p>
<blockquote>
	<p>
		Member sign-up: <a href="{membersignupurl}"><strong>{membersignupurl}</strong></a> &nbsp; &nbsp; &nbsp; (see Sample Email text in postscript)</p>
</blockquote>
<p>
	When they join, you will be notified by email as well. Once they join, you can email your members,&nbsp;and create meetings to record attendance at the following link:</p>
<blockquote>
	<p>
		CMS Organization: <strong><a href="{cmsorgpageurl}">{cmsorgpageurl}</a></strong><br />
		CMS Home Page: <strong><a href="http://cms.bellevue.org">http://cms.bellevue.org</a></strong></p>
</blockquote>
<p>
	Note that you are also a member of the GO Disciples Leaders Group. There you will be able to participate with other GO leaders and have discussions about ideas for how to lead your groups. You&#39;ll also find a couple of&nbsp;posts there telling you how to&nbsp;post on your own blog and how to record attendance. You should be able to see both the leaders blog and your blog&nbsp;in the dropdown Blogs menu. Finally, you should consider purchasing the lesson materials from New Tribe Ministries on <u>Firm Foundations, from Creation to Christ.</u> See the Resources Menu on the disciples site. You can see a sample lesson and there is a link where you can purchase all the lessons for $20.00. After your purchase, you will be given special access to a page where you download zip files containing the PowerPoints and PDFs.</p>
<p>
	Blessings,<br />
	{minister}</p>
<p>
	PS: &nbsp;<strong>Sample email to send to your members</strong></p>
<p>
	Hi&nbsp;</p>
<p>
	Here&#39;s the link to join my GO Disciples small group. Click the link below to complete the short form. It will take only a minute. You will receive an email confirmation with further instructions regarding access to our blog on the Bellevue Disciples website. I&#39;ll be notified, too.</p>
<p>
	Member sign-up: <strong>{membersignupurl}</strong></p>
<p>
	Thanks,</p>
<p>
	<br />
	{first}</p>
', NULL)
INSERT INTO [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES ('Header', 'Header', '<div style="float: left">
	<a href="/"><img alt="bvcms" border="0" height="74" src="/Content/images/bvcms_8.png" style="margin: 0px 20px 0px 5px" title="bvcms" width="240" /></a></div>
<div style="float: left">
	<h2>
		Bellevue Baptist Church</h2>
	<p>
		<font color="#004080" face="Arial" size="2"><em>Love God, Love People, Share Jesus, Make Disciples</em></font></p>
</div>
', '2009-04-17 20:42:09.213')
INSERT INTO [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES ('TermsOfUse', 'Terms Of Use', '
<div style="width: 300px">
<p><span style="font-size: medium">Access to this site is given by special pemission only.</span></p>
<p>This web site has a starter database.</p>
<p>The source code is licensed under the GPL (see <a href="http://bvcms.codeplex.com/license">license</a>)</p>
</div>
', NULL)
INSERT INTO [dbo].[Content] ([Name], [Title], [Body], [DateCreated]) VALUES ('Welcome', 'Welcome new user', '<p>
	Once you have successfully logged in, you can change your&nbsp;<span>password&nbsp;(we encourage you to do so).</span></p>
<p>
	To change your password on CMS:</p>
<ol>
	<li>
		Click the &quot;Change Password&quot; link in the top right corner of the CMS Home Page</li>
	<li>
		In box one, enter the password we assigned you</li>
	<li>
		In box two, enter your new password (for security purposes the password must be at least 7 characters and contain at least one mark of punctuation&hellip;like a period, a colon, an exclamation point)</li>
	<li>
		In box three, re-type your new password to confirm it</li>
	<li>
		Click the Change Password button</li>
</ol>
<p>
	There are links on the login page which will help you if you forget your username or password. There is also a blog post on CMS2 News explaining how those work.</p>
<p>
	You will begin receiving <b>weekly emails</b> from your ministry coordinator (for Bible Fellowship, Choir, etc.) letting you know that the attendance reports are ready for viewing. The email will contain a link which will take you to the log in page for CMS. As soon as you log on, you will be taken the meeting page for your class. On that page, you will see a list of everyone who attended that day. You can also access a PDF report &ndash; the Attendance Summary Report (which lists all absent members and all visitors) or the Attendee Report (which lists those who attended&amp;emdash;both members and visitors). Of course, you can access CMS anytime using the link above and will be able to view these and other reports. For Deacons meetings, you will receive these emails monthly.</p>
<p>
	We recommend that you view the short <b>Screencasts for Lay Leaders (5-7 minutes each)</b>. You will see a link to these in the <b>CMS2 News</b> section on the Home Page. These are brief instructional screencasts showing you some of the most common features. There are other blogs that you may find helpful as well.</p>
<p>
	Please let me know if you have any problems accessing the CMS. I hope you find this a helpful tool in your ministry.</p>
<p>
	&nbsp;</p>
<div>
	<i><font color="#000080">BVCMS Team</font></i></div>
<div>
	<i><font color="#000080"><a href="mailto:helpdesk@bellevue.org">helpdesk@bellevue.org</a> </font></i></div>
<div>
	<i><font color="#000080"><span>Bellevue&nbsp;Baptist Church</span></font></i></div>
<div>
	<i><font color="#000080">901-347-5880</font></i></div>', NULL)
