using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Reflection;
using System.Drawing.Design;
using Alias = System.Windows.Forms;
using System.Linq;
using System.ComponentModel.Design;

namespace CustomControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DisplayOrEdit runat=server></{0}:DisplayOrEdit>")]
    public class DisplayOrEdit : WebControl, IPostBackDataHandler, IDisplayOrEdit
    {
        public new bool DesignMode = (HttpContext.Current == null);

        public EditUpdateButton EditUpdateButton  { get; set; }

        public string BeforeText;


        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public string Text
        {
            get { return (string)ViewState["Text"] ?? String.Empty; }
            set { ViewState["Text"] = value; }
        }
        [Bindable(true)]
        [Category("Layout")]
        [DefaultValue("")]
        public int Rows { get; set; }

        [Bindable(true), Category("Layout"), DefaultValue("")]
        public int Size { get; set; }

        [Editor(typeof(MemberDropDownEditor), typeof(UITypeEditor))]
        [Bindable(true), Category("Binding"), DefaultValue("")]
        public string BindingMember { get; set; }

        [Bindable(true), Category("Binding")]
        public string BindingSource { get; set; }

        [Bindable(true), Category("Binding")]
        public string BindingType { get; set; }

        [Browsable(false)]
        public object BindingSourceObject { get; set; }

        public string DataSource { get; set; }
        public string DataTextField { get; set; }
        public string DataValueField { get; set; }

        [Category("Layout"), DefaultValue(typeof(EditType), "Text")]
        private EditType _Type;
        public EditType Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
            }
        }

        [Bindable(true), Category("Binding"), DefaultValue(BindingModes.TwoWay)]
        public BindingModes BindingMode { get; set; }
        
        [Browsable(false)]
        public int Changes
        {
            get
            {
                if (DesignMode)
                    return 0;
                return EditUpdateButton.Changes;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Type == EditType.Date && EditUpdateButton.Editing)
            {
                Type type = Page.GetType();
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterClientScriptBlock(type, "DatePickerSrc",
                    "<script language=javascript src='PopCal.js'></script>");
                cs.RegisterStartupScript(type, "DatePickerInit",
                    "<script language=javascript>window.attachEvent('onload',PopCalInit);</script>");
            }
        }

        protected override void DataBind(bool raiseOnDataBinding)
        {
            base.DataBind(raiseOnDataBinding);
            Text = EditUpdateButton.DataBindValue(this);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                writer.Write("DataBound");
                return;
            }
            if (string.IsNullOrEmpty(BindingMember))
                BindingMember = this.ID;

            if (EditUpdateButton.Editing && BindingMode == BindingModes.TwoWay)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
                switch (Type)
                {
                    case EditType.Text:
                    case EditType.Number:
                    case EditType.Phone:
                        writer.AddAttribute(HtmlTextWriterAttribute.Alt, BindingMember);
                        if (Rows > 1)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Title, BindingMember);
                            writer.AddAttribute(HtmlTextWriterAttribute.Rows, Rows.ToString());
                            writer.AddAttribute(HtmlTextWriterAttribute.Cols, Size.ToString());
                            writer.RenderBeginTag(HtmlTextWriterTag.Textarea);
                            writer.Write(Text);
                            writer.RenderEndTag();
                        }
                        else
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Title, BindingMember);
                            //writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, 
                            //    mi.GetAttributeInfo(typeof(SizeAttribute)).ToString());
                            writer.AddAttribute(HtmlTextWriterAttribute.Size, Size.ToString());
                            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                            writer.AddAttribute(HtmlTextWriterAttribute.Value, Text);
                            writer.RenderBeginTag(HtmlTextWriterTag.Input);
                            writer.RenderEndTag();
                        }
                        break;
                    case EditType.Date:
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "PopCalText_" + UniqueID);
                        writer.AddAttribute(HtmlTextWriterAttribute.Type, "Text");
                        writer.AddAttribute(HtmlTextWriterAttribute.Value, DateTime.Parse(Text).ToShortDateString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Input);
                        writer.RenderEndTag();
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "PopCalButton_" + UniqueID);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, "images/QuickDate.gif");
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "PopCalShow(this)");
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                        break;
                    case EditType.Code:
                        writer.RenderBeginTag(HtmlTextWriterTag.Select);
                        Dictionary<string, string> ht = EditUpdateButton.ReadMacros(BindingSource, BindingMember);
                        foreach (KeyValuePair<string, string> r in ht)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Value, r.Key);
                            if (Text == r.Key)
                                writer.AddAttribute(HtmlTextWriterAttribute.Selected, "true");
                            writer.RenderBeginTag(HtmlTextWriterTag.Option);
                            writer.Write(r.Key);
                            if (r.Key != r.Value && r.Value != "")
                                writer.Write(" - " + r.Value);
                            writer.RenderEndTag(); //option
                        }
                        writer.RenderEndTag(); //select
                        break;
                }
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Title, BindingMember);
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                switch (Type)
                {
                    case EditType.Code:
                        writer.Write(Text);
                        string desc = EditUpdateButton.ReadMacro(BindingSource, BindingMember, Text);
                        if (Text != desc && desc != "")
                            writer.Write(" - " + desc);
                        break;
                    case EditType.Date:
                        writer.Write(DateTime.Parse(Text).ToShortDateString());
                        break;
                    case EditType.Phone:
                    default:
                        writer.Write(Text);
                        break;
                }
                writer.RenderEndTag();
            }
        }

        #region IPostBackDataHandler Members

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string postedData = postCollection[postDataKey];
            if (postedData != Text)
            {
                BeforeText = Text;
                Text = postedData;
                return true;
            }
            return false;
        }

        public void RaisePostDataChangedEvent()
        {
            //if (Type == EditType.Code)
            //{
            //    EnumConverter ec = new EnumConverter(mi.MemberType);
            //    e.SetMemberValue(Field, ec.ConvertFromString(Text));
            //}
            if (string.IsNullOrEmpty(BindingMember))
                BindingMember = this.ID;
            if (BindingSourceObject == null)
            {
                if (BindingSource == "this")
                    BindingSourceObject = Page;
                else
                    BindingSourceObject = Utils.GetPropertyEx(Page, BindingSource);
            }
            if (BindingSourceObject == null)
                throw new ApplicationException("Invalid BindingSource");

            Type typBindingSource = null;
            object minfo = BindingSourceObject.GetType().GetMember(BindingMember, Utils.bindingFlags)[0];
            MemberTypes mt;
            if (minfo is FieldInfo)
                mt = ((FieldInfo)minfo).MemberType;
            else
                mt = ((PropertyInfo)minfo).MemberType;
            
            if (mt == MemberTypes.Field)
                typBindingSource = ((FieldInfo)minfo).FieldType;
            else
                typBindingSource = ((PropertyInfo)minfo).PropertyType;

            object value;
            if (typBindingSource == typeof(string))
                value = Text;
            else if (typBindingSource == typeof(int))
            {
                int v;
                if (!int.TryParse(Text, out v))
                    throw new Exception("Invalid numeric input");
                else
                    value = v;
            }
            else if (typBindingSource == typeof(byte))
                value = Convert.ToByte(Text);
            else if (typBindingSource == typeof(decimal))
                value = Decimal.Parse(Text);
            else if (typBindingSource == typeof(double))
                value = Double.Parse(Text);
            else if (typBindingSource == typeof(bool))
                value = Text;
            else if (typBindingSource == typeof(DateTime))
            {
                DateTime dt = DateTime.MinValue;
                if (!DateTime.TryParse(Text, out dt))
                    throw new Exception("Invalid date input");
                else
                    value = dt;
            }
            else if (typBindingSource.IsEnum)
                value = Enum.Parse(typBindingSource, Text);
            else
                throw (new Exception("Field Type not Handled by Data unbinding"));
            Utils.SetPropertyEx(BindingSourceObject, BindingMember, value);
            EditUpdateButton.Changes += 1;
        }
        #endregion

        #region IDisplayOrEdit Members


        public string TextValue
        {
            get { return Text; }
            set { Text = value; }
        }

        #endregion
    }
    public class MemberDropDownEditor : BaseDropDownListTypeEditor
    {
        protected override void FillInList(ITypeDescriptorContext pContext, IServiceProvider pProvider, Alias.ListBox pListBox)
        {
            DisplayOrEdit c = (DisplayOrEdit)pContext.Instance;
            IDesignerHost dh = (IDesignerHost)pContext.GetService(typeof(IDesignerHost));
            Control parent = dh.RootComponent as Control;

//            EnvDTE80.DTE2 dte2;
//            dte2 = (EnvDTE80.DTE2)Marshal.GetActiveObject("VisualStudio.DTE.8.0");
//            DTE2 dte = (DTE2)pContext.GetService(typeof(DTE));
//            string path = "";
//            foreach (EnvDTE.Property p in dte2.ActiveDocument.ProjectItem.ProjectItems)
//                if (string.Compare(p.Name, "LocalPath", true) == 0)
//                {
//                    path = p.Value.ToString();
//                    break;
//                }
//            StreamReader sr = File.OpenText(path);
//            string line = sr.ReadLine();
//            sr.Close();
//            Match m = Regex.Match(line, @"inherits=\""(?<class>.*)\""");
//            string classname = m.Groups["class"].Value;
//            Type t = System.Type.GetType(classname);
//            FieldInfo fi = t.GetFields().Where(p => p.Name == c.BindingMember).Single();
//            var props = fi.DeclaringType.GetProperties().OrderBy(p => p.Name).Select(p => p.Name);
            Type t = System.Type.GetType(c.BindingType);
            var props = t.GetProperties().OrderBy(p => p.Name).Select(p => p.Name);
            foreach (string s in props)
                pListBox.Items.Add(s);
        }
    }
}
