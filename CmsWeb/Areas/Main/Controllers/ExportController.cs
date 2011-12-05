using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Models;
using CmsData;
using UtilityExtensions;
using System.Linq.Dynamic;

namespace CmsWeb.Areas.Main.Controllers
{
    public class ExportController : Controller
    {
        public ActionResult UpdatePeople(int id)
        {
            return new UpdatePeopleModel(id);
        }
        public ActionResult MucketyMap(int id)
        {
            return new MucketyMapResult(id);
        }
        public ActionResult QueryBits(int id)
        {
            return new QueryBitsExcelResult(id);
        }
        public ActionResult ExtraValues(int id)
        {
            return new ExtraValueExcelResult(id);
        }
        [Authorize(Roles="Admin")]
        public ActionResult FreshBooks(int id)
        {
            var q = from p in DbUtil.Db.PeopleQuery(id)
                    let p2 = p.Family.People.FirstOrDefault(pp => pp.PositionInFamilyId == 10 && pp.PeopleId != p.PeopleId)
                    let name = p.GetExtra("name")
                    let web = p.GetExtra("web")
                    select new 
                    { 
                        Organization = name != "name" ? name : p.LastName,
                        FirstName = p2.PreferredName,
                        LastName = p2.LastName,
                        Email = p2.EmailAddress,
                        Street = p.PrimaryAddress,
                        Street2 = p.PrimaryAddress2,
                        City = p.PrimaryCity,
                        Province = p.PrimaryState,
                        Country = "USA",
                        PostalCode = p.PrimaryZip,
                        BusPhone = p.HomePhone.FmtFone(),
                        MobPhone = p2.CellPhone.FmtFone(),
                        Fax = "",
                        SecStreet = p.AddressLineOne,
                        SecStreet2 = p.AddressLineTwo,
                        SecCity = p.CityName,
                        SecProvince = p.StateCode,
                        SecCountry = "USA",
                        SecPostalCode = p.ZipCode,
                        Notes =   "DbName: " + p.LastName + "\n"
                                + "InvoiceDt: " + p.BirthDate.FormatDate() + "\n"
                                + "Web: " + web
                        };
            return new DataGridResult(q);
        }
        [Authorize(Roles="Finance")]
        [Authorize(Roles="Admin")]
        public ActionResult Contributions(int id, string start, string end, bool? totals)
        {
            return new ContributionsExcelResult(id, start, end, totals ?? false);
        }
    }
}
