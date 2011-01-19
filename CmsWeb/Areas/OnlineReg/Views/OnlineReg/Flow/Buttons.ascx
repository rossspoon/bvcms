<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel>" %>
<% if (Model.last != null && Model.last.OtherOK && Model.last.ShowDisplay())
    { %>
<div style="margin-top: 10px; text-align:right">
<%      if (Model.ManagingSubscriptions())
        { %>
    <input id="submitit" type="submit" class="submitbutton" 
            value='Manage Subscriptions' />
<%      }
        else if (Model.IsCreateAccount())
        { %>
    <input id="submitit" type="submit" class="submitbutton" 
            value='Create Account' />
<%      }
        else if (Model.ChoosingSlots())
        { %>
    <input id="submitit" type="submit" class="submitbutton" 
            value='Choose Times' />
<%      }
        else
        { %>
    <input id="submitit" type="submit" class="submitbutton" 
            value='Complete Registration' />
<%      }
        if (!Model.OnlyOneAllowed())
        { %>
    <input type="submit" href="/OnlineReg/AddAnotherPerson/" class="submitbutton ajax"
            value="Add Other Registrations" />
<%      } %>
</div>
<% } %>