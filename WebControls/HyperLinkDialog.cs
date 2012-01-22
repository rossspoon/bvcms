using System.ComponentModel;
using System.Web.UI;
using System;
using UtilityExtensions;
using System.Web;

namespace CustomControls
{
    [DefaultProperty("Text"), ToolboxData("<{0}:HyperLinkDialog runat=server></{0}:HyperLinkDialog>")]
    public class HyperLinkDialog : System.Web.UI.WebControls.HyperLink, IPostBackEventHandler
    {
        public event EventHandler Click;
        public string DialogReturn { get; set; }

        [Bindable(true), Category("Behavior"), DefaultValue("")]
        public string CallFunction { get; set; }

        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            if (this.NavigateUrl == "")
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:__" + UniqueID + "()");
        }

        override protected void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            if (this.NavigateUrl == "")
            {
                string post = Page.ClientScript.GetPostBackEventReference(this, "ret").Replace("'ret'", "ret");
                string Script = 
@"function __{1}() {{ var ret; ret=showModalDialog('{2}', '', 'resizable=yes;dialogWidth=700px;dialogHeight=500px'); {0}; }}"
.Fmt(post, UniqueID, CallFunction);
                Page.ClientScript.RegisterClientScriptBlock(typeof(HyperLinkDialog), "script_" + UniqueID, 
                    Script, true);
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            DialogReturn = HttpContext.Current.Request["__EVENTARGUMENT"];
            OnClick(new EventArgs());
        }

        protected virtual void OnClick(EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }
    }
}