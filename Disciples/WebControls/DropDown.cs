using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace CustomControls
{
    public class DropDownCC : DropDownList, IPostBackDataHandler
    {
        public DropDownCC() : base() { }

        public event EventHandler PostDataLoaded;

        protected override void OnInit(EventArgs e)
        {
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }
        /* the following two methods eliminate the problem
         * where the DropDownList requires ViewState to be enabled
         * in order for the SelectedIndex_Changed event to fire.
         * The previous state is now kept in ControlState instead of Viewstate
         */
        protected override void LoadControlState(object savedState)
        {
            object[] controlState = (object[])savedState;
            base.LoadControlState(controlState[0]);
            base.SelectedIndex = (int)controlState[1];
        }
        protected override object SaveControlState()
        {
            object[] controlState = new object[2];
            controlState[0] = base.SaveControlState();
            controlState[1] = base.SelectedIndex;
            return controlState;
        }
        protected override bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            bool changed = base.LoadPostData(postDataKey, postCollection);
            if(PostDataLoaded!=null)
                PostDataLoaded(this, new EventArgs());
            return changed;
        }
    }
}
