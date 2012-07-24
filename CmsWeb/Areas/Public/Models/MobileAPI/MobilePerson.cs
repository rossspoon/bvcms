using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using CmsData;

namespace CmsWeb.MobileAPI
{
	// <?xml version="1.0" encoding="utf-8"?>
	// <People count="1">
	// <Person peopleid="149" name="Steven Yarbrough" first="Steven" last="Yarbrough" address="4074 Fir Hill Dr E"
	// citystatezip="Lakeland, TN 38002" zip="38002" age="36" birthdate="9/27/1975"
	// homephone="" cellphone="  901-481-3443" workphone="" memberstatus="Just Added" email="steven@bvcms.com" haspicture="0" />
	// </People>

	// Test Class to compare JSON results to current API
	// IMPORTANT: Any class that will be converted to JSON needs default values or get/set methods or [DataMember] tags on each field
	public class MobilePerson
	{
		public int id = 0;

		public string first = "";
		public string last = "";
		public string address1 = "";
		public string address2 = "";
		public string city = "";
		public string state = "";
		public string zip = "";

		public int age = 0;
		public string birthday = "";

		public string home = "";
		public string work = "";
		public string cell = "";
		public string email1 = "";
		public string email2 = "";

		public int status = 0;

		public int picture = 0;
		public int deceased = 0;

		public MobilePerson populate( CmsData.Person p )
		{
			id = p.PeopleId;

			first = p.FirstName ?? "";
			last = p.LastName ?? "";
			address1 = p.AddressLineOne ?? "";
			address2 = p.AddressLineTwo ?? "";
			city = p.CityName ?? "";
			state = p.StateCode ?? "";
			zip = p.ZipCode ?? "";

			age = p.Age ?? 0;
			birthday = ""; // TODO: Fix this

			home = p.HomePhone ?? "";
			work = p.WorkPhone ?? "";
			cell = p.CellPhone ?? "";
			email1 = p.EmailAddress ?? "";
			email2 = p.EmailAddress2 ?? "";

			status = p.MemberStatusId;

			picture = p.PictureId ?? 0;
			deceased = ( p.Deceased ? 1 : 0 );

			return this;
		}
	}
}