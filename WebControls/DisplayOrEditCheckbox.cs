using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using UtilityExtensions;

namespace CustomControls
{
    public class DisplayOrEditCheckbox : CheckBox, IDisplayOrEdit, IPostBackDataHandler
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public string TextIfChecked { get; set; }
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public string TextIfNotChecked { get; set; }

        protected override void OnInit(EventArgs e)
        {
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
            this.RegisterEditButton(Page);
        }

        public override void DataBind()
        {
            if (EditUpdateButton.Updating)
                EditUpdateButton.UpdateBinding(this);
            //base.DataBind();
            bool ck;
            if (!bool.TryParse(EditUpdateButton.DataBindValue(this), out ck))
                ck = false;
            Checked = ck;
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
                if (TextIfChecked.HasValue() || TextIfNotChecked.HasValue())
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Label);
                    writer.Write(Checked ? TextIfChecked : TextIfNotChecked);
                    writer.RenderEndTag();
                }
                else
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
                    if (Checked)
                        writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
                    writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag(); //Input
                    writer.RenderBeginTag(HtmlTextWriterTag.Label);
                    writer.Write(Text);
                    writer.RenderEndTag(); //Label
                    writer.RenderEndTag(); //Span
                }
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
            return Checked.ToString();
        }
        public bool ChangedStatus { get; set; }
        public bool HadBeenChanged { get; set; }

        public bool HasEditButton
        {
            get { return EditUpdateButton != null; }
        }

        public bool Editing
        {
            get
            {
                return EditUpdateButton.Editing;
            }
        }
        public string AssociatedRowId { get; set; }
        public string EditGroup { get; set; }
        public string EditRole { get; set; }

        #endregion

        #region IPostBackDataHandler Members

        bool IPostBackDataHandler.LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            if (EditUpdateButton.Updating)
            {
                ChangedStatus = false;
                string str = postCollection[postDataKey];
                bool flag2 = !string.IsNullOrEmpty(str);
                ChangedStatus = flag2 != this.Checked;
                Checked = flag2;
                if (ChangedStatus)
                    RaisePostDataChangedEvent();
            }
            return ChangedStatus;
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            OnCheckedChanged(null);
        }

        #endregion
    }
}
