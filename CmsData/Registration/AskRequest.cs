using System.Text;
using UtilityExtensions;

namespace CmsData.Registration
{
	public class AskRequest : Ask
	{
		public string Label { get; set; }
		public AskRequest() : base("AskRequest") { }
		public static AskRequest Parse(Parser parser)
		{
			var r = new AskRequest();
			parser.GetBool();
			r.Label = parser.GetLabel("Request");
			return r;
		}
		public override void Output(StringBuilder sb)
		{
			Settings.AddValueCk(0, sb, "AskRequest", true);
			if (!Label.HasValue())
				Label = "Request";
			Settings.AddValueCk(1, sb, "Label", Label);
			sb.AppendLine();
		}
	}
}