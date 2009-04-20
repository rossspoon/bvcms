/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web.UI.WebControls;
using CMSPresenter;
using UtilityExtensions;
using System.Linq;
using System.Collections;
using CmsData;

namespace CMSWeb
{
    public partial class QueryBuilder : System.Web.UI.Page, IQueryView
    {
        private CodeValueController CVController = new CodeValueController();

        #region IQueryView Members

        public bool StartDateVisible
        {
            set { StartDiv.Visible = value; }
        }

        public bool EndDateVisible
        {
            set { EndDiv.Visible = value; }
        }

        public bool CodeVisible
        {
            set
            {
                ValueCheckCodes.Visible = false; // always off by default
                ValueCode.Visible = value;
            }
        }

        public bool SelectMultiple
        {
            get { return ValueCheckCodes.Visible; }
            set
            {
                if (ValueCode.Visible || ValueCheckCodes.Visible)
                {
                    ValueCheckCodes.Text = string.Empty;
                    ValueCheckCodes.Visible = value;
                    ValueCode.Visible = !value;
                }
            }
        }

        public string DataTextField
        {
            set
            {
                ValueCode.DataTextField = value;
                ValueCheckCodes.DataTextField = value;
            }
        }

        public string DataValueField
        {
            get { return ValueCode.DataValueField; }
            set
            {
                ValueCode.DataValueField = value;
                ValueCheckCodes.DataValueField = value;
            }
        }

        public object CodeDataSource
        {
            set { ValueCode.DataSource = value; ValueCode.DataBind(); }
        }

        public object CheckCodesDataSource
        {
            set { ValueCheckCodes.DataSource = value; ValueCheckCodes.DataBind(); }
        }

        public object CompareDataSource
        {
            set
            {
                Comparison.SelectedIndex = -1;
                Comparison.DataSource = value;
                Comparison.DataBind();
            }
        }

        public int? QueryId
        {
            get
            {
                if (Session["QueryId"].IsNotNull())
                    return (int)Session["QueryId"];
                return null;
            }
            set
            {
                if (value.HasValue)
                    Session["QueryId"] = value.Value;
                else
                    Session.Remove("QueryId");
            }
        }

        public bool DivOrgVisible
        {
            set
            {
                ProgDiv.Visible = value;
                if (value)
                {
                    DivOrg.Items.Clear();
                    DivOrg.Items.Add(new ListItem("(not specified)", "0"));
                    DivOrg.SelectedIndex = -1;
                    DivOrg.DataSource = CVController.OrgDivTags();
                    DivOrg.DataBind();
                }
            }
        }

        public bool SubDivOrgVisible
        {
            set
            {
                DivDiv.Visible = value;
                if (value)
                {
                    SubDivOrg.SelectedIndex = -1;
                    SubDivOrg.DataSource = CVController.OrgSubDivTags(0);
                    SubDivOrg.DataBind();
                }
            }
        }

        public bool OrganizationVisible
        {
            set
            {
                OrgDiv.Visible = value;
                if (value)
                {
                    Organization.SelectedIndex = -1;
                    Organization.DataSource = CVController.Organizations(0);
                    Organization.DataBind();
                }
            }
        }

        public string FieldText
        {
            get { return ConditionName.Value; }
            set
            {
                //Fields.SelectedValue = value;
                var f = FieldClass.Fields[value];
                CurrentCondition.Text = f.Title;
                ConditionName.Value = value;
            }
        }

        public string ComparisonText
        {
            get { return Comparison.SelectedValue; }
            set { Comparison.SelectedValue = value; }
        }

        public string CodeValue
        {
            get
            {
                if (!ValueCode.Visible && !ValueCheckCodes.Visible)
                    return null;
                if (SelectMultiple)
                    return ValueCheckCodes.Text;
                if (ValueCode.SelectedValue.HasValue())
                    return ValueCode.SelectedValue;
                if (ValueCode.SelectedItem.IsNotNull())
                    return ValueCode.SelectedItem.Text;
                return null;
            }
            set
            {
                ValueCheckCodes.Text = string.Empty;
                if (SelectMultiple)
                    ValueCheckCodes.Text = value;
                else
                    ValueCode.SelectedValue = value;
            }
        }

        public DateTime? StartDateValue
        {
            get { return StartDate.Visible ? StartDate.Text.ToDate() : null; }
            set
            {
                StartDate.Text = "";
                if (value.HasValue)
                    StartDate.Text = value.Value.ToShortDateString();
            }
        }

        public DateTime? EndDateValue
        {
            get { return EndDate.Visible ? EndDate.Text.ToDate() : null; }
            set
            {
                EndDate.Text = "";
                if (value.HasValue)
                    EndDate.Text = value.Value.ToShortDateString();
            }
        }

