using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using UtilityExtensions;
using System.Web.UI.HtmlControls;

namespace CustomControls
{
    public class DisplayOrEditMaskedText : DisplayOrEditText
    {
        public enum MaskTypes
        {
            Phone,
            Zip
        }
        public DisplayOrEditMaskedText()
            : base()
        {
            MaskType = MaskTypes.Phone;
        }

        [Bindable(true), Category("Appearance"), DefaultValue(MaskTypes.Phone)]
        public MaskTypes MaskType { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.RegisterEditButton(Page);
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
            if (DesignMode || ShowEditMode)
            {
                base.ToolTip = BindingMember;
                base.Render(writer);
            }
            else
            {
                var lab = new Label();
                switch (MaskType)
                {
                    case MaskTypes.Phone:
                        lab.Text = Text.FmtFone();
                        break;
                    case MaskTypes.Zip:
                        lab.Text = Text.FmtZip();
                        break;
                }
                lab.ToolTip = BindingMember;
                lab.RenderControl(writer);
            }
        }
        public override void DataBind()
        {
            if (EditUpdateButton.Updating)
                EditUpdateButton.UpdateBinding(this);
            //base.DataBind();
            switch (MaskType)
            {
                case MaskTypes.Phone:
                    Text = EditUpdateButton.DataBindValue(this).FmtFone();
                    break;
                case MaskTypes.Zip:
                    Text = EditUpdateButton.DataBindValue(this).FmtZip();
                    break;
            }
            if (AssociatedRowId.HasValue())
            {
                var tr = this.Parent.Parent as HtmlTableRow;
                if (tr != null && tr.ID == AssociatedRowId)
                    tr.Visible = Text.HasValue() || Editing;
            }
        }
        public override string GetTextValue()
        {
            return Text.GetDigits();
        }
    }
}
