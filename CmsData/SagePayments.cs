using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace CmsData
{
    public class SagePayments
    {
        string login;
        string key;
        CMSDataContext Db;

        public SagePayments(CMSDataContext Db, bool testing)
        {
#if DEBUG
            testing = true;
#endif
            this.Db = Db;
            if (testing)
            {
                login = "287793447481";
                key = "S4S3N4D2W1D9";
            }
            else
            {
                login = Db.Setting("M_id", "");
                key = Db.Setting("M_key", "");
            }
        }
        private XElement getResponse(System.Data.DataSet ds)
        {
            var xml = ds.GetXml();
            var x = XDocument.Parse(xml);
            var t = x.Root.Elements().First();
            var success = t.Element("SUCCESS");
            if (success != null && success.Value.ToLower() == "false")
            {
                var message = t.Element("MESSAGE").Value;
                throw new Exception(message);
            }
            return t;
        }
        public void storeVault(int PeopleId,
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
            var w = new SageVaultAPI.wsVaultSoapClient("wsVaultSoap");
            XElement resp = null;
            if (type == "C")
            {
                if (rg.SageCardGuid == null)
                {
                    var ret = w.INSERT_CREDIT_CARD_DATA(login, key, cardnumber, expires);
                    resp = getResponse(ret);
                    rg.SageCardGuid = Guid.Parse(resp.Element("GUID").Value);
                }
                else
                {
                    if (!cardnumber.StartsWith("X"))
                    {
                        var ret = w.UPDATE_CREDIT_CARD_DATA(login, key,
                            rg.SageCardGuid.ToString().Replace("-", ""), cardnumber, expires);
                        resp = getResponse(ret);
                    }
                    else
                    {
                        var ret = w.UPDATE_CREDIT_CARD_EXPIRATION_DATE(login, key,
                            rg.SageCardGuid.ToString().Replace("-", ""), expires);
                        resp = getResponse(ret);
                    }
                }
            }
            else
            {
                if (rg.SageBankGuid == null)
                {
                    var ret = w.INSERT_VIRTUAL_CHECK_DATA(login, key, routing, account, "DDA"); //SAV
                    resp = getResponse(ret);
                    rg.SageBankGuid = Guid.Parse(resp.Element("GUID").Value);
                }
                else
                {
                    if (!account.StartsWith("X"))
                    {
                        var ret = w.UPDATE_VIRTUAL_CHECK_DATA(login, key,
                            rg.SageBankGuid.ToString().Replace("-", ""), routing, account, "DDA");
                        resp = getResponse(ret);
                    }
                }
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
        public void deleteVaultData(int PeopleId)
        {
            var p = Db.LoadPersonById(PeopleId);
            var rg = p.RecurringGivings.First();
            var w = new SageVaultAPI.wsVaultSoapClient("wsVaultSoap");
            if (rg.SageCardGuid.HasValue)
                w.DELETE_DATA(login, key, rg.SageCardGuid.ToString().Replace("-", ""));
            if (rg.SageBankGuid.HasValue)
                w.DELETE_DATA(login, key, rg.SageBankGuid.ToString().Replace("-", ""));
            rg.SageCardGuid = null;
            rg.SageBankGuid = null;
            rg.MaskedCard = null;
            rg.MaskedAccount = null;
            rg.Ccv = null;
            Db.SubmitChanges();
        }

        public TransactionResponse createTransactionRequest(int PeopleId, decimal amt, string cardnumber, string expires, string description, int tranid, string cardcode)
        {
            var p = Db.LoadPersonById(PeopleId);

            var w = new SagePaymentsAPI.TRANSACTION_PROCESSINGSoapClient("TRANSACTION_PROCESSINGSoap");
            var ret = w.BANKCARD_SALE(login, key,
                p.Name,
                p.PrimaryAddress, p.PrimaryCity, p.PrimaryState, p.PrimaryZip, "",
                p.EmailAddress,
                cardnumber, expires, cardcode,
                PeopleId.ToString(),
                amt.ToString(), "", "",
                tranid.ToString(),
                p.HomePhone,
                "", "", "", "", "", "", "");
            var resp = getResponse(ret);
            var tr = new TransactionResponse
            {
                Approved = resp.Element("APPROVAL_INDICATOR").Value == "A",
                AuthCode = resp.Element("CODE").Value,
                Message = resp.Element("MESSAGE").Value,
                TransactionId = resp.Element("REFERENCE").Value
            };
            return tr;
        }
        public TransactionResponse createVaultTransactionRequest(int PeopleId, decimal amt, string description, int tranid)
        {
            var p = Db.LoadPersonById(PeopleId);
            var rg = p.RecurringGivings.First();

            XElement resp = null;
            if (rg.Type == "C")
            {
                var w = new SageVaultBankCardAPI.wsVaultBankcardSoapClient("wsVaultBankcardSoap");
                var ret = w.VAULT_BANKCARD_SALE_CVV(login, key,
                    p.Name,
                    p.PrimaryAddress, p.PrimaryCity, p.PrimaryState, p.PrimaryZip, "",
                    p.EmailAddress,
                    rg.SageCardGuid.ToString().Replace("-", ""),
                    PeopleId.ToString(),
                    amt.ToString(), "", "",
                    tranid.ToString(),
                    p.HomePhone,
                    "", "", "", "", "", "", "",
                    rg.Ccv);

                resp = getResponse(ret);
            }
            else
            {
                var w = new SageVaultCheckAPI.wsVaultVirtualCheckSoapClient("wsVaultVirtualCheckSoap");
                var mi = (p.MiddleName ?? " ").FirstOrDefault().ToString().Trim();
                var ret = w.VIRTUAL_CHECK_WEB_SALE(login, key,
                    "originator_id",
                    p.FirstName,
                    mi,
                    p.LastName,
                    p.SuffixCode,
                    p.PrimaryAddress,
                    p.PrimaryCity,
                    p.PrimaryState,
                    p.PrimaryZip,
                    "",
                    p.EmailAddress,
                    rg.SageBankGuid.ToString().Replace("-", ""),
                    amt.ToString("n2"),
                    "", "",
                    tranid.ToString(),
                    p.HomePhone,
                    "", "", "", "", "", "", "", "", "", "",
                    p.DOB);
                resp = getResponse(ret);
            }
            var tr = new TransactionResponse
            {
                Approved = resp.Element("APPROVAL_INDICATOR").Value == "A",
                AuthCode = resp.Element("CODE").Value,
                Message = resp.Element("MESSAGE").Value,
                TransactionId = resp.Element("REFERENCE").Value
            };
            return tr;
        }
    }
}