using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomControls
{
    public class CommandDropDownList : DropDownList, IPostBackEventHandler
    {
        private const string STR_CommandArgument = "CommandArgument";
        private const string STR_CommandName = "CommandName";
        private static readonly object EventCommand = new object();

        public CommandDropDownList()
        {
            base.AutoPostBack = true;
        }

        public string CommandArgument
        {
            get { return (string)ViewState[STR_CommandArgument]; }
            set { ViewState[STR_CommandArgument] = value; }
        }

        public string CommandName
        {
            get { return (string)ViewState[STR_CommandName]; }
            set { ViewState[STR_CommandName] = value; }
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            CommandArgument = "0";

            if (SelectedItem != null)
                CommandArgument = SelectedItem.Value;

            RaisePostBackEvent(eventArgument);
        }

        protected virtual void OnCommand(CommandEventArgs e)
        {
            var handler = Events[EventCommand] as CommandEventHandler;
            if (handler != null)
                handler(this, e);
            base.RaiseBubbleEvent(this, e);
        }

        protected virtual void RaisePostBackEvent(string eventArgument)
        {
            if (CausesValidation)
                Page.Validate(ValidationGroup);
            OnCommand(new CommandEventArgs(CommandName, CommandArgument));
        }
    }
}