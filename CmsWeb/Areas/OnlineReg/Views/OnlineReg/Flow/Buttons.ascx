<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel>" %>
<% if (Model.last != null && Model.last.OtherOK && Model.last.ShowDisplay())
    { %>
<div class="instruct" style="margin-top: 10px">
    <%   if (!Model.OnlyOneAllowed())
         { %>
    <input type="submit" href="/OnlineReg/AddAnotherPerson/" class="submitbutton ajax"
        value="Register Another" />
    <% } %>
    <% var amt = Model.Amount();
       if (amt > 0)
       { %>
    <input id="submitit" type="submit" class="submitbutton" value='Complete Registration and Pay <%=amt.ToString("c") %>' />
    <% }
       else if (Model.Terms.HasValue())
       { %>
    <input id="submitit" type="submit" class="submitbutton" value='Complete Registration and Read Terms' />
    <% }
       else if (Model.ManagingSubscriptions())
       { %>
    <input id="submitit" type="submit" class="submitbutton" value='Receive a Link to Manage Subscriptions' />
    <% }
       else if (Model.IsCreateAccount())
       { %>
    <input id="submitit" type="submit" class="submitbutton" value='Create Account' />
    <% }
       else if (Model.ChoosingSlots())
       { %>
    <input id="submitit" type="submit" class="submitbutton" value='Choose Committment Times' />
    <% }
       else
       { %>
    <input id="submitit" type="submit" class="submitbutton" value='Complete Registration' />
    <% } %>
</div>
<% } %>