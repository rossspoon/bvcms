using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsWeb.MobileAPI
{
	public class MobileTaskBox
	{
		public int id = 0;
		public string name = "";

		public MobileTaskBox populate(CmsData.TaskList tl)
		{
			id = tl.Id;
			name = tl.Name;

			return this;
		}
	}
}