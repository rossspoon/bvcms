using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;

namespace CustomControls
{
    public class CommandTextBox : TextBox, IPostBackEventHandler
    {
        private static readonly object EventCommand = new object();

        public CommandTextBox()
        {
            AutoPostBack = true;
        }

        public string CommandArgument
        {
            get { return (string)this.ViewState["CommandArgument"]; }
            set { this.ViewState["CommandArgument"] = value; }
        }

        public string CommandName
        {
            get { return (string)this.ViewState["CommandName"]; }
            set { this.ViewState["CommandName"] = value; }
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            this.CommandArgument = base.Text;
            this.RaisePostBackEvent(eventArgument);
        }

        protected virtual void OnCommand(CommandEventArgs e)
        {
            var handler = Events[EventCommand] as CommandEventHandler;
            if (handler != null)
                handler(this, e);
            RaiseBubbleEvent(this, e);
        }

        protected virtual void RaisePostBackEvent(string eventArgument)
        {
            if (CausesValidation)
                Page.Validate(ValidationGroup);
            OnCommand(new CommandEventArgs(CommandName, CommandArgument));
        }
    }
}