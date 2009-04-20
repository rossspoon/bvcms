using System;

namespace BellevueTeachers.Modules
{
    public partial class ResultMessage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Result.Visible = false; // default
        }
        public void ShowSuccess(string message)
        {
            Result.Visible = true;
            Success.Visible = true;
            Fail.Visible = false;
            Message.Text = message + " - " + DateTime.Now.ToString();
        }
        public void ShowFail(string message)
        {
            Result.Visible = true;
            Success.Visible = false;
            Fail.Visible = true;
            Message.Text = message + " - " + DateTime.Now.ToString();
        }
    }
}