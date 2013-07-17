using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using CmsWeb.Areas.People.Models;
using UtilityExtensions;
using System.Web.Routing;

namespace CmsWeb.Areas.People.Controllers
{
    [ValidateInput(false)]
    [SessionExpire]
    [RouteArea("People", AreaUrl = "Address")]
    public class AddressController : CmsStaffController
    {
        protected override void Initialize(RequestContext requestContext)
        {
            NoCheckRole = true;
            base.Initialize(requestContext);
        }

        [POST("Address/Edit/{type}/{id}")]
        public ActionResult Edit(int id, string type)
        {
            var m = AddressInfo.GetAddressInfo(id, type);
            return View(m);
        }

        [POST("Address/EditAgain")]
        public ActionResult EditAgain(AddressInfo m)
        {
            return View("Edit", m);
        }

        [POST("Address/Update/{noCheck?}")]
        public ActionResult Update(AddressInfo m, string noCheck)
        {
            if (noCheck.HasValue() == false)
                m.ValidateAddress(ModelState);
            if (!ModelState.IsValid)
                return View("Edit", m);
            if (m.Error.HasValue())
            {
                ModelState.Clear();
                return View("Edit", m);
            }
            m.UpdateAddress(ModelState);
            return View("Saved", m);
        }

        [POST("Address/ForceSave")]
        public ActionResult ForceSave(AddressInfo m)
        {
            m.UpdateAddress(ModelState, forceSave: true);
            return View("Saved", m);
        }

    }
}
