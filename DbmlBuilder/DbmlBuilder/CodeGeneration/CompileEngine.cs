using System;
using System.Text;
using System.Collections.Specialized;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using DbmlBuilder.Utilities;

namespace DbmlBuilder.CodeGenerator
{
    public class Compiler
    {
        private static readonly Regex regLineFix = new Regex(@"[\r\n]+", RegexOptions.Compiled);

        public void AddTemplate(Template template)
        {
            if (template != null)
            {
                if (templates.Count == 0)
                    References = template.References;
                template.EntryPoint = "Render";
                template.GeneratedRenderType = "Parser" + Templates.Count;
                template.TemplateText =
                Utility.FastReplace(template.TemplateText, "#TEMPLATENUMBER#", Templates.Count.ToString(), StringComparison.InvariantCultureIgnoreCase);
                templates.Add(template);
            }
        }

        private readonly TurboTemplateCollection templates = new TurboTemplateCollection();
        public TurboTemplateCollection Templates
        {
            get { return templates; }
        }

        private CodeDomProvider codeProvider = null;
        private CodeDomProvider CodeProvider
        {
            get
            {
                if (codeProvider == null)
                    codeProvider = CSharpCodeLanguage.CreateCodeProvider();
                return codeProvider;
            }
        }

        private CompilerParameters codeCompilerParameters = null;
        private CompilerParameters CodeCompilerParameters
        {
            get
            {
                if (codeCompilerParameters == null)
                {
                    codeCompilerParameters = new CompilerParameters();
                    codeCompilerParameters.CompilerOptions = "/target:library"; // you can add /optimize
                    codeCompilerParameters.GenerateExecutable = false;
                    codeCompilerParameters.GenerateInMemory = true;
                    codeCompilerParameters.IncludeDebugInformation = false;
                    codeCompilerParameters.ReferencedAssemblies.Add("mscorlib.dll");
                    foreach (string s in References)
                        codeCompilerParameters.ReferencedAssemblies.Add(s);
                }
                return codeCompilerParameters;
            }
        }

        public void Run()
        {
            int templateCount = Templates.Count;
            if (templateCount > 0)
            {
                ClearErrMsgs();

                string[] templateArray = new string[templateCount];
                for (int i = 0; i < templateCount; i++)
                {
                    templateArray[i] = Templates[i].TemplateText;
                    //System.IO.File.WriteAllText("c:\\ttt\\f" + i + ".cs", Templates[i].TemplateText);
                }
                Utility.WriteTrace("Compiling assembly...");
                CompilerResults results = CodeProvider.CompileAssemblyFromSource(CodeCompilerParameters, templateArray);
                Utility.WriteTrace("Done!");

                if (results.Errors.Count > 0 || results.CompiledAssembly == null)
                {
                    if (results.Errors.Count > 0)
                        foreach (CompilerError error in results.Errors)
                            LogErrMsgs("Compile Error: " + error.ErrorText);
                    if (results.CompiledAssembly == null)
                    {
                        string errorMessage = "Error generating template code: This usually indicates an error in template itself, such as use of reserved words. Detail: ";
                        Utility.WriteTrace(errorMessage + errMsg);
                        string sMessage = errorMessage + Environment.NewLine + errMsg;
                        throw new Exception(sMessage);
                    }
                    return;
                }

                Utility.WriteTrace("Extracting code from assembly and scrubbing output...");
                CallEntry(results.CompiledAssembly);
                Utility.WriteTrace("Done!");
            }
        }

        private static string ScrubOutput(string result)
        {
            if (!String.IsNullOrEmpty(result))
            {
                //the generator has an issue with adding extra lines. Trim them out

                result = regLineFix.Replace(result, "\r\n");

                result = Utility.FastReplace(result, "}", "}\r\n", StringComparison.InvariantCultureIgnoreCase);
                result = Utility.FastReplace(result, "namespace", "\r\nnamespace", StringComparison.InvariantCulture); //Must be case-sensitive, or it will cause VB End Namespace to wrap

                result = Utility.FastReplace(result, "public class ", "\r\npublic class ", StringComparison.InvariantCulture); //trailing space need to address class names that begin with "class"
                result = Utility.FastReplace(result, "[<]", "<", StringComparison.InvariantCultureIgnoreCase);
                result = Utility.FastReplace(result, "[>]", ">", StringComparison.InvariantCultureIgnoreCase);
            }
            return result;
        }

        internal StringCollection References = new StringCollection();

        private void CallEntry(Assembly assembly)
        {
            int templateCount = Templates.Count;
            Module mod = assembly.GetModules(false)[0];
            for (int i = 0; i < templateCount; i++)
            {
                Type type = mod.GetType(Templates[i].GeneratedRenderType);
                if (type != null)
                {
                    MethodInfo mi = type.GetMethod(Templates[i].EntryPoint, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    if (mi != null)
                    {
                        StringBuilder returnText = new StringBuilder();

                        if (Templates[i].AddUsingBlock)
                        {
                            returnText.Append(CSharpCodeLanguage.DefaultUsingStatements);
                            returnText.Append(Templates[i].CustomUsingBlock);
                        }
                        returnText.Append((string)mi.Invoke(null, null));

                        Templates[i].FinalCode = ScrubOutput(returnText.ToString());
                    }
                }
            }
        }


        internal StringBuilder errMsg = new StringBuilder();
        internal void LogErrMsgs(string customMsg)
        {
            LogErrMsgs(customMsg, null);
        }

        internal void LogErrMsgs(string customMsg, Exception ex)
        {
            errMsg.Append("\r\n").Append(customMsg).Append(Environment.NewLine);

            while (ex != null)
            {
                errMsg.Append("\t").Append(ex.Message).Append(Environment.NewLine);
                ex = ex.InnerException;
            }
        }

        internal void ClearErrMsgs()
        {
            errMsg.Remove(0, errMsg.Length);
        }
    }

}
