using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace CmsWeb.Code
{
	public class Word2Pdf
	{
		public static void Convert(byte[] data, string name)
		{
			var client = new WebClient();
			var coll = new NameValueCollection();
			coll.Add("OutputFileName", "CMSIndividualDir.pdf");
			coll.Add("ApiKey", "111199803");
			client.QueryString.Add(coll);
			var response = client.UploadData("http://do.convertapi.com/word2pdf", data);
			var responseHeaders = client.ResponseHeaders;
			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
			                        responseHeaders["OutputFileName"]);
			File.WriteAllBytes(path, response);
		}
	}
}