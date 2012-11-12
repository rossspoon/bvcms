using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;

namespace CmsData.Registration
{
	public class AskYesNoQuestions : Ask
	{
		public List<YesNoQuestion> list { get; private set; }

		public AskYesNoQuestions()
			: base("AskYesNoQuestions")
		{
			list = new List<YesNoQuestion>();
		}
		public override void Output(StringBuilder sb)
		{
			if (list.Count == 0)
				return;
			Settings.AddValueNoCk(0, sb, "YesNoQuestions", "");
			foreach (var q in list)
				q.Output(sb);
			sb.AppendLine();
		}
		public static AskYesNoQuestions Parse(Parser parser)
		{
			var ynq = new AskYesNoQuestions();
			parser.lineno++;
			if (parser.curr.indent == 0)
				return ynq;
			var startindent = parser.curr.indent;
			while (parser.curr.indent == startindent)
			{
				var q = YesNoQuestion.Parse(parser, startindent);
				ynq.list.Add(q);
			}
			return ynq;
		}
		public class YesNoQuestion
		{
			public string Name { get; set; }
			public string Question { get; set; }
			public string SmallGroup { get; set; }
			public void Output(StringBuilder sb)
			{
				Settings.AddValueCk(1, sb, Question ?? "need a question here");
				Settings.AddValueNoCk(2, sb, "SmallGroup", SmallGroup ?? Question);
			}
			public static YesNoQuestion Parse(Parser parser, int startindent)
			{
				var q = new YesNoQuestion();
				if (parser.curr.kw != Parser.RegKeywords.None)
					throw parser.GetException("unexpected line");
				q.Question = parser.GetLine();
				if (parser.curr.indent <= startindent)
					throw parser.GetException("Expected SmallGroup indented");
				if (parser.curr.kw != Parser.RegKeywords.SmallGroup)
					throw parser.GetException("Expected SmallGroup keyword");
				if (!parser.curr.value.HasValue())
					throw parser.GetException("Expected SmallGroup value");
				q.SmallGroup = parser.GetString();
				return q;
			}
		}
	}
}
