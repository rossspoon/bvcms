using System.Text;
using UtilityExtensions;

namespace CmsData.Registration
{
	public class AskHeader : Ask
	{
		public string Label { get; set; }
		public AskHeader() : base("AskHeader") { }
		public static AskHeader Parse(Parser parser)
		{
			var r = new AskHeader();
			parser.GetBool();
			r.Label = parser.GetLabel("Header");
			return r;
		}
		public override void Output(StringBuilder sb)
		{
			Settings.AddValueCk(0, sb, "AskHeader", true);
			if (!Label.HasValue())
				Label = "Header";
			Settings.AddValueCk(1, sb, "Label", Label);
			sb.AppendLine();
		}
	}
}