using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CmsData;
using System.Web.Security;
using System.Web.Caching;
using System.Diagnostics;
using System.Net;
using System.Text;
using UtilityExtensions;

namespace Prayer
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            //RegisterCacheEntry();
        }
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
                DbUtil.Db.CurrentUser = DbUtil.Db.Users.SingleOrDefault(uu => uu.Username == User.Identity.Name);
           else
                DbUtil.Db.CurrentUser = new User { Username = Request.AnonymousID };
        }
        public void AnonymousIdentification_OnCreate(Object sender, AnonymousIdentificationEventArgs e)
        {
            e.AnonymousID = "anon_" + DateTime.Now.Ticks;
        }
        //private const string DummyKey = "dummykey";
        //private void RegisterCacheEntry()
        //{
        //    if (HttpContext.Current.Cache[DummyKey] != null)
        //        return;

        //    HttpContext.Current.Cache.Add(DummyKey, "Test", null,
        //        DateTime.Now.Date.AddDays(1).AddHours(2), Cache.NoSlidingExpiration,
        //        CacheItemPriority.Normal,
        //        new CacheItemRemovedCallback(CacheExpired));
        //}
        //public void CacheExpired(string key, object value, CacheItemRemovedReason reason)
        //{
        //    var client = new WebClient();
        //    client.DownloadData("/Home/CacheExpire");
        //    Prayer.Models.PsUtil.SendNotifications();
        //}
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //if (HttpContext.Current.Request.Url.LocalPath == "/Home/CacheExpire")
            //    RegisterCacheEntry();
        }
        //class DropthingsDataContext2 : DropthingsDataContext, IDisposable
        //{
        //    public new void Dispose()
        //    {
        //        if (base.Connection != null)
        //            if (base.Connection.State != System.Data.ConnectionState.Closed)
        //            {
        //                base.Connection.Close();
        //                base.Connection.Dispose();
        //            }

        //        base.Dispose();
        //    }
        //}
//        This file is part of Foobar.
// 
//Foobar is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
// 
//Foobar is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//GNU General Public License for more details.
// 
//You should have received a copy of the GNU General Public License
//along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
    }
}