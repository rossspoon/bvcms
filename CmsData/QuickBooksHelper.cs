using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using UtilityExtensions;
using Intuit.Ipp.Core;
using Intuit.Ipp.Security;
using Intuit.Ipp.Services;
using Intuit.Ipp.Data;
using Intuit.Ipp.Data.Qbo;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;

namespace CmsData
{
    public class QuickBooksHelper
    {
        private IToken itToken;
        private String sBaseURL;

        public QuickBooksHelper() { }

        // Request constructor is needed for connection and disconnection
        public QuickBooksHelper(HttpRequestBase request)
        {
            sBaseURL = request.Url.Scheme + "://" + request.Url.Authority;
        }

        public string RequestOAuthToken()
        {
            itToken = getOAuthSession().GetRequestToken();

            DbUtil.Db.ExecuteCommand("UPDATE dbo.QBConnections SET Active = 0");

            QBConnection qbc = new QBConnection();
            qbc.Creation = DateTime.Now;
            qbc.DataSource = "QBO";
            qbc.Token = itToken.Token;
            qbc.UserID = 149;
            qbc.Active = 1;
            qbc.Secret = "";
            qbc.RealmID = "";

            DbUtil.Db.QBConnections.InsertOnSubmit(qbc);
            DbUtil.Db.SubmitChanges();

            // generate a user authorize url for this token (which you can use in a redirect from the current site)
            string authorizationLink = getOAuthSession().GetUserAuthorizationUrlForToken(itToken, getCallback());

            return authorizationLink;
        }

        public bool RequestAccessToken( string sRealmID, string sVerifier )
        {
            IToken accessToken = getOAuthSession().ExchangeRequestTokenForAccessToken(itToken, sVerifier);

            QBConnection qbc = (from i in DbUtil.Db.QBConnections
                                where i.Active == 1
                                select i).FirstOrDefault();

            qbc.Token = accessToken.Token;
            qbc.Secret = accessToken.TokenSecret;
            qbc.RealmID = sRealmID;
            DbUtil.Db.SubmitChanges();

            return true;
        }

        public bool Disconnect()
        {
            bool complete = doDisconnect();

            if (complete) DbUtil.Db.ExecuteCommand("UPDATE dbo.QBConnections SET Active = 0 WHERE Active = 1");

            return complete;
        }

        // Begin Common QuickBooks Routines
        public const string QB_EV_ID = "QBID";
		public const string QB_EV_SYNC_TOKEN = "QBSyncToken";
		public const string QB_EV_AMOUNT = "Amount";

		public const string QB_REQUEST_TOKEN = "https://oauth.intuit.com/oauth/v1/get_request_token";
		public const string QB_AUTHORIZE = "https://appcenter.intuit.com/Connect/Begin";
		public const string QB_ACCESS_TOKEN = "https://oauth.intuit.com/oauth/v1/get_access_token";
		public const string QB_DISCONNECT = "https://appcenter.intuit.com/api/v1/Connection/Disconnect";

        public const string QB_CALLBACK = "/Quickbooks/RequestAccessToken";

		// Account Creation Codes
		public const int QB_ACCOUNT_NONPROFITINCOME = 1;

		// QuickBooks Objects
		private static DataServices qbds;
		private static ServiceContext qbsc;
		private static OAuthConsumerContext qboacc;
		private static OAuthSession qboas;

		public string getToken()
		{
			return System.Configuration.ConfigurationManager.AppSettings["QuickBooksToken"] ?? "";
		}

		public string getKey()
		{
			return System.Configuration.ConfigurationManager.AppSettings["QuickBooksKey"] ?? "";
		}

		public string getSecret()
		{
			return System.Configuration.ConfigurationManager.AppSettings["QuickBooksSecret"] ?? "";
		}

        public string getCallback()
		{
            return sBaseURL + QB_CALLBACK;
		}

        public string getBaseURL()
        {
            return sBaseURL;
        }

		public OAuthConsumerContext getOAuthConsumerContext()
		{
			if (qboacc == null)
			{
				qboacc = new OAuthConsumerContext
				{
					ConsumerKey = getKey(),
					SignatureMethod = SignatureMethod.HmacSha1,
					ConsumerSecret = getSecret()
				};
			}

			return qboacc;
		}

		public OAuthSession getOAuthSession()
		{
			if (qboas == null)
			{
				qboas = new OAuthSession(getOAuthConsumerContext(), QB_REQUEST_TOKEN, QB_AUTHORIZE, QB_ACCESS_TOKEN, getCallback());
			}

			return qboas;
		}

		public ServiceContext getServiceContext()
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

		public DataServices getDataService()
		{
			if (qbds == null)
			{
				qbds = new DataServices(getServiceContext());
			}

			return qbds;
		}

		public TokenBase getAccessToken()
		{
			var qbc = (from i in DbUtil.Db.QBConnections
					   where i.Active == 1
					   select i).SingleOrDefault();

			return new TokenBase { Token = qbc.Token, ConsumerKey = getKey(), TokenSecret = qbc.Secret };
		}

		public bool doDisconnect()
		{
			OAuthSession oas = getOAuthSession();
			oas.ConsumerContext.UseHeaderForOAuthParameters = true;
			oas.AccessToken = getAccessToken();

			IConsumerRequest icr = oas.Request();

			icr = icr.Get();
			icr = icr.ForUrl( QB_DISCONNECT );
			icr = icr.SignWithToken();
			var ret = icr.ToWebResponse();

			if (ret.StatusCode.ToInt() == 200) return true;
			else return false;
		}

		public bool hasActiveConnection()
		{
			return (from i in DbUtil.Db.QBConnections
					where i.Active == 1
					select i).Count() > 0;
		}

		public List<Account> ListAllAccounts() // Max per page is 100
		{
			//AccountQuery aq = new AccountQuery();
			//return aq.ExecuteQuery<Account>( getServiceContext() ).ToList<Account>();
			return getDataService().FindAll(new Account(), 1, 100).ToList<Account>();
		}

		public bool CreateAccount( int type, string name, string number ) // Name and Subtype are required
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

		public Account FetchAccount(string name)
		{
			AccountQuery aq = new AccountQuery();
			aq.Name = name;

			return aq.ExecuteQuery<Account>( getServiceContext() ).FirstOrDefault();
		}

        List<JournalEntryLine> jelEntries;

        public void InitJournalEntires()
        {
            jelEntries = new List<JournalEntryLine>();
        }

        public bool AddJournalEntry( JournalEntryLine jel )
        {
            if (jelEntries == null) return false;

            jelEntries.Add(jel);
            return true;
        }

        public int CommitJournalEntries( string desc )
		{
            if (jelEntries == null) return 0;
            if (jelEntries.Count() == 0) return 0;

			JournalEntry jeNew = new JournalEntry();

			JournalEntryHeader jeh = new JournalEntryHeader();
			jeh.Note = desc;

			jeNew.Header = jeh;
			jeNew.Line = jelEntries.ToArray();

			JournalEntry jeMade = getDataService().Add(jeNew) as JournalEntry;

            if (jeMade.Id.Value.ToInt() > 0 && jeMade.SyncToken.ToInt() > -1) return jeMade.Id.Value.ToInt();
			else return 0;
		}
    }
}
