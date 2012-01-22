/*--- USING THIS CODE ----------------------------------------------------------
1. Add this file to a web project that has a custom control which needs it.

2. In the file with that custom control (as a suggestion), create a subclass of the desired base
   class.
   
FOR BaseDropDownListTypeEditor, override the FillInList method. Here is an example:

   public class SampleControlTypeEditor : BaseDropDownListTypeEditor
   {
      protected override void FillInList(ITypeDescriptorContext pContext, IServiceProvider pProvider, ListBox pListBox)
      {
         pListBox.Items.Add("String 1");
         pListBox.Items.Add("String 2");
      }
   }

3. Associate the TypeEditor to your property.

class MyControl ...
{
   [Editor(typeof(MyControlTypeEditor), typeof(UITypeEditor))]
   public string MyOtherControlID { get {...;} set {...;} }
...
}

------------------------------------------------------------------- */
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;

namespace CustomControls
{
    public abstract class BaseDropDownListTypeEditor : UITypeEditor
    {
        private IWindowsFormsEditorService fEdSvc = null;
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext pContext)
        {
            if (pContext != null && pContext.Instance != null)
            {
                return UITypeEditorEditStyle.DropDown;
            }
            return base.GetEditStyle(pContext);
        }

        public override object EditValue(ITypeDescriptorContext pContext, IServiceProvider pProvider, object pValue)
        {
            if (pContext != null
               && pContext.Instance != null
               && pProvider != null)
                try
                {
                    fEdSvc = (IWindowsFormsEditorService)
                       pProvider.GetService(typeof(IWindowsFormsEditorService));
                    ListBox vListBox = new ListBox();
                    vListBox.Click += new EventHandler(List_Click);
                    FillInList(pContext, pProvider, vListBox);
                    vListBox.SelectedItem = pValue;
                    fEdSvc.DropDownControl(vListBox);
                    return vListBox.SelectedItem;
                }
                finally
                {
                    fEdSvc = null;
                }
            else
                return pValue;

        }

        protected void List_Click(object pSender, EventArgs pArgs)
        {
            fEdSvc.CloseDropDown();
        }

        protected abstract void FillInList(ITypeDescriptorContext pContext, IServiceProvider pProvider, ListBox pListBox);
    }
} 
