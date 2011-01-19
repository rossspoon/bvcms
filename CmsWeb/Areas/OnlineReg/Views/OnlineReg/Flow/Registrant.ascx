<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel>" %>
<%  var p = Model.current;
    if (p.LastItem && !p.Finished())
    { %>
    <tr><td><div class="instruct">New Registrations</div></td></tr>
<%  }
    if (!p.ShowDisplay())
    // This is a header that will show above the last unfinished item
    {
        if (Model.UserSelectsOrganization())
        // show a select organization dropdown
        { %>
    <tr><td><div class="instruct">Make a Selection</div></td></tr>
    <tr><td><div><% Html.RenderPartial("Flow/ChooseClass", Model); %></div></td></tr>
<%      }
        var showfamily = Model.UserPeopleId.HasValue && Model.FamilyMembers().Count() > 0;
        if (showfamily)
        // show a family list cause we are logged in
        { %>
    <tr><td><h4>Select Family Member</h4></td></tr>
    <tr>
        <td>
            <div class="box">
                <% Html.RenderPartial("Flow/FamilyList", Model); %>
            </div>
            <%= Html.ValidationMessage("findf")%>
        </td>
    </tr>
<%      } %>
    <tr><td><h4>Enter new person's information</h4></td></tr>
    <tr>
        <td><h3 id="fillout" class="instruct">Please fill out the form.</h3></td>
    </tr>
<%  }
    // This is the registrant info, finished or not
%>
    <tr>
        <td>
            <div class="box">
<% // This is where we store some non editable meta data about the person
    Html.RenderPartial("Flow/PersonMetaHidden", p);
    if (p.ShowDisplay())
    // This is where we store the already entered info
    {
        Html.RenderPartial("Flow/PersonHidden", p);
        if (p.OtherOK && !p.ManageSubscriptions())
        // need to store the other info too
            Html.RenderPartial("Flow/OtherHidden", p);
    }
    if (!Model.IsCreateAccount() && !Model.ManagingSubscriptions())
    // This is the cancel link
    { %>
                <a href="/OnlineReg/Cancel/<%=p.index %>" class="close submitlink">
                    <img src="/images/delete.gif" border="0" alt="cancel" title="cancel this registration" /></a>
<%  }
    if (p.Finished())
    // This is where we show the finished registrant in a box
    { %>
                <div class="personheader">
                    <%=p.first + " " + p.last %>
                    <span class="blue" style="font-size: 80%">(<a class="toggle" href="#">Details</a>)</span>
<%      if ((p.age >= 16 || !p.birthday.HasValue) && !Model.IsCreateAccount())
        { %>
                    <input type="checkbox" name="m.List[<%=p.index %>].CreatingAccount" value = "true" <%=p.CreatingAccount == true ? "checked='checked'" : "" %>/>
                    Create Account (optional)
<%      } %>
                </div>
<%  } %>
                <table class="particpant" width="95%" cellpadding="6" style='<%=!p.Finished() ? "": "display: none" %>'>
<%  if (p.ShowDisplay())
    // Already found
    {
        Html.RenderPartial("Flow/PersonDisplay", p);
        if (p.OtherOK && !p.ManageSubscriptions())
        // finished with other info
            Html.RenderPartial("Flow/OtherDisplay", p);
        else if (p.AnyOtherInfo())
        // need to edit other info
        { %>
                    <tr>
                        <td colspan="2">
                            <p>
                                We <%=p.IsNew ? "have your new" : "found your"%> record,<br />
                                please continue below.</p>
                        </td>
                    </tr>
<%          Model.ShowOtherInstructions = true;
            Html.RenderPartial("Flow/OtherEdit", p);
        }
    }
    else if (!Model.IsEnded())
    // still taking registrations
        if (p.IsFamily)
        // Already found, display only
        {
            Html.RenderPartial("Flow/PersonDisplay", p);
        }

        else
        // Find or Add New
        {
            Model.ShowFindInstructions = true;
            Html.RenderPartial("Flow/PersonEdit", p);
        }
%>
                </table>
            </div>
        </td>
    </tr>