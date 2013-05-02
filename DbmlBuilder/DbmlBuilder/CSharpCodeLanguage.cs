using System;
using System.CodeDom.Compiler;
using System.Data;
using Microsoft.CSharp;
using DbmlBuilder.Utilities;

namespace DbmlBuilder
{
	public static class CSharpCodeLanguage
	{
		#region Constants & enumerations

		private static readonly string[] keywords = {
			// keywords
			"abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class",
			"const", "continue", "date", "datetime", "decimal", "default", "delegate", "do", "double", "else",
			"enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto",
			"if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new",
			"null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly",
			"ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct",
			"switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort",
			"using", "virtual", "volatile", "void", "while",
			// contextual keywords
			"get", "partial", "set", "value", "where", "yield"
		};


		#endregion

		#region Properties

        public static string DefaultUsingStatements
        {
            get
            {
                return @"using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
";
            }
        }

		public static string TemplatePrefix
		{
			get { return "CS_"; }
		}

		#endregion

		#region Methods

		public static CodeDomProvider CreateCodeProvider()
		{
			return new CSharpCodeProvider();
		}

		public static string GetVariableType(DbType dbType, bool isNullableColumn)
		{
			string variableType = GetVariableType(dbType);
			if (isNullableColumn && Utility.IsNullableDbType(dbType))
				variableType = variableType + "?";
			return variableType;
		}
		#endregion

		#region Private methods

		private static string GetVariableType(DbType dbType)
		{
			switch (dbType) {
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.String:
				case DbType.StringFixedLength: return "string";

				case DbType.Binary:		return "byte[]";
				case DbType.Boolean:	return "bool";
				case DbType.Byte:		return "byte";

				case DbType.Currency:
				case DbType.Decimal:
				case DbType.VarNumeric:	return "decimal";

				case DbType.Date:
				case DbType.DateTime:	return "DateTime";

				case DbType.Double:		return "double";
				case DbType.Guid:		return "Guid";
				case DbType.Int16:		return "short";
				case DbType.Int32:		return "int";
				case DbType.Int64:		return "long";
				case DbType.Object:		return "object";
				case DbType.SByte:		return "sbyte";
				case DbType.Single:		return "float";
				case DbType.Time:		return "TimeSpan";
				case DbType.UInt16:		return "ushort";
				case DbType.UInt32:		return "uint";
				case DbType.UInt64:		return "ulong";

				default:				return "string";
			}
		}

		#endregion
	}
}
