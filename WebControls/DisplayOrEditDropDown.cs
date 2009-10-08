using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using UtilityExtensions;
using System.Web.UI.HtmlControls;

namespace CustomControls
{
    public class DisplayOrEditDropDown : DropDownCC, IDisplayOrEdit
    {
        public enum DisplayModes
        {
            Code,
            Text
        }
        public bool MakeDefault0 { get; set; }
        public DisplayOrEditDropDown() : base()
        {
            DisplayMode = DisplayModes.Text;
        }

        [Bindable(true), Category("Appearance"), DefaultValue(DisplayModes.Text)]
        public DisplayModes DisplayMode { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.RegisterEditButton(Page);
        }
        public override void DataBind()
        {
            if (EditUpdateButton.Updating)
                EditUpdateButton.UpdateBinding(this);
            base.DataBind();
            string v = EditUpdateButton.DataBindValue(this);
            if (MakeDefault0)
                SelectedValue = v == "" ? "0" : v;
            else
                SelectedValue = v;
            if (AssociatedRowId.HasValue())
            {
                var tr = this.Parent.Parent as HtmlTableRow;
                if (tr != null && tr.ID == AssociatedRowId)
                    tr.Visible = v.HasValue() || Editing;
            }
        }
        private bool ShowEditMode
        {
            get
            {
                if (EditUpdateButton.Editing && BindingModes.TwoWay == BindingMode)
                    if (!EditRole.HasValue() || HttpContext.Current.User.IsInRole(EditRole))
                        return true;
                return false;
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Title, BindingMember);
            if (DesignMode || ShowEditMode)
                base.Render(writer);
            else
            {
                const string STR_style = "style";
                if (Attributes[STR_style].HasValue())
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, Attributes[STR_style]);
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                if (DisplayMode == DisplayModes.Code)
                    writer.Write(SelectedValue);
                else
                    writer.Write(SelectedItem);
                writer.RenderEndTag();
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
        }

        #region IDisplayOrEdit Members

        private EditUpdateButton EditUpdateButton;
        public void SetEditUpdateButton(EditUpdateButton bt)
        {
            EditUpdateButton = bt;
        }
        private string _BindingMember;
        public string BindingMember
        {
            get
            {
                if (!_BindingMember.HasValue())
                    return this.ID;
                return _BindingMember;
            }
            set { _BindingMember = value; }
        }
        public string BindingSource { get; set; }
        public BindingModes BindingMode { get; set; }
        public string GetTextValue()
        {
            if (MakeDefault0 && SelectedValue == "0")
                return null;
            return SelectedValue;
        }
        public bool ChangedStatus { get; set; }
        public bool HadBeenChanged { get; set; }

        public bool HasEditButton
        {
            get { return EditUpdateButton != null; }
        }

        public bool Editing
        {
            get { return EditUpdateButton.Editing; }
        }
        public string AssociatedRowId { get; set; }
        public string EditGroup { get; set; }
        public string EditRole { get; set; }

        #endregion
        protected override bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            ChangedStatus = base.LoadPostData(postDataKey, postCollection);
            return ChangedStatus;
        }
    }
}
