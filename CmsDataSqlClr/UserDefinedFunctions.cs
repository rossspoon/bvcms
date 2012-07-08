using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
	[SqlFunction]
	public static SqlBoolean IsValidEmail(SqlString addr)
	{
		if (addr == null)
			return true;
		var email = addr.Value.Trim();
		if (string.IsNullOrEmpty(email))
			return true;
		var re1 = new Regex(@"^(.*\b(?=\w))\b[A-Z0-9._%+-]+(?<=[^.])@[A-Z0-9.-]+\.[A-Z]{2,4}\b\b(?!\w)$", RegexOptions.IgnoreCase);
		var re2 = new Regex(@"^[A-Z0-9._%+-]+(?<=[^.])@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);
		return re1.IsMatch(email) || re2.IsMatch(email);
	}
}

