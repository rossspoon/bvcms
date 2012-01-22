using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using UtilityExtensions;
using System.Web.UI.HtmlControls;

namespace CustomControls
{
    public class DisplayOrEditText : PostTextBox, IDisplayOrEdit
    {
        public DisplayOrEditText()
            : base()
        {
            Width = 135;
            EnableViewState = false;
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresControlState(this);
            this.RegisterEditButton(Page);
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(135.0)]
        public override Unit Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }
        public override void DataBind()
        {
            if (EditUpdateButton.Updating)
                EditUpdateButton.UpdateBinding(this);
            //base.DataBind();
            Text = EditUpdateButton.DataBindValue(this);
            if (AssociatedRowId.HasValue())
            {
                var tr = this.Parent.Parent as HtmlTableRow;
                if (tr != null && tr.ID==AssociatedRowId)
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
            if (DesignMode || ShowEditMode)
            {
                base.ToolTip = BindingMember;
                base.Render(writer);
            }
            else
            {
                if (TextMode == TextBoxMode.MultiLine)
                {
                    var lit = new Literal();
                    lit.ID = this.ClientID;
                    lit.Text = Util.SafeFormat(Text);
                    lit.RenderControl(writer);
                }
                else
                {
                    var lab = new Label();
                    lab.ID = this.ClientID;
                    lab.Text = Text;
                    lab.ToolTip = BindingMember;
                    lab.RenderControl(writer);
                }
            }
        }

        protected override void LoadControlState(object savedState)
        {
            object[] controlState = (object[])savedState;
            base.LoadControlState(controlState[0]);
            Text = (string)controlState[1];
        }
        protected override object SaveControlState()
        {
            object[] controlState = new object[2];
            controlState[0] = base.SaveControlState();
            controlState[1] = Text;
            return controlState;
        }

        [DefaultValue(false)]
        public override bool EnableViewState
        {
            get { return base.EnableViewState; }
            set { base.EnableViewState = value; }
        }
        #region IDisplayOrEdit Members

        protected EditUpdateButton EditUpdateButton;
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
        public virtual string GetTextValue()
        {
            return Text;
        }
        public bool GetChangedStatus()
        {
            return ChangedStatus;
        }

        public bool Editing
        {
            get { return EditUpdateButton.Editing; }
        }

        public string AssociatedRowId { get; set; }
        public string EditGroup { get; set; }
        public string EditRole { get; set; }

        #endregion
    }
}
