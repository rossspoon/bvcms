using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.Design;

namespace CustomControls
{
    [NonVisualControl, Designer(typeof(SharedDataDesigner))]
//    [ProvideProperty("DataBindingItem", typeof(Control))]
//    [ParseChildren(true, "DataBindingItems")]
//    [PersistChildren(false)]
//    [DefaultProperty("DataBindingItems")]
//    [DefaultEvent("ValidateControl")]
    public class SharedData : Control
    {
        public delegate string ReadMacroDelegate(string Table, string Field, string Code);
        public delegate Dictionary<string, string> ReadMacrosDelegate(string Table, string Field);
        internal ReadMacroDelegate ReadMacro;
        internal ReadMacrosDelegate ReadMacros;

        public SharedData()
        {
            this.Init += new EventHandler(DisplayOrEditSharedData_Init);
        }

        void DisplayOrEditSharedData_Init(object sender, EventArgs e)
        {
            RecursiveShareControls(Page);
        }
        public override void DataBind()
        {
            foreach (DisplayOrEdit c in _Controls)
                c.DataBind();
        }

        public int Changes { get; set; }

        public SharedData(ReadMacroDelegate ReadMacro, ReadMacrosDelegate ReadMacros)
        {
            this.ReadMacro = ReadMacro;
            this.ReadMacros = ReadMacros;
            base.EnableViewState = true;
            base.Visible = false;
        }
        public bool Editing
        {
            get { return (bool)(ViewState["Editing"] ?? false); }
            set { ViewState["Editing"] = value; }
        }
        private List<DisplayOrEdit> _Controls = new List<DisplayOrEdit>();
        private void RecursiveShareControls(Control cc)
        {
            foreach (Control c in cc.Controls)
            {
                if (c is DisplayOrEdit)
                {
                    ((DisplayOrEdit)c).SharedData = this;
                    _Controls.Add((DisplayOrEdit)c);
                }
                if (c.HasControls())
                    RecursiveShareControls(c);
            }
        }
        public void DisableHyperlinks()
        {
            DisableHyperlinksRecursive(Page);
        }
        private void DisableHyperlinksRecursive(Control cc)
        {
            foreach (Control c in cc.Controls)
            {
                if (c is HyperLink)
                {
                    HyperLink h = (HyperLink)c;
                    if (Editing)
                        h.Attributes.Add("onclick", "alert('Must update first'); return false");
                    else
                        h.Attributes.Remove("onclick");
                }
                else if (c is LinkButtonConfirm)
                    ((LinkButtonConfirm)c).Editing = Editing;
                if (c.HasControls())
                    DisableHyperlinksRecursive(c);
            }
        }

    }
    internal class SharedDataDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            return base.CreatePlaceHolderDesignTimeHtml("Control Extender");
        }
    }

}
