<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel>" %>
<tr>
    <td nowrap="nowrap"><label id="personedit" for="first">First Name</label> <%=Html.Required() %></td>
    <td><input id="first" type="text" name="m.List[<%=Model.index%>].first" value="<%=Model.first%>" /></td>
    <td>middle:</td>
    <td><input type="text" name="m.List[<%=Model.index%>].middle" class="short" value="<%=Model.middle%>" /></td>
    <td width="100%"><%= Html.ValidationMessage("first") %> </td>
</tr>
<tr>
    <td nowrap="nowrap"><label for="last">Last Name</label> <%=Html.Required() %></td>
    <td nowrap="nowrap"><input id="last" type="text" name="m.List[<%=Model.index%>].last" value="<%=Model.last%>" /></td>
    <td>suffix:</td>
    <td><input type="text" name="m.List[<%=Model.index%>].suffix" class="short" value="<%=Model.suffix%>" /></td>
    <td width="100%"><%= Html.ValidationMessage("last") %></td>
</tr>
 <tr>
    <td nowrap="nowrap"><label for="dob">Date of Birth</label> <%=Html.NotRequired(!Model.RequiredDOB()) %></td>
    <td nowrap="nowrap"><input id="dob" type="text" name="m.List[<%=Model.index%>].dob" value="<%=Model.dob%>" class="dob" title="m/d/y, mmddyy, mmddyyyy" /></td>
    <td>(m/d/yy)</td>
    <td nowrap="nowrap">age: <span id="age"><%=Model.age %></span></td>
    <td width="100%"><%= Html.ValidationMessage("dob") %></td>
</tr>
<tr>
    <td><label for="phone">Phone</label></td>
    <td><input type="text" name="m.List[<%=Model.index%>].phone" value="<%=Model.phone%>" /></td>
    <td colspan="3"><%= Html.ValidationMessage("phone")%></td>
</tr>
<tr>
    <td nowrap="nowrap"><label for="email">Contact Email</label> <%=Html.Required() %></td>
    <td><input type="text" name="m.List[<%=Model.index%>].email" value="<%=Model.email%>" /></td>
    <td colspan="3"><%= Html.ValidationMessage("email")%></td>
</tr>
<%
    if (Model.ShowAddress)
    { %>
<% Html.RenderPartial("AddressEdit", Model); %>
<tr><td><%=Html.Required() %> Required</td>
<%      if (Model.ManageSubscriptions())
        { %>
    <td colspan="4">
        <a href="/OnlineReg/SubmitNew/<%=Model.index %>" class="submitbutton">Submit</a>
<%          if (Model.age >= 16 || !Model.birthday.HasValue)
            { %>                    
        <input type="checkbox" name="m.List[<%=Model.index %>].CreatingAccount" value = "true" <%=Model.CreatingAccount == true ? "checked='checked'" : "" %> /> Create Account (optional)
<%          } %>
    </td>
<%      }
        else
        { %>
    <td colspan="4"><a href="/OnlineReg/SubmitNew/<%=Model.index %>" class="submitbutton">Submit</a></td>
<%      } %>
</tr>
<%  }
    else
    { %>
<tr><td valign="top"><div class="blue"><%=Html.Required() %> Required</div></td>
    <td valign="top" colspan="5">
    <table><tr>
    <td>
<%      if (!Model.Found.HasValue)
        { %>
        <script type="text/javascript">
            $("div.instructions").hide();
            $("div.instructions.find").show();
        </script>
        <a href="/OnlineReg/PersonFind/<%=Model.index %>" class="submitbutton">Find Record</a>
        <%=Html.ValidationMessage("classidguest")%>
<%      }
        else
        { %>
       <div class="blue"><%=Model.NotFoundText %></div>
       <a style="display:inline-block" href="/OnlineReg/PersonFind/<%=Model.index %>" class="submitbutton">Try Find Again</a>
<%          if (Model.IsValidForContinue && !Model.MemberOnly() && Model.orgid != Util.CreateAccountCode)
            { %>
       or <a id="regnew" href="/OnlineReg/ShowMoreInfo/<%=Model.index %>" class="submitbutton">Add a New Record</a>
<%          }
            else if (Model.orgid == Util.CreateAccountCode)
            { %>
       <p class="blue">Call the church if you think your record should be in the system.
       It may be that we are missing some information on your record that we need to identify you
       like a phone number, date of birth or email address.</p>
<%          } %>
    </td>
<%          if (Model.Found == false)
            { %>
        <td valign="bottom">
<%              if (Model.index > 0 || Model.LoggedIn == true)
                { %>
        <span class="blue">Add new person to which family?</span><br />
        <input name="m.List[<%=Model.index%>].whatfamily" type="radio" value="3" /> New Family
<%              }
                else
                { %>
        <input name="m.List[<%=Model.index%>].whatfamily" type="hidden" value="3" />
<%              }
                if (Model.index > 0)
                { %>
        <br /><input name="m.List[<%=Model.index%>].whatfamily" type="radio" value="2" /> Previous Family
<%              }
                if (Model.LoggedIn == true)
                { %>
        <br /><input name="m.List[<%=Model.index%>].whatfamily" type="radio" value="1" /> My Family
<%              } %>
        <div><%=Html.ValidationMessage("whatfamily")%></div>
        </td>
        <td></td>
<%          }
        } %>
    </tr></table>
    </td>
</tr>
<%  } %>
