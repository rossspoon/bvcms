using System;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;

[assembly: WebResource("CustomControls.multiselect.js", "application/x-javascript")]
[assembly: WebResource("CustomControls.icon.bmp", "image/bmp")]
namespace CustomControls
{
    [DefaultProperty("Items"),
    ToolboxData("<{0}:DropCheck runat=server></{0}:DropCheck>")]
    public class DropCheck : System.Web.UI.WebControls.ListControl
    {
        private PostTextBox t;
        //private ListControl lc;
        private string title;
        private Unit width=Unit.Pixel(100);
        private string id;
        private bool transitional=true;
        private int maxDropDownHeight=200;

        #region Public Properties

        void t_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(e);
        }

        public int MaxDropDownHeight
        {
            get { return maxDropDownHeight; }
            set { maxDropDownHeight = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public override Unit Width
        {
            get { return width; }
            set { EnsureChildControls(); width = value; t.Width = width; }
        }

        public override string ID
        {
	        get { return id; }
            set {  id = value;}
        }

        public override string Text
        {
            get
            {
                EnsureChildControls();
                return t.Text;
            }
            set
            {
                EnsureChildControls();
                t.Text = value;
            }
        }
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public string Value
        {
            get
            {
                return t.Text;
            }
            set
            {
                t.Text = value;
            }
        }

        public bool TransitionalMode
        {
            get { return transitional; }
            set { transitional = value; }
        }

        #endregion

        private string GenerateDiv()
        {
            return string.Format(
                "<div id=\"{0}div\" style=\"width:{1}px; position:absolute;background-color:white;" +
                "z-index:10000;visibility:hidden;border-style:solid;border-width:1px;" +
                "height: expression(this.scrollHeight>{2}?'{2}px':'auto');" +
                "max-height: {2}px; overflow:auto;\">\n{3}\n</div>\n",
                t.UniqueID, Width.Value + 3, MaxDropDownHeight, GenerateCheckboxes());
        }

        private string GenerateCheckboxes()
        {
            if (this.DesignMode)
                return "";
            var sb = new StringBuilder();
            int n=0;
            foreach (ListItem i in Items)
                sb.AppendFormat("<input id=\"{2}_{3}\" type=\"checkbox\" value=\"{0}\"/>" +
                    "<span style=\"font-family:Tahoma;font-size:10px\">{1}<br/></span>\n",
                i.Value, i.Text, t.UniqueID, ++n);
            return sb.ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptResource(typeof(DropCheck), "CustomControls.multiselect.js");
            var b = new StringBuilder();
            b.Append("function PerformPostActions(controlID) { ");
            if(this.AutoPostBack) 
                b.Append("__doPostBack(controlID,'@@@AutoPostBack'); ");
            b.Append(" }");
            Page.ClientScript.GetPostBackEventReference(this, "@@@AutoPostBack");
            Page.ClientScript.RegisterClientScriptBlock(typeof(DropCheck), "autopostbackscript", b.ToString(), true);
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Page.IsPostBack)
            {
                string eventArg = Page.Request["__EVENTARGUMENT"];
                string eventTarget = Page.Request["__EVENTTARGET"];
                if (eventTarget == this.ClientID && eventArg != null)
                {
                    int offset = eventArg.IndexOf("@@@AutoPostBack");
                    if (offset > -1)
                        OnSelectedIndexChanged(new EventArgs());
                }
            }
            base.OnLoad(e);
        }

        protected override void CreateChildControls()
        {
            t = new PostTextBox();
            t.ID = ID + "tb";
            t.Width = Unit.Pixel(100);
            this.Controls.Add(t);
            t.TextChanged += new EventHandler(t_TextChanged);
        }
        public bool ChangedStatus
        { 
            get { return t.ChangedStatus; }
            set { t.ChangedStatus = value; }
        }
        public bool HadBeenChanged { get; set; }

        protected override void Render(HtmlTextWriter output)
        {
            EnsureChildControls();
            if (HttpContext.Current != null && HttpContext.Current.Request.Url.Port != 58001)
                t.ReadOnly = true;
                
            t.Attributes["onclick"] = "placeDiv('" + t.UniqueID + "')";
            t.Attributes["autocomplete"] = "off";
            t.Style["border-width"] = "0px";
            t.Style["vertical-align"] = !transitional?"3px":"5px";
            t.Style["padding"] = "0px";
            t.Width = Unit.Pixel((int)t.Width.Value - 21) ;
            int divWidth = (int)t.Width.Value;
            if (!transitional)
                divWidth += 21;
            else
                divWidth += 17;
            output.Write("<div style=\"border-style:inset; overflow:hidden; border-width:2px; width:" + divWidth + "px; height:" + (!transitional?"24px":"20px") + ";\">");
            this.RenderChildren(output);
            output.Write("<img " + "id=\"" + t.UniqueID +  "img\" style=\"height:20px;width:17px;border-width:0px;\" onclick=\"placeDiv('" + t.UniqueID + "')\" src=\"");
            output.Write(Page.ClientScript.GetWebResourceUrl(typeof(DropCheck), "CustomControls.icon.bmp"));
            output.Write("\"></img>");
            output.Write("</div>");
            output.Write(GenerateDiv());
        }
    }
}
