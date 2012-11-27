using System.Text;
using UtilityExtensions;

namespace CmsData.Registration
{
	public class AskTickets : Ask
	{
		public string Label { get; set; }
		public AskTickets() : base("AskTickets") { }
		public static AskTickets Parse(Parser parser)
		{
			var r = new AskTickets();
			parser.GetBool();
			r.Label = parser.GetLabel("No. of Items");
			return r;
		}
		public override void Output(StringBuilder sb)
		{
            Settings.AddValueCk(0, sb, "AskTickets", true);
			if (!Label.HasValue())
				Label = "No. of Items";
			Settings.AddValueCk(1, sb, "Label", Label);
			sb.AppendLine();
		}
	}
}