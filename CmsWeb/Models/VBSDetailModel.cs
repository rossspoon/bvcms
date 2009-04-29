/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using System.Data.Linq;
using UtilityExtensions;
using CmsData;
using System.Web.Mvc;
using System.Collections.Generic;

namespace CMSWeb.Models
{
    public interface IVBSDetailBindable
    {
        string Request { get; set; }
        bool PubPhoto { get; set; }
        bool ActiveInAnotherChurch { get; set; }
        string GradeCompleted { get; set; }
        bool MedAllergy { get; set; }
    }
    public class VBSDetailModel : IVBSDetailBindable
    {
        private VBSApp VBSApp;
        public VBSDetailModel(int id)
        {
            VBSApp = DbUtil.Db.VBSApps.SingleOrDefault(v => v.Id == id);
        }
        public int Id
        {
            get { return VBSApp.Id; }
        }
        public string Name
        {
            get
            {
                if (VBSApp.Person != null)
                    return VBSApp.Person.Name;
                return "not assigned";
            }
        }
        public int? PeopleId
        {
            get { return VBSApp.PeopleId; }
        }
        public string Request
        {
            get { return VBSApp.Request; }
            set { VBSApp.Request = value; }
        }
        public bool PubPhoto
        {
            get { return VBSApp.PubPhoto ?? false; }
            set { VBSApp.PubPhoto = value; }
        }
        public bool ActiveInAnotherChurch
        {
            get { return VBSApp.ActiveInAnotherChurch ?? false; }
            set { VBSApp.ActiveInAnotherChurch = value; }
        }
        public string GradeCompleted
        {
            get { return VBSApp.GradeCompleted; }
            set { VBSApp.GradeCompleted = value; }
        }
        public bool MedAllergy
        {
            get { return VBSApp.MedAllergy ?? false; }
            set { VBSApp.MedAllergy = value; }
        }
        public bool IsDocument
        {
            get { return VBSApp.IsDocument ?? false; }
        }
        public int ImgId
        {
            get { return VBSApp.ImgId ?? 0; }
        }
        internal void AssignPerson(int pid)
        {
            if (PeopleId != pid)
            {
                if (PeopleId.HasValue && VBSApp.OrgId.HasValue)
                {
                    var q = from om in DbUtil.Db.OrganizationMembers
                            where om.PeopleId == PeopleId && om.OrganizationId == VBSApp.OrgId
                            select om;
                    var member = q.SingleOrDefault();
                    if (member != null)
                        member.Drop();
                }
                VBSApp.PeopleId = pid;
                DbUtil.Db.SubmitChanges();
            }
        }
        public IEnumerable<SelectListItem> GradeCompleteds()
        {
			var sa = new []
			{ 
				new { Value="", Text="(not specified yet)" }, 
				new { Value="Pre-K", Text="Pre-K (4 before Oct 1 last year)" }, 
				new { Value="K-5", Text="K-5 (5 before Oct 1 last year)" }, 
				new { Value="1st", Text="1st grade completed this May" }, 
				new { Value="2nd", Text="2nd grade completed this May" }, 
				new { Value="3rd", Text="3rd grade completed this May" }, 
				new { Value="4th", Text="4th grade completed this May" }, 
				new { Value="5th", Text="5th grade completed this May" }, 
				new { Value="Exceptional", Text="Exceptional" }, 
				new { Value="Other", Text="Other" }, 
			};
			return from g in sa
				   select new SelectListItem
				   {
					   Text = g.Text,
					   Value = g.Value,
				   };
        }
    }
}
