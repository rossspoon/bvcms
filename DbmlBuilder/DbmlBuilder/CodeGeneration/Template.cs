using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;

namespace DbmlBuilder.CodeGenerator
{

    [Serializable]
    public class TurboTemplateCollection : List<Template>
    {
    }

    [Serializable]
    public class Template
    {
        private const string HEADER = @"using System;
            using System.Text.RegularExpressions;
            using System.Collections;
            using System.IO;
            using System.Text;
            using DbmlBuilder;
            using DbmlBuilder.CodeGenerator;
            using DbmlBuilder.TableSchema;
            using System.Data;
            using System.Configuration;
            using DbmlBuilder.Utilities;
            
	        public class Parser#TEMPLATENUMBER#
	        {
		        public static string Render()
		        {
			        MemoryStream mStream = new MemoryStream();
			        StreamWriter writer = new StreamWriter(mStream, System.Text.Encoding.UTF8);
                    writer.AutoFlush = false;

                ";

        private const string FOOTER = @"

                    StreamReader sr = new StreamReader(mStream); 
			        writer.Flush();
			        mStream.Position = 0;
			        return sr.ReadToEnd();
		        }
	        }";

        private static StringCollection references = null;
        public StringCollection References
        {
            get
            {
                if (references == null)
                {
                    references = new StringCollection();
                    references.Add("System.dll");
                    references.Add("System.Data.dll");
                    references.Add("System.Drawing.dll");
                    references.Add("System.Xml.dll");
                    references.Add("System.Windows.Forms.dll");
                    references.Add("System.Configuration.dll");
                    references.Add(typeof(Template).Assembly.Location);
                }
                return references;
            }
        }

        private bool addUsingBlock = true;
        public bool AddUsingBlock
        {
            get { return addUsingBlock; }
            set { addUsingBlock = value; }
        }

        private readonly string customUsingBlock = String.Empty;
        public string CustomUsingBlock
        {
            get { return customUsingBlock; }
        }

        private string entryPoint = "Render";
        public string EntryPoint
        {
            get { return entryPoint; }
            set { entryPoint = value; }
        }

        private string generatedRenderType = "Parser";
        public string GeneratedRenderType
        {
            get { return generatedRenderType; }
            set { generatedRenderType = value; }
        }

        private string templateText = null;
        public string TemplateText
        {
            get { return templateText; }
            set { templateText = value; }
        }

        private string templateName = String.Empty;
        public string TemplateName
        {
            get { return templateName; }
            set { templateName = value; }
        }

        private string finalCode = null;
        public string FinalCode
        {
            get { return finalCode; }
            set { finalCode = value; }
        }

        private string outputPath = null;
        public string OutputPath
        {
            get { return outputPath; }
            set { outputPath = value; }
        }

        public Template(string templateInputText)
        {
            templateText = CleanTemplate(templateInputText + "<%%>");
        }


        private static string CleanTemplate(string templateInputText)
        {
            //Modify this part if you want to read the <%@ tags also you can implement your own tags here.
            templateInputText = Regex.Replace(templateInputText, "(?i)<%@\\s*Property.*?%>", string.Empty);
            templateInputText = Regex.Replace(templateInputText, "(?i)<%@\\s*Assembly.*?%>", string.Empty);
            templateInputText = Regex.Replace(templateInputText, "(?i)<%@\\s*Import.*?%>", string.Empty);
            templateInputText = Regex.Replace(templateInputText, "(?i)<%@\\s*CodeTemplate.*?%>", string.Empty);

            templateInputText = ParseTemplate(templateInputText);
            templateInputText = Regex.Replace(templateInputText, @"<%=.*?%>", new MatchEvaluator(CleanCalls), RegexOptions.Singleline);
            templateInputText = Regex.Replace(templateInputText, @"<%%>", string.Empty, RegexOptions.Singleline);
            templateInputText = Regex.Replace(templateInputText, @"<%[^=|@].*?%>", new MatchEvaluator(CleanCodeTags), RegexOptions.Singleline);

            //strip the directive
            templateInputText = Regex.Replace(templateInputText, "(?i)<%@\\s*Page.*?%>", string.Empty);
            StringBuilder sb = new StringBuilder(HEADER);
            sb.Append(templateInputText);
            sb.Append(FOOTER);
            return sb.ToString().Trim();
        }

        private static string CleanCalls(Match m)
        {
            string x = m.ToString();
            x = Regex.Replace(x, "<%=", "\t\t\twriter.Write(");
            x = Regex.Replace(x, "%>", ");");
            return x;
        }

        private static string CleanCodeTags(Match m)
        {
            string x = m.ToString();
            x = x.Substring(2, x.Length - 4);
            x = "\t\t\t" + x;
            return x;
        }

        private static string ParseTemplate(string templateInputText)
        {
            if (String.IsNullOrEmpty(templateInputText))
                return String.Empty;

            MemoryStream mStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(mStream, Encoding.UTF8);

            int lnLast = 0;
            int lnAt = templateInputText.IndexOf("<%", 0);
            if (lnAt == -1)
                return templateInputText;

            while (lnAt > -1)
            {
                if (lnAt > -1)
                    writer.Write("\t\t\twriter.Write(@\"" + templateInputText.Substring(lnLast, lnAt - lnLast).Replace("\"", "\"\"") + "\" );");

                int lnAt2 = templateInputText.IndexOf("%>", lnAt);
                if (lnAt2 < 0)
                    break;

                writer.Write(templateInputText.Substring(lnAt, lnAt2 - lnAt + 2));

                lnLast = lnAt2 + 2;
                lnAt = templateInputText.IndexOf("<%", lnLast);
                if (lnAt < 0)
                    writer.Write("\t\t\twriter.Write(@\"" + templateInputText.Substring(lnLast, templateInputText.Length - lnLast).Replace("\"", "\"\"") + "\" );");
            }

            writer.Flush();
            mStream.Position = 0;
            StreamReader sr = new StreamReader(mStream);
            string returndata = sr.ReadToEnd();
            sr.Close();
            mStream.Close();
            writer.Close();
            return returndata;
        }


        public static string LoadTextFromManifest(string templateFileName)
        {
            string templateText = null;
            Assembly asm = Assembly.GetExecutingAssembly();
            Stream stream = asm.GetManifestResourceStream("DbmlBuilder.CodeGeneration.Templates." + templateFileName);
            if (stream != null)
            {
                StreamReader sReader = new StreamReader(stream);
                templateText = sReader.ReadToEnd();
                sReader.Close();
            }
            return templateText;
        }
    }
}
