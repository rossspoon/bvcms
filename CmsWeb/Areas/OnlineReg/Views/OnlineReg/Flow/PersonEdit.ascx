<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel>" %>
<tr>
    <td align="right" colspan="2">
        <%=Html.Required() %>
        required
    </td>
    <td>
    </td>
</tr>
<tr>
    <td valign="top" nowrap="nowrap">
        <%=Html.Required() %>
        <label id="personedit" for="first">
            First</label>
    </td>
    <td>
        <%=Html.TextBox(Model.inputname("first"), Model.first)%>
        <div>
            <%= Html.ValidationMessage(Model.inputname("first"))%></div>
    </td>
</tr>
<tr>
    <td valign="top" nowrap="nowrap">
        <%=Html.NotRequired() %>
        <label for="middle">
            Middle</label>
    </td>
    <td>
        <%=Html.TextBox(Model.inputname("middle"), Model.middle)%>
    </td>
</tr>
<tr>
    <td valign="top" nowrap="nowrap">
        <%=Html.Required() %>
        <label for="last">
            Last</label>
    </td>
    <td>
        <%=Html.TextBox(Model.inputname("last"), Model.last)%>
        <div>
            <%= Html.ValidationMessage(Model.inputname("last"))%></div>
    </td>
</tr>
<tr>
    <td valign="top" nowrap="nowrap">
        <%=Html.NotRequired() %>
        <label for="last">
            Suffix</label>
    </td>
    <td>
        <%=Html.TextBox(Model.inputname("suffix"), Model.suffix, new { @class = "short" })%>
    </td>
</tr>
<tr>
    <td valign="top" nowrap="nowrap">
        <%=Html.IsRequired(Model.RequiredDOB()) %>
        <label for="dob">
            Birthday</label>
    </td>
    <td>
        <%=Html.TextBox3("dob", Model.inputname("dob"), Model.dob, new { @class = "dob" })%>
        (<span id="age"><%=Model.age %></span>)
        <div>
            <%= Html.ValidationMessage(Model.inputname("dob"))%></div>
    </td>
</tr>
<tr>
    <td valign="top" nowrap="nowrap">
        <%=Model.ShowAddress == true? Html.IsRequired(Model.RequiredPhone()) : Html.NotRequired() %>
        <label for="phone">
            Phone</label>
    </td>
    <td>
        <%=Html.TextBox(Model.inputname("phone"), Model.phone)%>
        <div>
            <%= Html.ValidationMessage(Model.inputname("phone"))%></div>
    </td>
</tr>
<tr>
    <td valign="top" nowrap="nowrap">
        <%=Html.Required() %>
        <label for="email">
            Email</label>
    </td>
    <td>
        <%=Html.TextBox(Model.inputname("email"), Model.email)%>
        <div>
            <%= Html.ValidationMessage(Model.inputname("email"))%></div>
    </td>
</tr>
<%
    if (Model.ShowAddress)
    { %>
<% Html.RenderPartial("Flow/AddressEdit", Model); %>
<tr>
    <td>
    </td>
<%      if (Model.ManageSubscriptions())
        { %>
    <td>
        <a href="/OnlineReg/SubmitNew/<%=Model.index %>" class="submitbutton">Submit</a>
<%          if (Model.age >= 16 || !Model.birthday.HasValue)
            { %>
        <%=Html.CheckBox(Model.inputname("CreatingAccount"), Model.CreatingAccount)%>
        Create Account (optional)
<%          } %>
    </td>
<%      }
        else
        { %>
    <td>
        <a href="/OnlineReg/SubmitNew/<%=Model.index %>" class="submitbutton">Submit</a>
    </td>
<%      } %>
</tr>
<%  }
    else
    { %>
<%      if (!Model.Found.HasValue)
        { %>
<tr>
    <td colspan="2" align="right">
        <script type="text/javascript">
            $("div.instructions").hide();
            $("div.instructions.find").show();
        </script>
        <a href="/OnlineReg/PersonFind/<%=Model.index %>" class="submitbutton">Find Profile</a>
        <%=Html.ValidationMessage("classidguest")%>
    </td>
</tr>
<%      }
        else
        { %>
<tr>
    <td colspan="2">
        <div>
            <%= Html.ValidationMessage("findn") %></div>
        <div>
            <%=Model.NotFoundText %></div>
    </td>
</tr>
<%          if (Model.Found == false)
            { %>
<tr>
    <td align="right" colspan="2">
<%              if (Model.index > 0 || Model.LoggedIn == true)
                { %>
        <table>
            <tr>
                <td align="left">
                    Add to which family?
                </td>
            </tr>
            <tr>
                <td align="left">
                    <%=Html.RadioButton(Model.inputname("whatfamily"), "3") %>
                    New
                </td>
            </tr>
<%              }
                else
                { %>
            <%=Html.Hidden(Model.inputname("whatfamily"), "3") %>
<%              }
                if (Model.index > 0)
                { %>
            <tr>
                <td align="left">
                    <%=Html.RadioButton(Model.inputname("whatfamily"), "2") %>
                    Previous
                </td>
            </tr>
<%              }
                if (Model.LoggedIn == true)
                { %>
            <tr>
                <td align="left">
                    <%=Html.RadioButton(Model.inputname("whatfamily"), "1") %>
                    Mine
                </td>
            </tr>
<%              } %>
            <tr>
                <td align="left">
                    <%=Html.ValidationMessage(Model.inputname("whatfamily"))%>
                </td>
            </tr>
        </table>
    </td>
</tr>
<%          } %>
<tr>
    <td colspan="2" align="right">
        <a style="display: inline-block" href="/OnlineReg/PersonFind/<%=Model.index %>" class="submitbutton">
            Search Again</a>
<%          if (Model.IsValidForContinue && !Model.MemberOnly() && Model.orgid != Util.CreateAccountCode)
            { %>
        or <a id="regnew" href="/OnlineReg/ShowMoreInfo/<%=Model.index %>" class="submitbutton">
            Add New Profile</a>
<%          }
            else if (Model.orgid == Util.CreateAccountCode)
            { %>
        <p>
            Call the church if you think your record should be in the system. It may be that
            we are missing some information on your record that we need to identify you like
            a phone number, date of birth or email address.</p>
<%          } %>
    </td>
</tr>
<%      }
    } %>
