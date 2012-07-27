using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Need for JSON Routines
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace CmsWeb.MobileAPI
{
	public static class JSONHelper
	{
		public static string JsonSerializer<T>(T t)
		{
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
			MemoryStream ms = new MemoryStream();
			ser.WriteObject(ms, t);
			string jsonString = Encoding.UTF8.GetString(ms.ToArray());
			ms.Close();
			return jsonString;
		}

		public static T JsonDeserialize<T>(string jsonString)
		{
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
			MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
			T obj = (T)ser.ReadObject(ms);
			return obj;
		}
	}
}