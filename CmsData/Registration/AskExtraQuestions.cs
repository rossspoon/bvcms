﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;

namespace CmsData.Registration
{
	public class AskExtraQuestions : Ask
	{
		public List<ExtraQuestion> list { get; private set; }

		public AskExtraQuestions()
			: base("AskExtraQuestions")
		{
			list = new List<ExtraQuestion>();
		}
		public override void Output(StringBuilder sb)
		{
			if (list.Count == 0)
				return;
			Settings.AddValueNoCk(0, sb, "ExtraQuestions", "");
			foreach (var q in list)
				q.Output(sb);
			sb.AppendLine();
		}
		public static AskExtraQuestions Parse(Parser parser)
		{
			var eq = new AskExtraQuestions();
			parser.lineno++;
			if (parser.curr.indent == 0)
				return eq;
			var startindent = parser.curr.indent;
			while (parser.curr.indent == startindent)
			{
				var q = ExtraQuestion.Parse(parser, startindent);
				eq.list.Add(q);
			}
			return eq;
		}
		public class ExtraQuestion
		{
			public string Name { get; set; }
			public string Question { get; set; }
			public void Output(StringBuilder sb)
			{
				Settings.AddValueCk(1, sb, Question);
			}
			public static ExtraQuestion Parse(Parser parser, int startindent)
			{
				if (parser.curr.kw != Parser.RegKeywords.None)
					throw parser.GetException("unexpected line");
				return new ExtraQuestion { Question = parser.GetLine() };
			}
		}
	}
}
