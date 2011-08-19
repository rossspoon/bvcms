/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models;

namespace CmsWeb.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateInput(false)]
    public class DisplayController : CmsStaffController
    {
        public ActionResult Index()
        {
            var q = from c in DbUtil.Db.Contents
                    orderby c.Name
                    select c;
            return View(q);
        }
        public ActionResult Page(string id)
        {
            if (!id.HasValue())
                id = "Home";
            Content content = null;
            if (id == "Recent")
                content = DbUtil.Db.Contents.OrderByDescending(c => c.DateCreated).First();
            else
                content = DbUtil.Db.Contents.SingleOrDefault(c => c.Name == id);
            ViewData["title"] = id;
            if (content != null)
            {
                ViewData["html"] = content.Body;
                ViewData["title"] = content.Title;
            }
            ViewData["page"] = id;
            return View();
        }
        public ActionResult EditPage(string id, bool? ishtml)
        {
            var content = DbUtil.Content(id);
            if (content != null)
            {
                ViewData["html"] = content.Body;
                ViewData["title"] = content.Title;
            }
            ViewData["id"] = id;
            ViewData["ishtml"] = ishtml ?? true;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdatePage(string id, string title, string html)
        {
            var content = DbUtil.Content(id);
            if (content == null)
            {
                content = new CmsData.Content { Name = id };
                DbUtil.Db.Contents.InsertOnSubmit(content);
            }
            content.Body = html;
            content.Title = title;
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeletePage(string id)
        {
            var content = DbUtil.Content(id);
            DbUtil.Db.Contents.DeleteOnSubmit(content);
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Index", "Display");
        }
        public ActionResult OrgContent(int id, string what, bool? div)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            if (div == true && org.Division == null)
                return Content("no main division");

            switch (what)
            {
                case "message":
                    if (div == true)
                    {
                        ViewData["html"] = org.Division.EmailMessage;
                        ViewData["title"] = org.Division.EmailSubject;
                    }
                    break;
                case "instructions":
                    if (div == true)
                        ViewData["html"] = org.Division.Instructions;
                    ViewData["title"] = "Instructions";
                    break;
                case "terms":
                    if (div == true)
                        ViewData["html"] = org.Division.Terms;
                    ViewData["title"] = "Terms";
                    break;
            }
            ViewData["id"] = id;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateOrgContent(int id, bool? div, string what, string title, string html)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);

            switch (what)
            {
                case "message":
                    if (div == true)
                    {
                        org.Division.EmailMessage = html;
                        org.Division.EmailSubject = title;
                    }
                    break;
                case "instructions":
                    if (div == true)
                        org.Division.Instructions = html;
                    break;
                case "terms":
                    if (div == true)
                        org.Division.Terms = html;
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return Redirect("/Organization/Index/" + id);
        }
        /*
        private void SetContentDefaults()
        {
            var a = new List<string>
            {   
                "NewUserEmail",                     @"Hi {name},
<p>You have a new account on our Church Management System which you can access at the following link:<br />
<a href=""{cmshost}"">{cmshost}</a></p>
<table>
<tr><td>Username:</td><td><b>{username}</b></td></tr>
<tr><td>Password:</td><td><b>{password}</b></td></tr>
</table>
<p>Please visit <a href=""{cmshost}/Display/Page/Welcome"">this welcome page</a> for more information</p>
<p>Thanks,<br />
The bvCMS Team</p>

                
         * 
         * 
         * "ShellDefault", 
          
         * 
         * 
         * "OnlineRegHeader",
          
         * 
         * 
         * "OnlineRegTop",
          
         * 
         * 
         * "OnlineRegBottom",
          
         * 
         * 
         * "CreateAccountRegistration", "CreateAccountConfirmation",
                        @"Hi {name},
<p>We have created an account you for in our church database.</p>
<p>This will make it easier for you to do online registrations.
Just use this account next time you register online.</p>
<p>And this will allow you to help us maintain your correct address, email and phone numbers.
Just login to {host} and you will be taken to your record where you can make corrections if needed.</p>
<p>The following are the credentials you can use. Both the username and password are system generated.
</p>
<blockquote><table>
<tr><td>Username:</td><td><strong><span style=""font-family: courier new, courier, monospace"">{username}</span></strong></td></tr>
<tr><td>Password:</td><td><strong><span style=""font-family: courier new, courier, monospace"">{password}</span></strong></td></tr>
</table></blockquote>
<p>Thank You</p>
          
         * 
         * 
         * "OneTimeConfirmation",
                    @"Hi {name},
<p>Here is your <a href=""{url}"">link</a> to manage your subscriptions. (note: it will only work once for security reasons)</p> ");
          
         * 
         * 
         * "DiffEmailMessage",
                    c.Body = @"Hi {name},
<p>You registered for {orgname} using a different email address than the one we have on record.
It is important that you call the church <strong>{phone}</strong> to update our records
so that you will receive future important notices regarding this registration.</p>";
                    c.Title = "{orgname}, different email address than one on record";
          
         * 
         * 
         * "NoEmailMessage",
                    c.Body = @"Hi {name},
<p>You registered for {orgname}, and we found your record, 
but there was no email address on your existing record in our database.
If you would like for us to update your record with this email address or another,
Please contact the church at <strong>{phone}</strong> to let us know.
It is important that we have your email address so that
you will receive future important notices regarding this registration.
But we won't add that to your record without your permission.

Thank you</p>";
                    c.Title = "{orgname}, no email on your record";
          
         * 
         * 
         * "GoDisciplesDisabled",
<p>
	&nbsp;</p>
<p>
	GO Disciples Registration is disabled temporarily for maintenance.</p>
<p>
	Please check back later today.</p>
<p>
	Thank you!</p>
          
         * 
         * 
         * "GODisciplesIndex",
<h2>
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
          
         * 
         * 
         * "GoDisciplesDisabled",
         <p>
	&nbsp;</p>
<p>
	GO Disciples Registration is disabled temporarily for maintenance.</p>
<p>
	Please check back later today.</p>
<p>
	Thank you!</p>

          
         * 
         * 
         * "GODisciplesGroupWelcome",
<p>
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
          
         * 
         * 
         * "GODisciplesFirstPost",
<p>
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
          
         * 
         * 
         * "GODisciplesLeaderConfirm",
<p>
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
         
         * 
         * 
         * * "GODisciplesIndividualConfirm",
<p>
	&nbsp;</p>
<div style="font-family: Arial, Verdana, sans-serif; font-size: 12px; color: rgb(34, 34, 34); background-color: rgb(255, 255, 255); ">
	<p>
		Hi {first},</p>
	<p>
		Welcome to GO Disciples and thank you for registering. Your account at {disciplesurl} is now set up for you to visit. You will be able to read the public blogs and resources. And use the Scripture Memory utility.</p>
	<p>
		Here are your credentials which you will use to access the site.</p>
	<blockquote>
		<p>
			Username:&nbsp;<strong>{username}</strong><br />
			Password:&nbsp;<strong>{password}</strong></p>
	</blockquote>
</div>
          
         * 
         * 
         * "GoDisciplesGroupConfirm",
<p>
	Hi {first}</p>
<p>
	Here are the credentials you will use to log onto the site:</p>
<p>
	Username: {username}</p>
<p>
	Password: &nbsp;{password}</p>
<p>
	Attention Bellevue Employees: If you only log into CMS2 you probably will not know your password for the external site. Please click the link above to request a new password.</p>
<p>
	Group: {groupname}</p>
<p>
	Link: <a href="{disciplesurl}">GoDisciples</a></p>
          
         * 
         * 
         * "GODisciplesConfirm",
<p>
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
          
         * 
         * 
         * "RegisterMessage",
          
         * 
         * 
         * "Site2Header",
		<div id=""header"">
		   <div id=""title"">
		      <h1><img alt=""logo"" src='{0}' align=""middle"" />&nbsp;{1}</h1>
		   </div>
		</div>".Fmt(logoimg, headertext);
            };
        }
         * */
    }
}
