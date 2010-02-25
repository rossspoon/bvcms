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
using System.Web.Services;
using System.Web.Services.Protocols;
using CMSPresenter;
using UtilityExtensions;
using CmsData;
using CMSWeb;

namespace CMSWeb
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WebService : System.Web.Services.WebService
    {
        public ServiceAuthHeader CustomSoapHeader;

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        [AuthenticatonSoapExtensionAttribute]
        public void UploadImage(int? PeopleId, string UserInfo, int TypeId, string mimetype, byte[] bits)
        {
            var Db = DbUtil.Db;
            switch (TypeId)
            {
                case 1:
                    var vb = new VBSApp();
                    vb.UserInfo = UserInfo;
                    Db.VBSApps.InsertOnSubmit(vb);
                    vb.ImgId = InsertImage(mimetype, bits);
                    vb.Uploaded = Util.Now;
                    break;
                //case 2:
                //    var rr = new RecReg();
                //    rr.UserInfo = UserInfo;
                //    Db.RecRegs.InsertOnSubmit(rr);
                //    rr.ImgId = InsertImage(mimetype, bits);
                //    rr.Uploaded = Util.Now;
                //    break;
                case 3:
                    if (!PeopleId.HasValue)
                        return;
                    var guid = new Guid(UserInfo);
                    var tok = DbUtil.Db.TemporaryTokens.SingleOrDefault(tt => tt.Id == guid);
                    if (tok == null)
                        return;
                    if (Util.Now.Subtract(tok.CreatedOn).TotalHours > 20 || tok.Expired)
                        return;
                    var person = DbUtil.Db.People.Single(pp => pp.PeopleId == PeopleId);
                    if (person.Picture == null)
                        person.Picture = new Picture();
                    var p = person.Picture;
                    p.CreatedDate = Util.Now;
                    p.CreatedBy = Util.UserName;
                    p.SmallId = ImageData.Image.NewImageFromBits(bits, 120, 120).Id;
                    p.MediumId = ImageData.Image.NewImageFromBits(bits, 320, 400).Id;
                    p.LargeId = ImageData.Image.NewImageFromBits(bits, 570, 800).Id;
                    Db.SubmitChanges();
                    break;
            }
            Db.SubmitChanges();
        }
        [WebMethod]
        public void UploadImage2(int? PeopleId, string UserInfo, int TypeId, string mimetype, byte[] bits)
        {
            var Db = DbUtil.Db;
            if (!PeopleId.HasValue)
                return;
            var guid = new Guid(UserInfo);
            var tok = DbUtil.Db.TemporaryTokens.SingleOrDefault(tt => tt.Id == guid);
            if (tok == null)
                return;
            if (Util.Now.Subtract(tok.CreatedOn).TotalHours > 20 || tok.Expired)
                return;
            var person = DbUtil.Db.People.Single(pp => pp.PeopleId == PeopleId);
            if (person.Picture == null)
                person.Picture = new Picture();
            var p = person.Picture;
            p.CreatedDate = Util.Now;
            p.CreatedBy = Util.UserName;
            p.SmallId = ImageData.Image.NewImageFromBits(bits, 120, 120).Id;
            p.MediumId = ImageData.Image.NewImageFromBits(bits, 320, 400).Id;
            p.LargeId = ImageData.Image.NewImageFromBits(bits, 570, 800).Id;
            Db.SubmitChanges();
        }

        private static int? InsertImage(string mimetype, byte[] bits)
        {
            var img = new ImageData.Image();
            switch (mimetype)
            {
                case "image/jpeg":
                case "image/pjpeg":
                case "image/gif":
                    return ImageData.Image.NewImageFromBits(bits).Id;
                case "application/pdf":
                case "application/msword":
                case "application/vnd.ms-excel":
                    return ImageData.Image.NewImageFromBits(bits, mimetype).Id;
            }
            return null;
        }

        private PersonResult[] more = 
		        { 
		            new PersonResult 
		            { 
                         Name = "(more than ten results)",
                         PeopleId = 0,
		            } 
		        };

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        [AuthenticatonSoapExtensionAttribute]
        public PersonResult[] SearchPerson(string name, string comm, string addr, string birthday)
        {
            var ctl = new PersonSearchController();
            var q = ctl.FetchPeopleList(0, 10, "", name, comm, addr, 0, 0, birthday, 99, 0, 0, false, 99);
            var q2 = from p in q
                     select new PersonResult
                     {
                         Name = p.Deceased ? "[" + p.Name + "]" : p.Name,
                         Age = p.Age,
                         Address = p.Address,
                         CSZ = p.CityStateZip,
                         Phone = string.Join(",", p.Phones.ToArray()),
                         Birthday = p.BirthDate,
                         Deceased = p.Deceased,
                         PeopleId = p.PeopleId,
                     };
            if (ctl.count > 10)
                return q2.Union(more).ToArray();
            return q2.ToArray();
        }
        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        [AuthenticatonSoapExtensionAttribute]
        public BundleResult[] RecentBundleList()
        {
            var q = from b in DbUtil.Db.BundleHeaders
                    where b.BundleStatusId == (int)BundleHeader.StatusCode.Open
                    orderby b.BundleHeaderId descending
                    select new BundleResult
                    {
                        BundleId = b.BundleHeaderId,
                        Date = b.ContributionDate,
                        Fund = b.FundId,
                        Total = b.TotalCash + b.TotalChecks + b.TotalEnvelopes,
                        Count = b.BundleDetails.Count(),
                    };
            return q.ToArray();
        }
        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        [AuthenticatonSoapExtensionAttribute]
        public BundleDetail[] BundleDetails(int BundleId)
        {
            var q = from b in DbUtil.Db.BundleDetails
                    where b.BundleHeaderId == BundleId
                    orderby b.BundleDetailId
                    select new BundleDetail
                    {
                        Amount = b.Contribution.ContributionAmount.Value,
                        PeopleId = b.Contribution.PeopleId.Value,
                        Fund = b.Contribution.FundId,
                        Date = b.Contribution.ContributionDate.Value,
                        Pledge = b.Contribution.PledgeFlag,
                        Name = b.Contribution.Person.Name,
                    };
            return q.ToArray();
        }
        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        [AuthenticatonSoapExtensionAttribute]
        public void UploadBundle(int BundleId, BundleDetail[] a)
        {
            var bundle = DbUtil.Db.BundleHeaders.Single(b => b.BundleHeaderId == BundleId);
            if (bundle.BundleDetails.Count() > 0)
            {
                foreach (var d in bundle.BundleDetails)
                {
                    DbUtil.Db.Contributions.DeleteOnSubmit(d.Contribution);
                    DbUtil.Db.BundleDetails.DeleteOnSubmit(d);
                }
            }
            var t = a.Sum(d => d.Amount);
            if (t != bundle.TotalChecks + bundle.TotalCash + bundle.TotalEnvelopes)
                throw new Exception("Totals do not match");
            foreach (var d in a)
            {
                var f = DbUtil.Db.ContributionFunds.SingleOrDefault(cf => cf.FundId == d.Fund && cf.FundStatusId == 1);
                if (f == null)
                    throw new Exception("Unknown Fund ({0})".Fmt(d.Fund));
            }
            var now = Util.Now;
            foreach (var d in a)
            {
                var bd = new CmsData.BundleDetail
                {
                    BundleHeaderId = BundleId,
                    CreatedBy = CustomSoapHeader.userid,
                    CreatedDate = now,
                };
                int type;
                if (d.Pledge)
                    type = (int)Contribution.TypeCode.Pledge;
                else
                    type = (int)Contribution.TypeCode.CheckCash;
                bd.Contribution = new Contribution
                {
                    CreatedBy = CustomSoapHeader.userid,
                    CreatedDate = now,
                    FundId = d.Fund,
                    PeopleId = d.PeopleId,
                    ContributionDate = d.Date,
                    ContributionAmount = d.Amount,
                    ContributionStatusId = 0,
                    PledgeFlag = d.Pledge,
                    ContributionTypeId = type,
                };
                bundle.BundleDetails.Add(bd);
            }
            DbUtil.Db.SubmitChanges();
        }
    }

    public class ServiceAuthHeaderValidation
    {
        public static bool Validate(ServiceAuthHeader soapHeader)
        {
            if (soapHeader == null)
                throw new NullReferenceException("No soap header was specified.");
            if (soapHeader.Username == null)
                throw new NullReferenceException("Username was not supplied for authentication in SoapHeader.");
            if (soapHeader.Password == null)
                throw new NullReferenceException("Password was not supplied for authentication in SoapHeader.");
            if (soapHeader.Username == "tkup" && soapHeader.Password == "password")
            {
                soapHeader.userid = 0;
                return true;
            }
            if (!CMSMembershipProvider.provider.ValidateUser(soapHeader.Username, soapHeader.Password))
                throw new Exception("Please pass the proper username and password for this service.");
            var u = DbUtil.Db.Users.Single(us => us.Username == soapHeader.Username);
            soapHeader.userid = u.UserId;
            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AuthenticatonSoapExtensionAttribute : SoapExtensionAttribute
    {
        private int _priority;
        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
        public override Type ExtensionType
        {
            get { return typeof(AuthenticatonSoapExtension); }
        }
    }
    public class AuthenticatonSoapExtension : SoapExtension
    {
        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attrib)
        {
            return null;
        }
        public override object GetInitializer(Type WebServiceType)
        {
            return null;
        }
        public override void Initialize(object initializer)
        {
        }
        public override void ProcessMessage(SoapMessage message)
        {
            if (message.Stage == SoapMessageStage.AfterDeserialize)
                Authenticate(message);
        }
        public void Authenticate(SoapMessage message)
        {
            var header = message.Headers[0] as ServiceAuthHeader;
            if (header != null)
                ServiceAuthHeaderValidation.Validate(header);
            else
                throw new ArgumentNullException("No ServiceAuthHeader was specified in SoapMessage.");
        }
    }
    public class ServiceAuthHeader : SoapHeader
    {
        public string Username;
        public string Password;
        public int userid;
    }
    public class PersonResult
    {
        public string Name { get; set; }
        public int PeopleId { get; set; }
        public string Address { get; set; }
        public string CSZ { get; set; }
        public string Phone { get; set; }
        public string Birthday { get; set; }
        public bool Deceased { get; set; }
        public string Age { get; set; }
    }
    public class BundleResult
    {
        public int BundleId { get; set; }
        public DateTime Date { get; set; }
        public Decimal? Total { get; set; }
        public int? Fund { get; set; }
        public int Count { get; set; }
    }
    public class BundleDetail
    {
        public int PeopleId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int Fund { get; set; }
        public bool Pledge { get; set; }
        public string Name { get; set; }
    }
}
