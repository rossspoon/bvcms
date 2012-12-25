using System.Text;
using UtilityExtensions;

namespace CmsData.Registration
{
	public class AskInstruction : Ask
	{
		public string Label { get; set; }
		public AskInstruction() : base("AskInstruction") { }
		public static AskInstruction Parse(Parser parser)
		{
			var r = new AskInstruction();
			parser.GetBool();
			r.Label = parser.GetLabel("Instruction");
			return r;
		}
		public override void Output(StringBuilder sb)
		{
			Settings.AddValueCk(0, sb, "AskInstruction", true);
			if (!Label.HasValue())
				Label = "Instruction";
			Settings.AddValueCk(1, sb, "Label", Label);
			sb.AppendLine();
		}
	}
}