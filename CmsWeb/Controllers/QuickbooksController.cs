using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;
using System.IO;
using CmsWeb.Models;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using CmsData;

namespace BvcmsWww.Controllers
{
	public class QuickbooksController : Controller
	{
		public static IToken requestToken;

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult RequestOAuthToken()
		{
			requestToken = QuickbooksModel.getOAuthSession().GetRequestToken();

			DbUtil.Db.ExecuteCommand( "UPDATE dbo.QBConnections SET Active = 0" );

			QBConnection qbc = new QBConnection();
			qbc.Creation = DateTime.Now;
			qbc.DataSource = "QBO";
			qbc.Token = requestToken.Token;
			qbc.UserID = 149;
			qbc.Active = 1;
			qbc.Secret = "";
			qbc.RealmID = "";

			DbUtil.Db.QBConnections.InsertOnSubmit(qbc);
			DbUtil.Db.SubmitChanges();

			// generate a user authorize url for this token (which you can use in a redirect from the current site)
			string authorizationLink = QuickbooksModel.getOAuthSession().GetUserAuthorizationUrlForToken(requestToken, QuickbooksModel.getCallback());

			return Redirect( authorizationLink );
		}

		public ActionResult RequestAccessToken()
		{
			IToken accessToken = QuickbooksModel.getOAuthSession().ExchangeRequestTokenForAccessToken(requestToken, Request["oauth_verifier"]);

			QBConnection qbc = (from i in DbUtil.Db.QBConnections
									  where i.Active == 1
									  select i).FirstOrDefault();

			qbc.Token = accessToken.Token;
			qbc.Secret = accessToken.TokenSecret;
			qbc.RealmID = Request["realmId"];
			DbUtil.Db.SubmitChanges();

			return View("Index");
		}
	}
}
