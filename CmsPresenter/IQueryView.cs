/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using CmsData;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using UtilityExtensions;
using System.Linq.Expressions;
using CmsData.View;

namespace CMSPresenter
{
    public interface IQueryView
    {
        bool ResultsVisible { get; set; }
        bool RightPanelVisible { set; }
        int SelectedId { get; set; }
        string FieldText { get; set; }
        bool TextVisible { set; }
        bool DateVisible { set; }
        bool NumberVisible { set; }
        bool IntegerVisible { set; }
        bool StartDateVisible { set; }
        bool EndDateVisible { set; }
        bool CodeVisible { set; }
        bool SelectMultiple { get; set; }
        string DataTextField { set; }
        string DataValueField { get; set; }
        object CodeDataSource { set; }
        object CheckCodesDataSource { set; }
        object CompareDataSource { set; }
        string ComparisonText { get; set; }
        string CodeValue { get; set; }
        string TextValue { get; set; }
        int? QueryId { get; set; }
        DateTime? DateValue { get; set; }
        DateTime? StartDateValue { get; set; }
        DateTime? EndDateValue { get; set; }
        int? DivOrgValue { get; set; }
        int? SubDivOrgValue { get; set; }
        int? OrganizationValue { get; set; }
        int DaysValue { get; set; }
        string SavedQueryIdDesc { get; set; }
        string QuartersValue { get; set; }
        string TagsValue { get; set; }
        bool DivOrgVisible { set; }
        bool SubDivOrgVisible { set; }
        bool OrganizationVisible { set; }
        bool DaysVisible { set; }
        bool WeekVisible { get; set; }
        bool QuartersVisible { set; }
        bool TagsVisible { set; }
        bool SavedQueryVisible { set; }
        bool UpdateButtonEnabled { set; }
        bool AddToGroupEnabled { set; }
        bool AddEnabled { set; }
        bool IsPublic { get; set; }
        string Description { get; set; }
    }
}
