using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using CmsData;
using LumenWorks.Framework.IO.Csv;
using UtilityExtensions;

namespace CmsWeb.Models
{
	public class UploadPeopleModel
	{
		private Dictionary<string, int> names;
		private StringBuilder psb;
		private StringBuilder fsb;
		private CMSDataContext Db;
		private int PeopleId;

		public UploadPeopleModel(CMSDataContext Db, int PeopleId)
		{
			this.Db = Db;
			this.PeopleId = PeopleId;
		}

		private void UpdateField(Family f, string[] a, string prop, string s)
		{
			if (names.ContainsKey(s))
				if (a[names[s]].HasValue())
					f.UpdateValue(fsb, prop, a[names[s]]);
		}

		private void UpdateField(Person p, string[] a, string prop, string s)
		{
			if (names.ContainsKey(s))
				if (a[names[s]].HasValue())
					p.UpdateValue(psb, prop, a[names[s]]);
		}

		private void UpdateField(Person p, string[] a, string prop, string s, object value)
		{
			if (names.ContainsKey(s))
				if (a[names[s]].HasValue())
					p.UpdateValue(psb, prop, value);
		}

		private string GetDigits(string[] a, string s)
		{
			if (names.ContainsKey(s))
				if (a[names[s]].HasValue())
					return a[names[s]].GetDigits();
			return "";
		}

		private int Gender(string[] a)
		{
			if (names.ContainsKey("Gender"))
				if (a[names["Gender"]].HasValue())
				{
					var v = a[names["Gender"]].TrimEnd();
					switch (v)
					{
						case "Male":
						case "M":
							return 1;
						case "Female":
						case "F":
							return 2;
					}
				}
			return 0;
		}

		private int Marital(string i, string[] a)
		{
			if (names.ContainsKey(i))
				if (a[names[i]].HasValue())
				{
					var v = a[names[i]].TrimEnd();
					switch (v)
					{
						case "Married":
						case "M":
							return 20;
						case "Single":
						case "S":
							return 10;
						case "Widowed":
						case "W":
							return 50;
						case "Divorced":
						case "D":
							return 40;
						case "Separated":
							return 30;
					}
				}
			return 0;
		}

		private int Position(string[] a)
		{
			if (names.ContainsKey("Position"))
				if (a[names["Position"]].HasValue())
				{
					var v = a[names["Position"]].TrimEnd();
					switch (v)
					{
						case "Primary":
							return 10;
						case "Secondary":
							return 20;
						case "Child":
							return 30;
					}
				}
			return 10;
		}

