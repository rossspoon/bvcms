using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;
using System.Text.RegularExpressions;
using System.Data.Linq;
using System.Xml.Linq;
using System.Data.Linq.SqlClient;
using IronPython.Hosting;
using System.IO;
using CmsData.Codes;
using System.Web;

namespace CmsData
{
	public class PythonEvents
	{
		private CMSDataContext Db;
		public dynamic instance { get; set; }

		public PythonEvents(CMSDataContext Db, string classname, string script)
		{
			this.Db = Db;
			var engine = Python.CreateEngine();
			var sc = engine.CreateScriptSourceFromString(script);

			var code = sc.Compile();
			var scope = engine.CreateScope();
            scope.SetVariable("model", this);
			code.Execute(scope);

			dynamic Event = scope.GetVariable(classname);
			instance = Event();
		}
		public void CreateTask(int forPeopleId, Person p, string description)
		{
			DbUtil.LogActivity("Adding Task about: {0}".Fmt(p.Name));
			var t = p.AddTaskAbout(Db, forPeopleId, description);
			Db.SubmitChanges();
            Db.Email(DbUtil.SystemEmailAddress, DbUtil.Db.LoadPersonById(forPeopleId),
                "TASK: " + description,
                Task.TaskLink(Db, description, t.Id) + "<br/>" + p.Name);
		}
	}
}
