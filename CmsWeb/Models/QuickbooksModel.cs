using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Intuit.Ipp.Core;
using Intuit.Ipp.Security;
using Intuit.Ipp.Services;
using Intuit.Ipp.Data;
using Intuit.Ipp.Data.Qbo;
using CmsData;
using UtilityExtensions;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;

namespace CmsWeb.Models
{
	public static class QuickbooksModel
	{
		public const string QB_EV_ID = "QBID";
		public const string QB_EV_SYNC_TOKEN = "QBSyncToken";
		public const string QB_EV_AMOUNT = "Amount";

		// TODO: Util.Host for callback to assign the church
		public const string QB_CALLBACK = "http://www.bvcms.com/Quickbooks/RequestAccessToken";
		public const string QB_REQUEST_TOKEN = "https://oauth.intuit.com/oauth/v1/get_request_token";
		public const string QB_AUTHORIZE = "https://appcenter.intuit.com/Connect/Begin";
		public const string QB_ACCESS_TOKEN = "https://oauth.intuit.com/oauth/v1/get_access_token";

		private static DataServices qbds;
		private static ServiceContext qbsc;
		private static OAuthConsumerContext qboacc;
		private static OAuthSession qboas;

		public static string getToken()
		{
			return System.Configuration.ConfigurationManager.AppSettings["QuickBooksToken"] ?? "";
		}

		public static string getKey()
		{
			return System.Configuration.ConfigurationManager.AppSettings["QuickBooksKey"] ?? "";
		}

		public static string getSecret()
		{
			return System.Configuration.ConfigurationManager.AppSettings["QuickBooksSecret"] ?? "";
		}

		public static string getCallback()
		{
			// Development
			return QB_CALLBACK;

			// Production
			//return QB_CALLBACK_PREFIX + Util.Host + QB_CALLBACK_SUFFIX;
		}

		public static OAuthConsumerContext getOAuthConsumerContext()
		{
			if (qboacc == null)
			{
				qboacc = new OAuthConsumerContext
				{
					ConsumerKey = QuickbooksModel.getKey(),
					SignatureMethod = SignatureMethod.HmacSha1,
					ConsumerSecret = QuickbooksModel.getSecret()
				};
			}

			return qboacc;
		}

		public static OAuthSession getOAuthSession()
		{
			if (qboas == null)
			{
				qboas = new OAuthSession(QuickbooksModel.getOAuthConsumerContext(), QB_REQUEST_TOKEN, QB_AUTHORIZE, QB_ACCESS_TOKEN, getCallback());
			}

			return qboas;
		}

		public static ServiceContext getServiceContext()
		{
			if (qbsc == null)
			{
				var qbc = (from i in DbUtil.Db.QBConnections
						   where i.Active == 1
						   select i).SingleOrDefault();

				var orv = new OAuthRequestValidator(qbc.Token, qbc.Secret, getKey(), getSecret());

				qbsc = new ServiceContext(orv, getToken(), qbc.RealmID, IntuitServicesType.QBO);
			}

			return qbsc;
		}

		public static DataServices getDataService()
		{
			if (qbds == null)
			{
				qbds = new DataServices(getServiceContext());
			}

			return qbds;
		}
	}
}