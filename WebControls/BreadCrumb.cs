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
    [ToolboxData("<{0}:BreadCrumb runat=\"server\" />")]
    public class BreadCrumb : WebControl
    {
        private string _Text;
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        private string _Url;
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Text.HasValue())
                Add(Text, Url);
        }
        private List<WebControl> list = new List<WebControl>();
        public BreadCrumb Add(string Text)
        {
            return Add(Text, null);
        }
        public BreadCrumb Add(string Text, string urlformat, params object[] args)
        {
            if (string.IsNullOrEmpty(urlformat))
            {
                Label label = new Label();
                label.Text = Text;
                list.Add((WebControl)label);
            }
            else
            {
                HyperLink h = new HyperLink();
                h.NavigateUrl = urlformat.Fmt( args);
                h.Text = Text;
                list.Add((WebControl)h);
            }
            return this;
        }
        protected override void RenderContents(HtmlTextWriter output)
        {
            if (list.Count == 1 && list[0] is Label)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                list[i].RenderControl(output);
                if (i<list.Count-1)
                    output.Write(" ? ");
            }
            //window.ctl00_ctl00_bhcr_nr_CommunityPopup = new Telligent_PopupMenu('ctl00_ctl00_bhcr_nr_CommunityPopup','CSContextMenuGroup','CSContextMenuItem','CSContextMenuItemHover','CSContextMenuItemExpanded','',0,0,0,0,'updown',100,null,null,null,[['ctl00_ctl00_bhcr_nr_ctl01','Forums','/forums/',null,null,null,null],['ctl00_ctl00_bhcr_nr_ctl02','<div class="CSContextMenuSeparator"></div>',null,null,null,null,null],['ctl00_ctl00_bhcr_nr_ctl03','Blogs','/blogs/',null,null,null,null],['ctl00_ctl00_bhcr_nr_ctl04','<div class="CSContextMenuSeparator"></div>',null,null,null,null,null],['ctl00_ctl00_bhcr_nr_ctl05','Downloads','/files/',null,null,null,null],['ctl00_ctl00_bhcr_nr_ctl06','<div class="CSContextMenuSeparator"></div>',null,null,null,null,null],['ctl00_ctl00_bhcr_nr_ctl07','Photos','/photos/',null,null,null,null]],true);
        }
    }
}
