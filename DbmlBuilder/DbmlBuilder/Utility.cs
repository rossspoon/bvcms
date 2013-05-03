using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace DbmlBuilder.Utilities
{
    public static class Utility
    {
        public static SqlDataReader GetReader(this SqlCommand cmd)
        {
            var automaticConnectionScope = new AutomaticConnectionScope();
            cmd.Connection = (SqlConnection)automaticConnectionScope.Connection;
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            return rdr;
        }
        public static void WriteTrace(string message)
        {
            if (HttpContext.Current != null && HttpContext.Current.Trace.IsEnabled)
            {
                message = DateTime.Now.ToString("H:mm:ss:fff") + " > " + message;
                HttpContext.Current.Trace.Write("DbmlBuilder", message);
            }
            else if (System.Diagnostics.Debug.Listeners.Count > 0)
            {
                message = DateTime.Now.ToString("H:mm:ss:fff") + " > " + message;
                System.Diagnostics.Debug.WriteLine(message, "DbmlBuilder");
                Console.WriteLine(message);
            }
        }

        #region Tests

        public static bool IsMatch(string stringA, string stringB)
        {
            return String.Equals(stringA, stringB, StringComparison.InvariantCultureIgnoreCase);
        }
        public static string StripWhitespace(string inputString)
        {
            if (!String.IsNullOrEmpty(inputString))
                return Regex.Replace(inputString, @"\s", String.Empty);
            return inputString;
        }

        public static bool IsStringNumeric(string str)
        {
            double result;
            return (double.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.CurrentInfo, out result));
        }

        public static bool IsNullableDbType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.Binary:
                case DbType.Object:
                case DbType.String:
                case DbType.StringFixedLength:
                    return false;
                default:
                    return true;
            }
        }

        #endregion

        #region Types

        // TODO: Refactor out
        public static string GetVariableType(DbType dbType, bool isNullableColumn)
        {
            return CSharpCodeLanguage.GetVariableType(dbType, isNullableColumn);
        }

        public static string PrefixParameter(string parameter)
        {
            string prefix = Db.Service.GetParameterPrefix();
            if (!parameter.StartsWith(prefix))
                parameter = prefix + parameter;
            return parameter;
        }

        #endregion

        public static string ParseCamelToProper(string sIn)
        {
            //No transformation if string is alread all caps
            if (Regex.IsMatch(sIn, @"^[A-Z]+$"))
                return sIn;
            char[] letters = sIn.ToCharArray();
            StringBuilder sOut = new StringBuilder();
            int index = 0;

            if (sIn.Contains("ID"))
            {
                //just upper the first letter
                sOut.Append(letters[0]);
                sOut.Append(sIn.Substring(1, sIn.Length - 1));
            }
            else
            {
                foreach (char c in letters)
                {
                    if (index == 0)
                    {
                        sOut.Append(" ");
                        sOut.Append(c.ToString().ToUpper());
                    }
                    else if (Char.IsUpper(c))
                    {
                        //it's uppercase, add a space
                        sOut.Append(" ");
                        sOut.Append(c);
                    }
                    else
                        sOut.Append(c);
                    index++;
                }
            }
            return sOut.ToString().Trim();
        }

        public static string GetProperName(string sIn)
        {
            string propertyName = sIn;
            propertyName = Inflector.ToPascalCase(propertyName);
            if (propertyName.EndsWith("TypeCode"))
                propertyName = propertyName.Substring(0, propertyName.Length - 4);
            return propertyName;
        }

        public static string PluralToSingular(string sIn)
        {
            return Inflector.MakeSingular(sIn);
        }

        public static string SingularToPlural(string sIn)
        {
            return Inflector.MakePlural(sIn);
        }

        public static string KeyWordCheck(string word, string table, string appendWith)
        {
            string newWord = word + appendWith;

            if (word == "Schema")
                newWord = word + appendWith;

            //Can't have a property with same name as class.
            if (word == table)
                return newWord;

            switch (word.ToLower())
            {
                // C# keywords
                case "abstract": return newWord;
                case "as": return newWord;
                case "base": return newWord;
                case "bool": return newWord;
                case "break": return newWord;
                case "byte": return newWord;
                case "case": return newWord;
                case "catch": return newWord;
                case "char": return newWord;
                case "checked": return newWord;
                case "class": return newWord;
                case "const": return newWord;
                case "continue": return newWord;
                case "date": return newWord;
                case "datetime": return newWord;
                case "decimal": return newWord;
                case "default": return newWord;
                case "delegate": return newWord;
                case "do": return newWord;
                case "double": return newWord;
                case "else": return newWord;
                case "enum": return newWord;
                case "event": return newWord;
                case "explicit": return newWord;
                case "extern": return newWord;
                case "false": return newWord;
                case "finally": return newWord;
                case "fixed": return newWord;
                case "float": return newWord;
                case "for": return newWord;
                case "foreach": return newWord;
                case "goto": return newWord;
                case "if": return newWord;
                case "implicit": return newWord;
                case "in": return newWord;
                case "int": return newWord;
                case "interface": return newWord;
                case "internal": return newWord;
                case "is": return newWord;
                case "lock": return newWord;
                case "long": return newWord;
                case "namespace": return newWord;
                case "new": return newWord;
                case "null": return newWord;
                case "object": return newWord;
                case "operator": return newWord;
                case "out": return newWord;
                case "override": return newWord;
                case "params": return newWord;
                case "private": return newWord;
                case "protected": return newWord;
                case "public": return newWord;
                case "readonly": return newWord;
                case "ref": return newWord;
                case "return": return newWord;
                case "sbyte": return newWord;
                case "sealed": return newWord;
                case "short": return newWord;
                case "sizeof": return newWord;
                case "stackalloc": return newWord;
                case "static": return newWord;
                case "string": return newWord;
                case "struct": return newWord;
                case "switch": return newWord;
                case "this": return newWord;
                case "throw": return newWord;
                case "true": return newWord;
                case "try": return newWord;
                case "typeof": return newWord;
                case "uint": return newWord;
                case "ulong": return newWord;
                case "unchecked": return newWord;
                case "unsafe": return newWord;
                case "ushort": return newWord;
                case "using": return newWord;
                case "virtual": return newWord;
                case "volatile": return newWord;
                case "void": return newWord;
                case "while": return newWord;

                // C# contextual keywords
                case "get": return newWord;
                case "partial": return newWord;
                case "set": return newWord;
                case "value": return newWord;
                case "where": return newWord;
                case "yield": return newWord;

                // CMSBuilder keywords
                case "schema": return newWord;

                default: return word;
            }
        }

        public static string KeyWordCheck(string word, string table)
        {
            string appendWith = "X";
            return KeyWordCheck(word, table, appendWith);
        }

        public static string FastReplace(string original, string pattern, string replacement, StringComparison comparisonType)
        {
            if (original == null)
                return null;

            if (String.IsNullOrEmpty(pattern))
                return original;

            int lenPattern = pattern.Length;
            int idxPattern = -1;
            int idxLast = 0;

            StringBuilder result = new StringBuilder();

            while (true)
            {
                idxPattern = original.IndexOf(pattern, idxPattern + 1, comparisonType);

                if (idxPattern < 0)
                {
                    result.Append(original, idxLast, original.Length - idxLast);
                    break;
                }

                result.Append(original, idxLast, idxPattern - idxLast);
                result.Append(replacement);

                idxLast = idxPattern + lenPattern;
            }

            return result.ToString();
        }

        public static string StripText(string inputString, string stripString)
        {
            if (!String.IsNullOrEmpty(stripString))
            {
                string[] replace = stripString.Split(new char[] { ',' });
                for (int i = 0; i < replace.Length; i++)
                    if (!String.IsNullOrEmpty(inputString))
                        inputString = Regex.Replace(inputString, replace[i], String.Empty);
            }
            return inputString;
        }

        public static string GetParameterName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return string.Empty;

            string newName = name;
            newName = newName.Replace(" ", "").Replace("_", "").Trim();
            newName = GetProperName(newName);
            newName = IsStringNumeric(newName) ? "_" + newName : newName;
            newName = StripNonAlphaNumeric(newName);
            newName = newName.Replace("@", String.Empty);
            newName = newName.Trim();
            return KeyWordCheck(newName, String.Empty);
        }

        public static string StripNonAlphaNumeric(string sIn)
        {
            StringBuilder sb = new StringBuilder(sIn);
            char c = " ".ToCharArray()[0];
            string stripList = ".'?\\/><$!@%^*&+,;:\"{}[]|-#";

            for (int i = 0; i < stripList.Length; i++)
                sb.Replace(stripList[i], c);
            sb.Replace(" ", String.Empty);
            return sb.ToString();
        }

        public static string[] Split(string list)
        {
            string[] find;
            try
            {
                find = list.Split(new string[] { ", ", "," }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch
            {
                find = new string[] { string.Empty };
            }
            return find;
        }

    }
}