		public bool DoUpload(string text, bool testing = false)
		{
			var rt = Db.UploadPeopleRuns.OrderByDescending(mm => mm.Id).First();
			var csv = new CsvReader(new StringReader(text), false, '\t');
			var list = csv.ToList();

			var list0 = list.First().ToList();
			names = list0.ToDictionary(i => i.TrimEnd(),
									   i => list0.FindIndex(s => s == i));

			if (names.ContainsKey("Campus"))
			{
				var campuslist = (from li in list.Skip(1)
								  where li.Length == names.Count
								  group li by li[names["Campus"]]
									  into campus
									  select campus.Key).ToList();
				var dbc = from c in campuslist
						  join cp in Db.Campus on c equals cp.Description into j
						  from cp in j.DefaultIfEmpty()
						  select new { cp, c };
				var clist = dbc.ToList();
				var maxcampusid = Db.Campus.Max(c => c.Id);
				foreach (var i in clist)
					if (i.cp == null)
					{
						var cp = new Campu { Description = i.c, Id = ++maxcampusid };
						if (!testing)
							Db.Campus.InsertOnSubmit(cp);
					}
			}
			if (!testing)
				Db.SubmitChanges();
			var campuses = Db.Campus.ToDictionary(cp => cp.Description, cp => cp.Id);

			var q = (from li in list.Skip(1)
					 where li.Length == names.Count
					 group li by li[names["FamilyId"]]
						 into fam
						 select fam).ToList();
			rt.Count = q.Sum(ff => ff.Count());

			var standardnames = new List<string>
			                    	{
			                    		"FamilyId",
			                    		"Title",
			                    		"First",
			                    		"Last",
			                    		"GoesBy",
			                    		"AltName",
			                    		"Gender",
			                    		"Married",
			                    		"Marital",
			                    		"MaidenName",
			                    		"Address",
			                    		"Address2",
			                    		"City",
			                    		"State",
			                    		"Zip",
			                    		"Position",
			                    		"Birthday",
			                    		"CellPhone",
			                    		"HomePhone",
			                    		"WorkPhone",
			                    		"Email",
			                    		"Email2",
			                    		"Suffix",
			                    		"Middle",
			                    		"JoinDate",
			                    		"DropDate",
			                    		"BaptismDate",
			                    		"WeddingDate",
			                    		"MemberStatus",
										"Employer",
										"Occupation",
			                    	};

			foreach (var fam in q)
			{
				Family f = null;

				foreach (var a in fam)
				{
					var first = a[names["First"]];
					var last = a[names["Last"]];
					DateTime dt;
					DateTime? dob = null;
					if (names.ContainsKey("Birthday"))
						if (DateTime.TryParse(a[names["Birthday"]], out dt))
						{
							dob = dt;
							if (dob.Value < SqlDateTime.MinValue)
								dob = null;
						}
					string email = null;
					string cell = null;
					string homephone = null;
					if (names.ContainsKey("Email"))
						email = a[names["Email"]].Trim();
					if (names.ContainsKey("CellPhone"))
						cell = a[names["CellPhone"]].GetDigits();
					if (names.ContainsKey("HomePhone"))
						homephone = a[names["HomePhone"]].GetDigits();
					Person p = null;
					var pid = Db.FindPerson3(first, last, dob, email, cell, homephone, null).FirstOrDefault();
					if (pid != null) // found
					{
						p = Db.LoadPersonById(pid.PeopleId.Value);
						psb = new StringBuilder();
						fsb = new StringBuilder();

						UpdateField(p, a, "TitleCode", "Title");
						UpdateField(p, a, "FirstName", "First");
						UpdateField(p, a, "NickName", "GoesBy");
						UpdateField(p, a, "LastName", "Last");
						UpdateField(p, a, "EmailAddress", "Email");
						UpdateField(p, a, "EmailAddress2", "Email2");
						UpdateField(p, a, "DOB", "Birthday");
						UpdateField(p, a, "AltName", "AltName");
						UpdateField(p, a, "SuffixCode", "Suffix");
						UpdateField(p, a, "MiddleName", "Middle");

						UpdateField(p, a, "CellPhone", "CellPhone", GetDigits(a, "CellPhone"));
						UpdateField(p, a, "WorkPhone", "WorkPhone", GetDigits(a, "WorkPhone"));
						UpdateField(p, a, "GenderId", "Gender", Gender(a));
						UpdateField(p, a, "MaritalStatusId", "Married", Marital("Married", a));
						UpdateField(p, a, "MaritalStatusId", "Marital", Marital("Marital", a));
						UpdateField(p, a, "PositionInFamilyId", "Position", Position(a));
						if (names.ContainsKey("Campus"))
							UpdateField(p, a, "CampusId", "Campus", campuses[a[names["Campus"]]]);

						UpdateField(p.Family, a, "AddressLineOne", "Address");
						UpdateField(p.Family, a, "AddressLineTwo", "Address2");
						UpdateField(p.Family, a, "CityName", "City");
						UpdateField(p.Family, a, "StateCode", "State");
						UpdateField(p.Family, a, "ZipCode", "Zip");

						//UpdateField(p, a, "AddressLineOne", "Address");
						//UpdateField(p, a, "AddressLineTwo", "Address2");
						//UpdateField(p, a, "CityName", "City");
						//UpdateField(p, a, "StateCode", "State");
						//UpdateField(p, a, "ZipCode", "Zip");

						if (!testing)
						{
							p.LogChanges(Db, psb, PeopleId);
							p.Family.LogChanges(Db, fsb, p.PeopleId, PeopleId);
							Db.SubmitChanges();
						}
					}
					else // new person
					{
						if (f == null || !a[names["FamilyId"]].HasValue())
						{
							f = new Family();
							if (names.ContainsKey("Address"))
								f.AddressLineOne = a[names["Address"]];
							if (names.ContainsKey("Address2"))
								f.AddressLineTwo = a[names["Address2"]];
							if (names.ContainsKey("City"))
								f.CityName = a[names["City"]];
							if (names.ContainsKey("State"))
								f.StateCode = a[names["State"]];
							if (names.ContainsKey("Zip"))
								f.ZipCode = a[names["Zip"]];
							if (names.ContainsKey("HomePhone"))
								f.HomePhone = a[names["HomePhone"]].GetDigits();
							Db.Families.InsertOnSubmit(f);
							if (!testing)
							{
								Db.SubmitChanges();
							}
						}

						string goesby = null;
						if (names.ContainsKey("GoesBy"))
							goesby = a[names["GoesBy"]];
						p = Person.Add(Db, false, f, 10, null,
									   a[names["First"]],
									   goesby,
									   a[names["Last"]],
									   dob.FormatDate(),
									   0, 0, 0, null, testing);
						p.FixTitle();
						if (names.ContainsKey("AltName"))
							p.AltName = a[names["AltName"]];

						if (names.ContainsKey("Suffix"))
							p.SuffixCode = a[names["Suffix"]];
						if (names.ContainsKey("Middle"))
							p.MiddleName = a[names["Middle"]];
						if (names.ContainsKey("MaidenName"))
							p.MaidenName = a[names["MaidenName"]];

						if (names.ContainsKey("Employer"))
							p.EmployerOther = a[names["Employer"]];
						if (names.ContainsKey("Occupation"))
							p.OccupationOther = a[names["Occupation"]];

						if (names.ContainsKey("CellPhone"))
							p.CellPhone = a[names["CellPhone"]].GetDigits();
						if (names.ContainsKey("WorkPhone"))
							p.WorkPhone = a[names["WorkPhone"]].GetDigits();
						if (names.ContainsKey("Email"))
							p.EmailAddress = a[names["Email"]].Trim();
						if (names.ContainsKey("Email2"))
							p.EmailAddress2 = a[names["Email2"]].Trim();
						if (names.ContainsKey("Gender"))
							p.GenderId = Gender(a);
						if (names.ContainsKey("Married"))
							p.MaritalStatusId = Marital("Married", a);
						if (names.ContainsKey("Marital"))
							p.MaritalStatusId = Marital("Marital", a);
						if (names.ContainsKey("WeddingDate"))
							p.WeddingDate = a[names["WeddingDate"]].ToDate();
						if (names.ContainsKey("JoinDate"))
							p.JoinDate = a[names["JoinDate"]].ToDate();
						if (names.ContainsKey("DropDate"))
							p.DropDate = a[names["DropDate"]].ToDate();
						if (names.ContainsKey("BaptismDate"))
							p.BaptismDate = a[names["BaptismDate"]].ToDate();
						if (names.ContainsKey("Position"))
							p.PositionInFamilyId = Position(a);
						if (names.ContainsKey("Title") && a[names["Title"]].HasValue())
							p.TitleCode = a[names["Title"]].Truncate(10);
						if (names.ContainsKey("Campus"))
							p.CampusId = campuses[a[names["Campus"]]];
						if (names.ContainsKey("MemberStatus"))
						{
							var ms = a[names["MemberStatus"]];
							var qms = from mm in Db.MemberStatuses
									  where mm.Description == ms
									  select mm;
							var m = qms.SingleOrDefault();
							if (m == null)
							{
								var nx = Db.MemberStatuses.Max(mm => mm.Id) + 1;
								m = new MemberStatus { Id = nx, Description = ms, Code = nx.ToString() };
								Db.MemberStatuses.InsertOnSubmit(m);
							}
							p.MemberStatusId = m.Id;
						}
					}

					var nq = from name in names.Keys
							 where !standardnames.Contains(name)
							 select name;
					var now = DateTime.Now;
					foreach (var name in nq)
					{
						var b = name.Split('.');
						if (name.EndsWith(".txt"))
							p.AddEditExtraData(b[0], a[names[name]].Trim());
						else if (name.EndsWith(".dt"))
						{
							var d = a[names[name]].Trim().ToDate();
							if (d.HasValue)
								p.AddEditExtraDate(b[0], d.Value);
						}
						else if (name.EndsWith(".int"))
							p.AddEditExtraInt(b[0], a[names[name]].Trim().ToInt());
						else
							p.AddEditExtraValue(name, a[names[name]].Trim());
					}
					rt.Processed++;
					if (!testing)
						Db.SubmitChanges();
				}
				if (!testing)
					Db.SubmitChanges();
			}
			return true;
		}
	}
}