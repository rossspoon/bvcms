using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Linq.Dynamic;
using CmsWeb.Areas.Main.Controllers;
using CmsWeb.Areas.Manage.Controllers;
using System.Text;
using System.Data.SqlClient;

namespace CmsWeb.Models
{
    public class FreshBooksResult: ActionResult
    {
        private int qid;
        public FreshBooksResult(int qid)
        {
            this.qid = qid;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;

            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=CMSPeople.xls");
            Response.Charset = "";

            var q = from p in DbUtil.Db.PeopleQuery(qid)
                    let p2 = p.Family.People.FirstOrDefault(pp => pp.PositionInFamilyId == 10 && pp.PeopleId != p.PeopleId)
                    let name = DbUtil.Db.PeopleExtras.SingleOrDefault(ee => ee.Field == "name" && ee.PeopleId == p.PeopleId).Data
                    let web = DbUtil.Db.PeopleExtras.SingleOrDefault(ee => ee.Field == "web" && ee.PeopleId == p.PeopleId).Data

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
                        BusPhone = p.WorkPhone.FmtFone(),
                        MobPhone = p2.CellPhone.FmtFone(),
                        Fax = "",
                        SecStreet = p.AddressLineOne,
                        SecStreet2 = p.AddressLineTwo,
                        SecCity = p.CityName,
                        SecProvince = p.StateCode,
                        SecCountry = "USA",
                        SecPostalCode = p.ZipCode,
                        Notes = "DbName: " + p.LastName + "\nWeb:" + web
                    };
            var dg = new DataGrid();
            dg.DataSource = q;
            dg.DataBind();
            dg.RenderControl(new HtmlTextWriter(Response.Output));
        }
    }
}