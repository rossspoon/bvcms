/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using UtilityExtensions;
using System.Xml.Linq;
using System.Web.Caching;
using System.Data.SqlClient;

namespace CmsData
{
	public static partial class DbUtil
	{
		private const string CMSDbKEY = "CMSDbKey";
		//private static CMSDataContext _idb;
		private static CMSDataContext InternalDb
		{
			get
			{
				//return _idb;
				return (CMSDataContext)HttpContext.Current.Items[CMSDbKEY];
			}
			set
			{
				//_idb = value;
				HttpContext.Current.Items[CMSDbKEY] = value;
			}
		}
		public static CMSDataContext Db
		{
			get
			{
				if (HttpContext.Current == null)
					return new CMSDataContext(Util.ConnectionString);
				if (InternalDb == null)
				{
					InternalDb = new CMSDataContext(Util.ConnectionString);
					InternalDb.CommandTimeout = 1200;
				}
				return InternalDb;
			}
			set
			{
				InternalDb = value;
			}
		}
		public static void LogActivity(string activity)
		{
			var db = new CMSDataContext(Util.ConnectionString);
			int? uid = Util.UserId;
			if (uid == 0)
				uid = null;
			var a = new ActivityLog
			{
				ActivityDate = Util.Now,
				UserId = uid,
				Activity = activity,
				Machine = System.Environment.MachineName,
			};
			db.ActivityLogs.InsertOnSubmit(a);
			db.SubmitChanges();
			db.Dispose();
		}
		public static void DbDispose()
		{
			if (InternalDb != null)
			{
				InternalDb.Dispose();
				InternalDb = null;
			}
		}
		public static string StandardExtraValues()
		{
			var s = HttpRuntime.Cache[Db.Host + "StandardExtraValues"] as string;
			if (s == null)
			{
				s = Content("StandardExtraValues.xml", "<Fields />");
				HttpRuntime.Cache.Insert(Db.Host + "StandardExtraValues", s, null,
					 DateTime.Now.AddMinutes(Util.IsDebug() ? 0 : 2), Cache.NoSlidingExpiration);
			}
			return s;
		}
		public static string TopNotice()
		{
			var hc = HttpRuntime.Cache[Db.Host + "topnotice"] as string;
			if (hc == null)
			{
				var h = Content("TopNotice");
				if (h != null)
					hc = h.Body;
				else
					hc = string.Empty;
				HttpRuntime.Cache.Insert(Db.Host + "topnotice", hc, null,
					 DateTime.Now.AddMinutes(3), Cache.NoSlidingExpiration);
			}
			return hc;
		}
		public static string HeaderImage(string def)
		{
			var hc = HttpRuntime.Cache[Db.Host + "headerimg"] as string;
			if (hc == null)
			{
				var h = Content("HeaderImg");
				if (h != null)
					hc = h.Body;
				else
					hc = def;
				HttpRuntime.Cache.Insert(Db.Host + "headerimg", hc, null,
					 DateTime.Now.AddMinutes(3), Cache.NoSlidingExpiration);
			}
			return hc;
		}
		public static string Header()
		{
			var hc = HttpRuntime.Cache[Db.Host + "header"] as string;
			if (hc == null)
			{
				var h = Content("Header");
				if (h != null)
					hc = h.Body;
				else
					hc = @"
<div id='CommonHeaderImage'>
    <a href='/'><img src='/images/headerimage.jpg' /></a>
</div>
<div id='CommonHeaderTitleBlock'>
    <h1 id='CommonHeaderTitle'>Bellevue Baptist Church</h1>
    <h2 id='CommonHeaderSubTitle'>Feed My Sheep</h2>
</div>
";
				HttpRuntime.Cache.Insert(Db.Host + "header", hc, null,
					 DateTime.Now.AddMinutes(3), Cache.NoSlidingExpiration);
			}
			return hc;
		}

		public static Content Content(string name)
		{
			return DbUtil.Db.Contents.SingleOrDefault(c => c.Name == name);
		}

		public static Content ContentFromID(int id)
		{
			return DbUtil.Db.Contents.SingleOrDefault(c => c.Id == id);
		}

		public static string Content(string name, string def)
		{
			var content = DbUtil.Db.Contents.SingleOrDefault(c => c.Name == name);
			if (content != null)
				return content.Body;
			return def;
		}

		public static string SystemEmailAddress { get { return Db.Setting("SystemEmailAddress", ""); } }
		public static string AdminMail { get { return Db.Setting("AdminMail", SystemEmailAddress); } }
		public static string StartAddress { get { return Db.Setting("StartAddress", "2000+Appling+Rd,+Cordova,+Tennessee+38016"); } }
		public static bool CheckRemoteAccessRole { get { return Db.Setting("CheckRemoteAccessRole", "") == "true"; } }

		public const string MiscTagsString = "Misc Tags";
		public const int TagTypeId_Personal = 1;
		public const int TagTypeId_OrgMembersOnly = 3;
		public const int TagTypeId_OrgLeadersOnly = 6;
		public const int TagTypeId_CouplesHelper = 4;
		public const int TagTypeId_AddSelected = 5;
		public const int TagTypeId_ExtraValues = 6;
		public const int TagTypeId_Query = 7;
	}
}
