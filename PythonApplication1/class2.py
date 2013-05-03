import clr
clr.AddReferenceByName("CmsData")
from CmsData.API import *

import System
import System.Text
from System import *
from System.Text import *

class OrgMembers(object):

	def Run(self, m, w, q):
		w.Start("OrgMembers")
		for i in q:
			w.Start("Member")
			w.Attr("PeopleId", i.member.PeopleId)
			w.Attr("PreferredName", i.member.Person.PreferredName)
			w.Attr("LastName", i.member.Person.LastName)
			w.Attr("Email", i.member.Person.EmailAddress)
			w.Attr("Enrolled", i.member.EnrollmentDate)
			w.Attr("MemberType", i.member.MemberType.Description)
			for t in i.tags:
				w.Add("Group", t)
			for a in i.meetings:
				w.Add("Meeting", a.MeetingDate)
			w.End()
		w.End()
		return w.ToString()

api = APIFunctions()
w = APIWriter()
q = api.OrgMembersData(89114)
om = OrgMembers()
ret = om.Run(api, w, q)

Console.WriteLine(ret);
Console.Write('press any key')
Console.ReadKey(True)