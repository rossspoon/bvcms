/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using UtilityExtensions;
using CMSPresenter;

public partial class QuickSearchParameters : System.Web.UI.UserControl, SearchParameters
{
    public event EventHandler SearchButtonClicked;
    public event EventHandler ClearButtonClicked;

    public virtual void OnSearchButtonClicked(EventArgs e)
    {
        if (SearchButtonClicked != null)
            SearchButtonClicked(null, e);
    }
    protected void Page_Load(System.Object sender, EventArgs e)
    {
    }
    protected void SearchButton_Click(object sender, EventArgs e)
    {
        OnSearchButtonClicked(e);
    }
    protected void ClearSearch_Click(object sender, EventArgs e)
    {
        GenderSearch.SelectedIndex = 0;
        MemberSearch.SelectedIndex = 0;
        OrgIdSearch.Text = "";
        AddressSearch.Text = "";
        NameSearch.Text = "";
        TagSearch.SelectedIndex = 0;
        CommunicationSearch.Text = "";
        DOBSearch.Text = "";
        CampusSearch.SelectedIndex = 0;
        OnClearButtonClicked(e);
    }
    public virtual void OnClearButtonClicked(EventArgs e)
    {
        if (ClearButtonClicked != null)
            ClearButtonClicked(null, e);
    }


    #region SearchParameters Members

    public string DOB
    {
        get { return DOBSearch.Text; }
        set { DOBSearch.Text = value; }
    }

    public string Comm
    {
        get { return CommunicationSearch.Text; }
        set { CommunicationSearch.Text = value; }
    }

    public int OrgId
    {
        get { return OrgIdSearch.Text.ToInt(); }
        set { OrgIdSearch.Text = value.ToString(); }
    }

    public string Name
    {
        get { return NameSearch.Text; }
        set { NameSearch.Text = value; }
    }

    public string Addr
    {
        get { return AddressSearch.Text; }
        set { AddressSearch.Text = value; }
    }

    public int Gender
    {
        get
        {
            if (GenderSearch.SelectedValue == "")
                return 99;
            return GenderSearch.SelectedValue.ToInt();
        }
        set { GenderSearch.SelectedValue = value.ToString(); }
    }

    public int Member
    {
        get { return MemberSearch.SelectedValue.ToInt(); }
        set { MemberSearch.SelectedValue = value.ToString(); }
    }
    public int Campus
    {
        get { return CampusSearch.SelectedValue.ToInt(); }
        set { CampusSearch.SelectedValue = value.ToString(); }
    }

    public int Tag
    {
        get { return TagSearch.SelectedValue.ToInt(); }
        set
        {
            var item = TagSearch.Items.FindByValue(value.ToString());
            if (item != null)
                item.Selected = true;
        }
    }

    #endregion
}
