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

		public const string QB_REQUEST_TOKEN = "https://oauth.intuit.com/oauth/v1/get_request_token";
		public const string QB_AUTHORIZE = "https://appcenter.intuit.com/Connect/Begin";
		public const string QB_ACCESS_TOKEN = "https://oauth.intuit.com/oauth/v1/get_access_token";

		public static string QB_CALLBACK = DbUtil.Db.CmsHost + "/Quickbooks/RequestAccessToken";

		// Account Creation Codes
		public const int QB_ACCOUNT_NONPROFITINCOME = 1;

		// QuickBooks Objects
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

		public static bool hasActiveConnection()
		{
			return (from i in DbUtil.Db.QBConnections
					where i.Active == 1
					select i).Count() > 0;
		}

		public static List<Account> ListAllAccounts() // Max per page is 100
		{
			return getDataService().FindAll(new Account(), 1, 100).ToList<Account>();
		}

		public static bool CreateAccount( int type, string name, string number ) // Name and Subtype are required
		{
			Account aNew = new Account();
			aNew.Name = name;
			aNew.AcctNum = number;

			switch( type )
			{
				case QB_ACCOUNT_NONPROFITINCOME:
					aNew.Subtype = QboAccountDetailTypeEnum.NonProfitIncome.ToString();
					break;

				default: break;
			}

			Account aMade = getDataService().Add(aNew) as Account;

			if (aMade.Id.Value.ToInt() > 0 && aMade.SyncToken.ToInt() > 0) return true;
			else return false;
		}

		public static Account FetchAccount(string name)
		{
			AccountQuery aq = new AccountQuery();
			aq.Name = name;

			return aq.ExecuteQuery<Account>( getServiceContext() ).FirstOrDefault();
		}

		public static bool CreateJournalEntry( string desc, decimal amount, int from, int to )
		{
			JournalEntry jeNew = new JournalEntry();

			JournalEntryHeader jeh = new JournalEntryHeader();
			jeh.Note = desc;

			JournalEntryLine jelFrom = new JournalEntryLine();
			jelFrom.Desc = desc;
			jelFrom.Amount = amount;
			jelFrom.AmountSpecified = true;
			jelFrom.AccountId = new IdType() { Value = from.ToString() };
			jelFrom.PostingType = PostingTypeEnum.Debit;
			jelFrom.PostingTypeSpecified = true;

			JournalEntryLine jelTo = new JournalEntryLine();
			jelTo.Desc = desc;
			jelTo.Amount = amount;
			jelTo.AmountSpecified = true;
			jelTo.AccountId = new IdType() { Value = to.ToString() };
			jelTo.PostingType = PostingTypeEnum.Credit;
			jelTo.PostingTypeSpecified = true;

			jeNew.Header = jeh;
			jeNew.Line = new JournalEntryLine[] { jelFrom, jelTo };

			JournalEntry jeMade = getDataService().Add(jeNew) as JournalEntry;

			if (jeMade.Id.Value.ToInt() > 0 && jeMade.SyncToken.ToInt() > 0) return true;
			else return false;
		}
	}
}