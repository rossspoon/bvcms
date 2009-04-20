using System.Web.UI.WebControls;
using System;
using System.Web.UI;

namespace CustomControls
{
    public class DefaultButtonTextBox : TextBox
    {
        private const string OnKeyDownButtonAttribute = "KeyDownHandler{0}(event);";

        private Control FindCtl(Control root, string id)
        {
            if (root.ID == id)
                return root;
            foreach (Control c in root.Controls)
            {
                Control f = FindCtl(c, id);
                if (f != null)
                    return f;
            }
            return null;
        }
        
        protected override void OnPreRender(EventArgs e)
        {

            Button button = (Button)FindCtl(Page, ButtonId);
            if (button != null)
            {
                RegisterButtonScript(button);
                Attributes.Add("onkeydown", string.Format("KeyDownHandler{0}(event);",
                    button.ClientID));
            }
            base.OnPreRender(e);
        }

        protected void RegisterButtonScript(Button button)
        {
            string script =
    @"<script type=""text/javascript"">
<!--
function KeyDownHandler{0}(event)
{{
    if (event.keyCode == 13)
    {{
        event.returnValue = false;
        event.cancel = true;
        document.getElementById('{0}').click();
    }}
}}
//-->
</script>";
            script = string.Format(script, button.ClientID
                //Page.ClientScript.GetPostBackEventReference(button, "")
                );

            Page.ClientScript.RegisterStartupScript(typeof(DefaultButtonTextBox),
                button.ClientID,
                script, false);
            Page.RegisterRequiresPostBack(this);
        }
        public string ButtonId
        {
            get { return (((string)ViewState["ButtonId"]) ?? string.Empty); }
            set { ViewState["ButtonId"] = value; }
        }
    }
}