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
    public class RecDetailModel
    {
        private RecReg recreg;
        public RecDetailModel(int id)
        {
            recreg = DbUtil.Db.RecRegs.SingleOrDefault(v => v.Id == id);
        }
        public int Id
        {
            get { return recreg.Id; }
        }
        public string Name
        {
            get
            {
                if (recreg.Person != null)
                    return recreg.Person.Name;
                return "not assigned";
            }
        }
        public int? PeopleId
        {
            get { return recreg.PeopleId; }
        }
        public string ShirtSize
        {
            get { return recreg.ShirtSize; }
            set { recreg.ShirtSize = value; }
        }
        public bool FeePaid
        {
            get { return recreg.FeePaid ?? false; }
            set { recreg.FeePaid = value; }
        }
        public string Request
        {
            get { return recreg.Request; }
            set { recreg.Request = value; }
        }
        public bool ActiveInAnotherChurch
        {
            get { return recreg.ActiveInAnotherChurch ?? false; }
            set { recreg.ActiveInAnotherChurch = value; }
        }
        public bool MedAllergy
        {
            get { return recreg.MedAllergy ?? false; }
            set { recreg.MedAllergy = value; }
        }
        public bool IsDocument
        {
            get { return recreg.IsDocument ?? false; }
        }
        public int ImgId
        {
            get { return recreg.ImgId ?? 0; }
        }
    }
}
