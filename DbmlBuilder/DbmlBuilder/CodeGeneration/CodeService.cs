using System;
using System.Collections.Generic;
using System.CodeDom;
using System.Text;
using DbmlBuilder.CodeGenerator;
using System.Text.RegularExpressions;
using System.IO;
using DbmlBuilder.Utilities;
using DbmlBuilder.TableSchema;
using System.Collections.Specialized;

namespace DbmlBuilder
{
    public static class CodeService
    {

        #region Helpers
        public enum TemplateType
        {
            Class,
            WebForm_cs,
            WebForm_aspx,
            ReadOnly,
            TableFunction,
            Context,
        }

        public enum ReplacementVariable
        {
            Table,
            Provider,
            View,
            StoredProcedure
        }
        public class Replacement
        {
            public Replacement(ReplacementVariable variable, string replace)
            {
                Variable = variable;
                ReplaceWith = replace;
            }

            private ReplacementVariable replaceVar;
            public ReplacementVariable Variable
            {
                get { return replaceVar; }
                set { replaceVar = value; }
            }


            private string replaceWith;
            public string ReplaceWith
            {
                get { return replaceWith; }
                set { replaceWith = value; }
            }

        }

        #endregion

        public static Template BuildClassTemplate(string tableName)
        {
                List<Replacement> list = new List<Replacement>();
                list.Add(new Replacement(ReplacementVariable.Table, tableName));

                return PrepareTemplate("Class: " + tableName, TemplateType.Class, list);
        }

        public static Template BuildWebTemplate1(string tableName)
        {
                List<Replacement> list = new List<Replacement>();
                list.Add(new Replacement(ReplacementVariable.Table, tableName));

                return PrepareTemplate("Class: " + tableName, TemplateType.WebForm_aspx, list);
        }
        public static Template BuildWebTemplate2(string tableName)
        {
                List<Replacement> list = new List<Replacement>();
                list.Add(new Replacement(ReplacementVariable.Table, tableName));

                return PrepareTemplate("Class: " + tableName, TemplateType.WebForm_cs, list);
        }

        public static Template BuildViewTemplate(string tableName)
        {

                List<Replacement> list = new List<Replacement>();
                list.Add(new Replacement(ReplacementVariable.View, tableName));

                return PrepareTemplate("View - " + Db.Service.Name + ": " + tableName, TemplateType.ReadOnly, list);
        }
        public static Template BuildContextTemplate()
        {
            List<Replacement> list = new List<Replacement>();
            return PrepareTemplate("Context Class", TemplateType.Context, list);
        }


        static Template PrepareTemplate(string templateName,
            TemplateType templateType,
            IEnumerable<Replacement> settings)
        {
            string templateText = GetTemplateText(templateType);
            //set the provider and tablename
            foreach (Replacement var in settings)
            {
                string replaceHolder = "#" + Enum.GetName(typeof(ReplacementVariable), var.Variable).ToUpper() + "#";
                templateText = Utility.FastReplace(templateText, replaceHolder, var.ReplaceWith, StringComparison.InvariantCultureIgnoreCase);
            }
            Template t = new Template(templateText);
            t.TemplateName = templateName;
            return t;
        }

        static string GetTemplateText(TemplateType t)
        {
            string template = CSharpCodeLanguage.TemplatePrefix;
            string templateText = null;

            switch (t)
            {
                case TemplateType.Class:
                    template += TemplateName.CLASS;
                    break;
                case TemplateType.ReadOnly:
                    template += TemplateName.VIEW;
                    break;
                case TemplateType.Context:
                    template += TemplateName.CONTEXT;
                    break;
                case TemplateType.WebForm_aspx:
                    template += TemplateName.WEBFORM1;
                    break;
                case TemplateType.WebForm_cs:
                    template += TemplateName.WEBFORM2;
                    break;
                default:
                    template += TemplateName.CLASS;
                    break;
            }

            template += FileExtension.DOT_ASPX;

            templateText = Template.LoadTextFromManifest(template);

            if (String.IsNullOrEmpty(templateText))
                throw new Exception("The template \"" + template + "\" is empty or cannot be found.");

            return templateText;
        }
    }
}