        public int? DivOrgValue
        {
            get { return DivOrg.Visible ? DivOrg.SelectedValue.ToInt2() : 0; }
            set
            {
                if (value.HasValue)
                    DivOrg.SelectedValue = value.ToString();
                else
                    DivOrg.SelectedIndex = -1;
                ReBindDivOrg();
            }
        }

        public int? SubDivOrgValue
        {
            get { return SubDivOrg.Visible ? SubDivOrg.SelectedValue.ToInt2() : 0; }
            set
            {
                if (value.HasValue)
                    SubDivOrg.SelectedValue = value.ToString();
                else
                    SubDivOrg.SelectedIndex = -1;
                ReBindOrg();
            }
        }

        public int? OrganizationValue
        {
            get { return Organization.Visible ? Organization.SelectedValue.ToInt2() : 0; }
            set
            {
                if (value.HasValue)
                    Organization.SelectedValue = value.ToString();
                else
                    Organization.SelectedIndex = -1;
            }
        }

        public bool RightPanelVisible
        {
            set { RightPanel.Visible = value; }
        }
        public int SelectedId
        {
            get { return SavedSelectedId.Value.ToInt(); }
            set { SavedSelectedId.Value = value.ToString(); }
        }
        public bool AddToGroupEnabled
        {
            set { AddToGroup.Enabled = value; }
        }

        public int DaysValue
        {
            get { return Days.Text.ToInt(); }
            set { Days.Text = value.ToString(); }
        }

        public bool DaysVisible
        {
            set { DaysDiv.Visible = value; }
        }
        public bool UpdateButtonEnabled
        {
            set { Update.Enabled = value; }
        }
        public bool AddEnabled
        {
            set
            {
                Add.Enabled = value;
                Delete.Enabled = value;
            }
        }

        public bool IsPublic
        {
            get { return PublicCheck.Checked; }
            set { PublicCheck.Checked = value; }
        }

        public string Description
        {
            get { return QueryDescription.Text; }
            set
            {
                QueryDescription.Text = value;
                SavedQueryName.Text = value;
            }
        }


        public string QuartersValue
        {
            get
            {
                if (WeekVisible)
                    return Week.Text;
                return Quarters.Text;
            }
            set
            {
                Week.Text = value;
                Quarters.Text = value;
            }
        }

        public bool QuartersVisible
        {
            set { QuartersDiv.Visible = value; }
        }

        public string SavedQueryIdDesc
        {
            get
            {
                if (SavedQueryDesc.SelectedItem != null)
                    return SavedQueryDesc.SelectedItem.Text;
                return "";
            }
            set
            {
                SavedQueryDesc.SelectedIndex = -1;
                if (value.HasValue())
                {
                    var i = SavedQueryDesc.Items.FindByText(value);
                    if (i != null)
                        i.Selected = true;
                }
            }
        }

        public bool SavedQueryVisible
        {
            set
            {
                SavedQueryDiv.Visible = value;
                if (value)
                {
                    SavedQueryDesc.SelectedIndex = -1;
                    SavedQueryDesc.DataSource = CVController.UserQueries();
                    SavedQueryDesc.DataBind();
                }
            }
        }

        public string TagsValue
        {
            get
            {
                if (!TagDiv.Visible)
                    return null;
                return Tags.Text;
            }
            set
            {
                Tags.Text = value;
            }
        }

        public bool TagsVisible
        {
            set
            {
                TagDiv.Visible = value;
                if (value)
                {
                    Tags.DataSource = CVController.UserTags(Util.UserPeopleId);
                    Tags.DataBind();
                }
            }
        }

        public bool ResultsVisible
        {
            get { return Results.Visible; }
            set { Results.Visible = value; }
        }

        public DateTime? DateValue
        {
            get { return ValueDate.Visible ? ValueDate.Text.ToDate() : null; }
            set
            {
                ValueDate.Text = "";
                if (value.HasValue)
                    ValueDate.Text = value.Value.ToShortDateString();
            }
        }

        public string TextValue
        {
            get
            {
                if (ValueText.Visible && ValueText.Text.HasValue())
                    return ValueText.Text;
                else
                    return null;
            }
            set { ValueText.Text = value; }
        }

        public bool DateVisible
        {
            set
            {
                ValueDate.Visible = value;
            }
        }

        public bool TextVisible
        {
            set
            {
                ValueText.Visible = value;
                if (value)
                    HiddenTextType.Value = "t";
            }
        }

        public bool NumberVisible
        {
            set
            {
                ValueText.Visible = value;
                if (value)
                    HiddenTextType.Value = "n";
            }
        }

        public bool IntegerVisible
        {
            set
            {
                ValueText.Visible = value;
                if (value)
                    HiddenTextType.Value = "i";
            }
        }

        public bool WeekVisible
        {
            get { return WeekDiv.Visible; }
            set { WeekDiv.Visible = value; }
        }

        #endregion
    }
}
