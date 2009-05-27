/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using CMSPresenter;
using UtilityExtensions;
using System.Collections.Generic;
using System.Web.SessionState;

namespace CMSWeb
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class bulkmail : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            var r = context.Response;
            r.ContentType = "text/plain";
            r.AddHeader("Content-Disposition", "attachment;filename=CMSPeople.csv");
            r.Charset = "";
            int? qid = context.Request.QueryString["id"].ToInt2();
            if (!qid.HasValue)
            {
                r.Write("no queryid");
                r.Flush();
                r.End();
            }
            var labelNameFormat = context.Request.QueryString["format"];
            var ctl = new MailingController();
            var useTitles = context.Request.QueryString["titles"];
            ctl.UseTitles = useTitles == "true";
            IEnumerable<MailingInfo> q = null;
            switch (labelNameFormat)
            {
                case "Individual":
                    q = ctl.FetchIndividualList("Name", qid.Value);
                    break;
                case "Family":
                    q = ctl.FetchFamilyList("Name", qid.Value);
                    break;
                case "ParentsOf":
                    q = ctl.FetchParentsOfList("Name", qid.Value);
                    break;
                case "CouplesEither":
                    q = ctl.FetchCouplesEitherList("Name", qid.Value);
                    break;
                case "CouplesBoth":
                    q = ctl.FetchCouplesBothList("Name", qid.Value);
                    break;
            }

            foreach (var mi in q)
                r.Write(string.Format("{0},{1},{2},{3},{4},{5},{6}\n",
                    mi.LabelName, mi.Address, mi.Address2, mi.City, mi.State, mi.Zip.FmtZip(),mi.PeopleId));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
