using System;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using UtilityExtensions;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.Web.UI.HtmlControls;

namespace CustomControls
{
    public class DisplayOrEditCurrency : TextBox, IDisplayOrEdit
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.RegisterEditButton(Page);
        }
        public override void DataBind()
        {
            if (EditUpdateButton.Updating)
                EditUpdateButton.UpdateBinding(this);
            //base.DataBind();
            SetTextValue(EditUpdateButton.DataBindValue(this));
            if (AssociatedRowId.HasValue())
            {
                var tr = this.Parent.Parent as HtmlTableRow;
                if (tr != null && tr.ID == AssociatedRowId)
                    tr.Visible = Text.HasValue() || Editing;
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
                var label = new Label();
                label.Text = GetTextValue();
                label.ID = this.UniqueID;
                label.RenderControl(writer);
            }
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
            return this.Text;
        }

        private void SetTextValue(string value)
        {
            Decimal d;
            if (Decimal.TryParse(value, out d))
                Text = d.ToString("n");
            else
                Text = "";
        }
        public bool ChangedStatus { get; set; }
        public bool HadBeenChanged { get; set; }
        public bool Editing
        {
            get { return EditUpdateButton.Editing; }
        }

        public bool HasEditButton
        {
            get { return EditUpdateButton != null; }
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
