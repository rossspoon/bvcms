using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;

namespace CustomControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:LinkButtonConfirm runat=server></{0}:LinkButtonConfirm>")]
    public class LinkButtonConfirm : LinkButton
    {
        public bool Editing;

        private string _Confirm;
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Confirm
        {
            get { return _Confirm; }
            set
            {
                _Confirm = value.Replace("'", "");
                if (!_Confirm.HasValue())
                    _Confirm = "Are you sure?";
            }
        }

        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            if (Confirm != "" & !Editing)
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "javascript:return confirm('" + Confirm + "')");
            else if (Editing)
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "alert('Must update first'); return false");
            base.AddAttributesToRender(writer);
        }
    }
}
