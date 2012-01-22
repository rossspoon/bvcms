using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilityExtensions;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace CmsData
{
    public class AuthorizeNet : IDisposable
    {
        XNamespace ns = "AnetApi/xml/v1/schema/AnetApiSchema.xsd";
        const string produrl = "https://api.authorize.net/xml/v1/request.api";
        const string testurl = "https://apitest.authorize.net/xml/v1/request.api";
        string testMode = "";
        string url;
        string login;
        string key;
        CMSDataContext Db;
        public AuthorizeNet(CMSDataContext Db, bool testing)
        {
#if DEBUG
            testing = true;
#endif
            this.Db = Db;
            if (testing)
            {
                login = "9t8Pqzs4CW3S";
                key = "9j33v58nuZB865WR";
                url = testurl;
                testMode = "testMode";
            }
            else
            {
                login = Db.Setting("x_login", "");
                key = Db.Setting("x_tran_key", "");
                url = produrl;
                testMode = "liveMode";
            }
        }
        private XDocument getResponse(string request)
        {
            var wc = new WebClient();
            wc.Headers.Add("Content-Type", "text/xml");
            var bits = Encoding.UTF8.GetBytes(request);
            var ret = wc.UploadData(url, "POST", bits);
            using (var xmlStream = new MemoryStream(ret))
            using (var xmlReader = new XmlTextReader(xmlStream))
            {
                var x = XDocument.Load(xmlReader);
                var result = x.Descendants(ns + "resultCode").First().Value;
                if (result == "Error")
                {
                    var message = x.Descendants(ns + "text").First().Value;
                    throw new Exception(message);
                }
                return x;
            }
        }

public void AddUpdateCustomerProfile(int PeopleId,
    string semievery,
    int? day1,
    int? day2,
    int? everyn,
    string period,
    DateTime? startwhen,
    DateTime? stopwhen,
    string type,
    string cardnumber,
    string expires,
    string cardcode,
    string routing,
    string account,
    bool testing)
{
    var p = Db.LoadPersonById(PeopleId);
    var rg = p.RecurringGiving();
    if (rg.AuNetCustId == null) // create a new profilein Authorize.NET CIM
    {
        var request = new XDocument(new XDeclaration("1.0", "utf-8", null),
            Element("createCustomerProfileRequest",
                Element("merchantAuthentication",
                    Element("name", login),
                    Element("transactionKey", key)
                    ),
                Element("profile",
                    Element("merchantCustomerId", PeopleId),
                    Element("email", p.EmailAddress),
                    Element("paymentProfiles",
                        Element("billTo",
                            Element("firstName", p.FirstName),
                            Element("lastName", p.LastName),
                            Element("address", p.PrimaryAddress),
                            Element("city", p.PrimaryCity),
                            Element("state", p.PrimaryState),
                            Element("zip", p.PrimaryZip),
                            Element("phoneNumber", p.HomePhone)
                            ),
                        Element("payment",
                            Element("creditCard",
                                Element("cardNumber", cardnumber),
                                Element("expirationDate", expires),
                                Element("cardCode", cardcode)
                                ),
                            Element("bankAccount",
                                Element("routingNumber", routing),
                                Element("accountNumber", account),
                                Element("nameOnAccount", p.Name)
                                )
                            )
                        )
                    ),
                Element("validationMode", testMode)
                )
            );
        var s = request.ToString();
        var x = getResponse(s);
        var id = x.Descendants(ns + "customerProfileId").First().Value.ToInt();
        var pid = x.Descendants(ns + "customerPaymentProfileIdList")
                    .Descendants(ns + "numericString").First().Value.ToInt();
        rg.AuNetCustId = id;
        rg.AuNetCustPayId = pid;
    }
            else
            {
                if (account.StartsWith("X"))
                {
                    var xe = getCustomerPaymentProfile(PeopleId);
                    var xba = xe.Descendants(ns + "bankAccount").Single();
                    routing = xba.Element(ns + "routingNumber").Value;
                    account = xba.Element(ns + "accountNumber").Value;
                }

                var request = new XDocument(new XDeclaration("1.0", "utf-8", null),
                    Element("updateCustomerProfileRequest",
                        Element("merchantAuthentication",
                            Element("name", login),
                            Element("transactionKey", key)
                            ),
                        Element("profile",
                            Element("merchantCustomerId", PeopleId),
                            Element("email", p.EmailAddress),
                            Element("customerProfileId", rg.AuNetCustId)
                            ),
                        Element("validationMode", testMode)
                    )
                );
                var x = getResponse(request.ToString());
                request = new XDocument(new XDeclaration("1.0", "utf-8", null),
                    Element("updateCustomerPaymentProfileRequest",
                        Element("merchantAuthentication",
                            Element("name", login),
                            Element("transactionKey", key)
                            ),
                        Element("customerProfileId", rg.AuNetCustId),
                        Element("paymentProfile",
                            Element("billTo",
                                Element("firstName", p.FirstName),
                                Element("lastName", p.LastName),
                                Element("address", p.PrimaryAddress),
                                Element("city", p.PrimaryCity),
                                Element("state", p.PrimaryState),
                                Element("zip", p.PrimaryZip),
                                Element("phoneNumber", p.HomePhone)
                                ),
                            Element("payment",
                                Element("creditCard",
                                    Element("cardNumber", cardnumber),
                                    Element("expirationDate", expires),
                                    Element("cardCode", cardcode)
                                    ),
                                Element("bankAccount",
                                    Element("routingNumber", routing),
                                    Element("accountNumber", account),
                                    Element("nameOnAccount", p.Name)
                                    )
                                ),
                            Element("customerPaymentProfileId", rg.AuNetCustPayId)
                        )
                    ),
                    Element("validationMode", testMode)
                );
                x = getResponse(request.ToString());
            }
            rg.SemiEvery = semievery;
            rg.Day1 = day1;
            rg.Day2 = day2;
            rg.EveryN = everyn;
            rg.Period = period;
            rg.StartWhen = startwhen;
            rg.StopWhen = stopwhen;
            rg.Type = type;
            rg.MaskedAccount = Util.MaskAccount(account);
            rg.MaskedCard = Util.MaskCC(cardnumber);
            rg.Ccv = cardcode;
            rg.Expires = expires;
            rg.Testing = testing;
            rg.NextDate = rg.FindNextDate(startwhen.Value);
            Db.SubmitChanges();
        }
        public string deleteCustomerProfile(int custid)
        {
            var request = new XDocument(new XDeclaration("1.0", "utf-8", null),
                Element("deleteCustomerProfileRequest",
                    Element("merchantAuthentication",
                        Element("name", login),
                        Element("transactionKey", key)
                        ),
                    Element("customerProfileId", custid)
                )
            );
            var x = getResponse(request.ToString());
            return x.ToString();
        }
        public string getCustomerProfileIds()
        {
            var request = new XDocument(new XDeclaration("1.0", "utf-8", null),
                Element("getCustomerProfileIdsRequest",
                    Element("merchantAuthentication",
                        Element("name", login),
                        Element("transactionKey", key)
                        )
                    )
                );
            var x = getResponse(request.ToString());
            return x.ToString();
        }
        public XDocument getCustomerPaymentProfile(int PeopleId)
        {
            var rg = Db.RecurringGivings.Single(pp => pp.PeopleId == PeopleId);
            var request = new XDocument(new XDeclaration("1.0", "utf-8", null),
                Element("getCustomerPaymentProfileRequest",
                    Element("merchantAuthentication",
                        Element("name", login),
                        Element("transactionKey", key)
                        ),
                    Element("customerProfileId", rg.AuNetCustId),
                    Element("customerPaymentProfileId", rg.AuNetCustPayId)
                )
            );
            var x = getResponse(request.ToString());
            return x;
        }

        public string getCustomerProfile(int PeopleId)
        {
            var au = Db.RecurringGivings.Single(pp => pp.PeopleId == PeopleId);
            var request = new XDocument(new XDeclaration("1.0", "utf-8", null),
                Element("getCustomerProfileRequest",
                    Element("merchantAuthentication",
                        Element("name", login),
                        Element("transactionKey", key)
                        ),
                    Element("customerProfileId", au.AuNetCustId)
                )
            );
            var x = getResponse(request.ToString());
            return x.ToString();
        }
        public TransactionResponse createCustomerProfileTransactionRequest(int PeopleId, decimal amt, string description, int tranid, string cardcode)
        {
            var au = Db.RecurringGivings.Single(pp => pp.PeopleId == PeopleId);
            var request = new XDocument(new XDeclaration("1.0", "utf-8", null),
            Element("createTransactionRequest",
                Element("merchantAuthentication",
                    Element("name", login),
                    Element("transactionKey", key)
                    ),
                Element("profileTransAuthCapture",
                    Element("amount", amt),
                    Element("customerProfileId", au.AuNetCustId),
                    Element("customerPaymentProfileId", au.AuNetCustPayId),
                    Element("order",
                        Element("invoiceNumber", tranid),
                        Element("description", description)
                        ),
                    Element("cardCode", cardcode)
                    )
                )
            );
            var x = getResponse(request.ToString());

            var resp = x.Descendants(ns + "directResponse").First().Value;
            var a = resp.Split('|');
            var tr = new TransactionResponse
            {
                Approved = a[0] == "1",
                Message = a[3],
                AuthCode = a[4],
                TransactionId = a[6]
            };
            return tr;
        }
        private XElement Element(string name, params object[] content)
        {
            return new XElement(ns + name, content);
        }
        public TransactionResponse createTransactionRequest(int PeopleId, decimal amt, string cardnumber, string expires, string description, int tranid, string cardcode)
        {
            var p = Db.LoadPersonById(PeopleId);
            var au = p.RecurringGivings.First();
            var request = new XDocument(new XDeclaration("1.0", "utf-8", null),
            Element("createTransactionRequest",
                Element("merchantAuthentication",
                    Element("name", login),
                    Element("transactionKey", key)
                    ),
                Element("transactionRequest",
                    Element("transactionType", "authCaptureTransaction"), // or refundTransaction or voidTransaction
                    Element("amount", amt),
                    Element("payment",
                        Element("creditCard",
                            Element("cardNumber", cardnumber),
                            Element("expirationDate", expires),
                            Element("cardCode", cardcode)
                            )
                        ),
                    Element("order",
                        Element("invoiceNumber", tranid),
                        Element("description", description)
                        ),
                    Element("customer",
                        Element("id", PeopleId),
                        Element("email", p.EmailAddress)
                        ),
                    Element("billTo",
                        Element("firstName", p.FirstName),
                        Element("lastName", p.LastName),
                        Element("address", p.PrimaryAddress),
                        Element("city", p.PrimaryCity),
                        Element("state", p.PrimaryState),
                        Element("zip", p.PrimaryZip),
                        Element("phoneNumber", p.HomePhone)
                        ),
                    Element("customerIP", Util.GetIPAddress())
                    )
                )
            );

            var x = getResponse(request.ToString());
            var resp = x.Descendants(ns + "transactionResponse").First();
            var tr = new TransactionResponse
            {
                Approved = resp.Element(ns + "responseCode").Value == "1",
                AuthCode = resp.Element(ns + "authCode").Value,
                Message = resp.Descendants(ns + "message").First().Element(ns + "description").Value,
                TransactionId = resp.Element(ns + "transId").Value
            };
            return tr;
        }

        public void Dispose()
        {
        }
    }
}