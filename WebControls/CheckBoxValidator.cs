using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Configuration;
using UtilityExtensions;

namespace CustomControls
{
    public class CheckBoxValidator : BaseValidator
    {
        [Description("Whether the CheckBox must be checked or unchecked to be considered valid.")]
        public bool MustBeChecked
        {
            get
            {
                object o = ViewState["MustBeChecked"];
                if (o == null)
                    return true;
                else
                    return (bool)o;
            }
            set
            {
                ViewState["MustBeChecked"] = value;
            }
        }

        private CheckBox _ctrlToValidate = null;
        protected CheckBox CheckBoxToValidate
        {
            get
            {
                if (_ctrlToValidate == null)
                    _ctrlToValidate = FindControl(this.ControlToValidate) as CheckBox;

                return _ctrlToValidate;
            }
        }

        protected override bool ControlPropertiesValid()
        {
            // Make sure ControlToValidate is set
            if (this.ControlToValidate.Length == 0)
                throw new HttpException("The ControlToValidate property of '{0}' cannot be blank.".Fmt(this.ID));

            // Ensure that the control being validated is a CheckBox
            if (CheckBoxToValidate == null)
                throw new HttpException(string.Format("The CheckBoxValidator can only validate controls of type CheckBox."));

            return true;    // if we reach here, everything checks out
        }

        protected override bool EvaluateIsValid()
        {
            // Make sure that the CheckBox is set as directed by MustBeChecked
            return CheckBoxToValidate.Checked == MustBeChecked;
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            // Add the client-side code (if needed)
            if (this.RenderUplevel)
            {
                // Indicate the mustBeChecked value and the client-side function to used for evaluation
                // Use AddAttribute if Helpers.EnableLegacyRendering is true; otherwise, use expando attributes
                if (EnableLegacyRendering())
                {
                    writer.AddAttribute("evaluationfunction", "CheckBoxValidatorEvaluateIsValid", false);
                    writer.AddAttribute("mustBeChecked", MustBeChecked ? "true" : "false", false);
                }
                else
                {
                    this.Page.ClientScript.RegisterExpandoAttribute(this.ClientID, "evaluationfunction", "CheckBoxValidatorEvaluateIsValid", false);
                    this.Page.ClientScript.RegisterExpandoAttribute(this.ClientID, "mustBeChecked", MustBeChecked ? "true" : "false", false);
                }
            }
        }
        public static bool EnableLegacyRendering()
        {
            Configuration cfg = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            XhtmlConformanceSection xhtmlSection = (XhtmlConformanceSection)cfg.GetSection("system.web/xhtmlConformance");

            return xhtmlSection.Mode == XhtmlConformanceMode.Legacy;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.RenderUplevel && this.Page != null && !this.Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "CCValidate"))
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CCValidate", "function CheckBoxValidatorEvaluateIsValid(val){var c=document.getElementById(val.controltovalidate);var mustBeChecked=Boolean(val.mustBeChecked);return c.checked==mustBeChecked;}", true);
            //this.Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "CCValidate", this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "CustomControls.Validate.js"));
        }
    }
}
