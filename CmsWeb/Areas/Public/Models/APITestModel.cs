using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Text;
using UtilityExtensions;
using System.Web.Mvc;
using System.Diagnostics;

namespace CmsWeb.Models
{
    public class ApiSectionInfo
    {
        public string section { get; set; }
        public List<ApiTestInfo> list { get; set; }
    }
    public class ApiTestInfo
    {
        public string api { get; set; }
        public Dictionary<string, string> args { get; set; }
        public string script { get; set; }
        public string description { get; set; }
        public int LineCount
        {
            get
            {
                if (!script.HasValue())
                    return 0;
                return script.Split('\n').Length - 1;
            }
        }

        public static List<ApiSectionInfo> testplan()
        {
            var list = new List<ApiSectionInfo>();
            string pushline = null;
            ApiSectionInfo section = null;
            ApiTestInfo test = null;
            var sm = new System.IO.StringReader(plan.Replace("    ", "\t"));
            var LookingFor = "dashes";
            var sb = new StringBuilder();
            string[] a;
            string line = null;
            do
            {
                if (pushline != null)
                {
                    line = pushline;
                    pushline = null;
                }
                else
                    line = sm.ReadLine();
                Debug.WriteLine(line);
                if (line == null)
                    break;
                switch (LookingFor)
                {
                    case "dashes":
                        if (!line.StartsWith("------"))
                            throw new Exception("Expected dashes");
                        line = sm.ReadLine();
                        if (line == null)
                            break;
                        a = ParseLine(line);
                        switch (a[0])
                        {
                            case "Section":
                                section = new ApiSectionInfo 
                                { 
                                    section = a[1], 
                                    list = new List<ApiTestInfo>() 
                                };
                                list.Add(section);
                                break;
                            case "Test":
                                test = new ApiTestInfo { api = a[1], args = new Dictionary<string,string>() };
                                section.list.Add(test);
                                LookingFor = "description";
                                break;
                            default:
                                throw new Exception("expected Test or Section");
                        }
                        break;
                    case "description":
                        if (!line.StartsWith("Description"))
                            throw new Exception("Expected Description");
                        a = ParseDescription(line);
                        if (a[1].HasValue())
                            test.description = a[1];
                        else
                        {
                            LookingFor = "descriptionlines";
                            sb = new StringBuilder();
                        }
                        break;
                    case "descriptionlines":
                        if (line.StartsWith("\t"))
                            sb.AppendLine(line.Substring(1));
                        else
                        {
                            test.description = sb.ToString();
                            LookingFor = "args";
                            pushline = line;
                        }
                        break;
                    case "args":
                        a = ParseLine(line);
                        if (!a[0].StartsWith("Arg"))
                        {
                            LookingFor = "script";
                            pushline = line;
                        }
                        else
                            test.args.Add(a[1], a[2]);
                        break;
                    case "script":
                        if (!line.StartsWith("Script"))
                            throw new Exception("Expected Script");
                        LookingFor = "scriptlines";
                        sb = new StringBuilder();
                        break;
                    case "scriptlines":
                        if (line.StartsWith("\t"))
                            sb.AppendLine(line.Substring(1));
                        else
                        {
                            test.script = sb.ToString();
                            LookingFor = "dashes";
                            pushline = line;
                        }
                        break;
                }
            } while (line != null);
            return list;
        }
        private static string[] ParseLine(string s)
        {
            var a = s.SplitStr(":", 2);
            var aa = new string[3];
            aa[0] = a[0];
            if (a.Length > 1)
            {
                var b = a[1].SplitStr(",");
                aa[1] = b[0].Trim();
                if (b.Length > 1)
                    aa[2] = b[1].Trim();
            }
            return aa;
        }
        private static string[] ParseDescription(string s)
        {
            var a = s.SplitStr(":", 2);
            var aa = new string[2];
            aa[0] = a[0]; ;
            if (a.Length > 1)
                aa[1] = a[1].Trim(); ;
            return aa;
        }
        private static string plan = @"------------
Section: Meta
------------
Test: Lookups
Description:
    <ul>
    <li>These are tables of id / value pairs for look up tables</li>
    <li>They will allow you to convert a number into a description for display</li>
    <li>See the Admin > Lookups for a list of tables</li>
    <li>The table parameter is the name of the table from the Lookups page</li>
    </ul>
Arg: table, MemberStatus
Script:
	xml = webclient.DownloadString('APIMeta/lookups/' + table)
	return xml
------------
Section: Person
------------
Test: GetPerson
Description:
    <ul>
    <li>peopleid (required)</li>
    </ul>
Arg: peopleid
Script:
	xml = webclient.DownloadString('APIPerson/GetPerson/' + peopleid)
	return xml
------------
Test: GetPeople
Description:
    <ul>
    <li>peopleid (optional)</li>
    <li>famid (optional)</li>
    <li>first (optional, exact match)</li>
    <li>last (optional, exact match)</li>
    </ul>
Arg: peopleid
Arg: famid
Arg: first
Arg: last
Script:
    def AddArg(args, arg, value):
    	if len(value) > 0:
    		if len(args) > 0:
    			args += '&'
    		else:
    			args = '?'
    		args += arg + '=' + value
    	return args
    args = ''
    args = AddArg(args, 'peopleid', peopleid)
    args = AddArg(args, 'famid', famid)
    args = AddArg(args, 'first', first)
    args = AddArg(args, 'last', last)
    xml = webclient.DownloadString('APIPerson/GetPeople' + args)
    return xml
------------
Test: UpdatePerson
Description:
    <ul>
    <li>You will upload an XML document which has the same elements and structure as the XML returned for the GetPerson above.</li>
    <li>All elements are optional. It will only update those that are included.</li>
    <li>Address verification is done as well. If the address does not verify, you get an XML doc showing you the proposed changes with notices about previous values.</li>
    <li>If the address was found, you could resubmit with the new values and it would save then</li>
    </ul>
Arg: PeopleId
Arg: MiddleName
Arg: WorkPhone
Arg: AddressLineOne
Arg: CityName
Arg: StateCode
Arg: ZipCode
Arg: CountryName
Script:
	d = dict()
	d['PeopleId'] = PeopleId
	d['MiddleName'] = MiddleName
	d['WorkPhone'] = WorkPhone
	d['AddressLineOne'] = AddressLineOne
	d['CityName'] = CityName
	d['StateCode'] = StateCode
	d['ZipCode'] = ZipCode
	d['CountryName'] = CountryName
	xml = '''<?xml version=""1.0"" encoding=""utf-16""?>
	<Person PeopleId=""%(PeopleId)s"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
	  <MiddleName>%(MiddleName)s</MiddleName>
	  <WorkPhone>%(WorkPhone)s</WorkPhone>
	  <PersonalAddress>
	    <AddressLineOne>%(AddressLineOne)s</AddressLineOne>
	    <CityName>%(CityName)s</CityName>
	    <StateCode>%(StateCode)s</StateCode>
	    <ZipCode>%(ZipCode)s</ZipCode>
	    <CountryName>%(CountryName)s</CountryName>
	  </PersonalAddress>
	</Person>''' % d
	retxml = webclient.UploadString('APIPerson/UpdatePerson/', xml)
	return retxml
------------
Test: Login
Description:
    <ul>
    <li>The user and password are for the person you want to authenticate</li>
    <li>The return xml doc comes from the APILoginInfo Python script in Special Content</li>
    <li>A list of QueryBits (tags that are updated nightly) are returned</li>
    </ul>
Arg: username
Arg: password
Script:
	coll = NameValueCollection()
	coll.Add('user', user)
	coll.Add('password', password)
	resp = webclient.UploadValues('APIPerson/Login', 'POST', coll)
	xml = Encoding.ASCII.GetString(resp)
	return xml
------------
Test: LoginInfo
Description:
    <ul>
    <li>Returns the same information as the Login method above.</li>
    </ul>
Arg: peopleid
Script:
	xml = webclient.DownloadString('APIPerson/LoginInfo/' + peopleid)
	return xml
------------
Test: FamilyMembers
Description:
    <ul>
    <li>returns the members of a family</li>
    </ul>
Arg: familyid
Script:
	xml = webclient.DownloadString('APIPerson/FamilyMembers/' + familyid)
	return xml
------------
Test: ExtraValues
Description:
    <ul>
    <li>leave fields blank to get all extravalues</li>
    <li>or put in a list of comma separated field names</li>
    </ul>
Arg: peopleid
Arg: fields
Script:
	xml = webclient.DownloadString('APIPerson/ExtraValues/' + peopleid + '?fields=' + fields)
	return xml
------------
Test: AddEditExtraValue
Description:
    <ul>
    <li>this will add an extra value or update an existing one</li>
    <li>type can be one of data (default), code, int, or date</li>
    </ul>
Arg: peopleid
Arg: field
Arg: value
Arg: type
Script:
	coll = NameValueCollection()
	coll.Add('peopleid', peopleid)
	coll.Add('field', field)
	coll.Add('value', value)
	coll.Add('type', type)
	resp = webclient.UploadValues('APIPerson/AddEditExtraValue', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Test: DeleteExtraValue
Description:
Arg: peopleid
Arg: field
Script:
	coll = NameValueCollection()
	coll.Add('peopleid', peopleid)
	coll.Add('field', field)
	resp = webclient.UploadValues('APIPerson/DeleteExtraValue', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Test: GetOneTimeLoginLink
Description:
    <ul>
    <li>This is for Single Sign On capability</li>
    <li>Once you have authenticated a person using the Login method above,</li>
    <li>You can use this method to let them visit a page on BVCMS without logging in</li>
    <li>They will be able to use BVCMS as if they had logged in</li>
    <li>Basically, you present a hyperlink to the user which goes to a URL on your server</li>
    <li>Then behind the scenes, you call this method with the URL they really intended to visit</li>
    <li>You will be returned a special URL with a token that will work only once</li>
    <li>You should redirect their browser to this URL</li>
    </ul>
Arg: url
Arg: user
Script:
	coll = NameValueCollection()
	coll.Add('url', url)
	coll.Add('user', user)
	resp = webclient.UploadValues('APIPerson/GetOneTimeLoginLink', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Test: GetOneTimeRegisterLink
Description:
    <ul>
    <li>This works the same as the above except that it is for a registration</li>
    <li>You just need to pass the PeopleId and OrgId instead of the UserName</li>
    </ul>
Arg: peopleid
Arg: orgid
Script:
	coll = NameValueCollection()
	coll.Add('PeopleId', PeopleId)
	coll.Add('OrgId', OrgId)
	resp = webclient.UploadValues('APIPerson/GetOneTimeRegisterLink', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Test: AccessUsers
Description:
    <ul>
    <li>Returns a list of users with Access role</li>
    </ul>
Script:
	xml = webclient.DownloadString('APIPerson/AccessUsers/')
	return xml
------------
Test: ChangePassword
Description:
Arg: username
Arg: current
Arg: password
Script:
	coll = NameValueCollection()
	coll.Add('username', username)
	coll.Add('current', current)
	coll.Add('password', password)
	resp = webclient.UploadValues('APIPerson/ChangePassword', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Section: Org
------------
Test: OrganizationsForDiv
Description:
Arg: divid
Script:
	xml = webclient.DownloadString('APIOrg/OrganizationsForDiv/' + divid)
	return xml
------------
Test: OrgMembers
Description:
    <ul>
    <li>Returns a list of OrgMembers</li>
    <li>You can optionally pass a search parameter.<br />
        It searches on the name formatted as 'last, first'.
        You do not have to pass the entire name, just a few characters will suffice to narrow the list.</li>
    </ul>
Arg: orgid
Arg: search
Script:
	xml = webclient.DownloadString('APIOrg/OrgMembers/' + orgid + '?search=' + search)
	return xml
------------
Test: OrgMembers2
Description:
Arg: orgid
Script:
	xml = webclient.DownloadString('APIOrg/OrgMembers2/' + orgid)
	return xml
------------
Test: UpdateOrgMember
Description:
	<ul>
	<li>type is an integer code for MemberType</li>
	<li>enrolled is a DateTime value</li>
	<li>inactive is a DateTime value</li>
	<li>You can pass the word 'null' for the InactiveDate to reset it to a null value.</li>
	<li>If you leave any of the fields blank, that field will not be updated.</li>
	</ul>
Arg: peopleid
Arg: orgid
Arg: type
Arg: enrolled
Arg: inactive
Script:
	coll = NameValueCollection()
	coll.Add('PeopleId', PeopleId)
	coll.Add('OrgId', OrgId)
	coll.Add('type', type)
	coll.Add('enrolled', enrolled)
	coll.Add('inactive', inactive)
	resp = webclient.UploadValues('APIOrg/UpdateOrgMember', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
---------------
Test: ExtraValues
Description:
    <ul>
    <li>leave fields blank to get all extravalues</li>
    <li>or put in a list of comma separated field names</li>
    </ul>
Arg: orgid
Arg: fields
Script:
	xml = webclient.DownloadString('APIOrg/ExtraValues/' + orgid + '?fields=' + fields)
	return xml
------------
Test: AddEditExtraValue
Description:
    <ul>
    <li>this will add an extra value or update an existing one</li>
    </ul>
Arg: orgid
Arg: field
Arg: value
Script:
	coll = NameValueCollection()
	coll.Add('orgid', orgid)
	coll.Add('field', field)
	coll.Add('value', value)
	resp = webclient.UploadValues('APIOrg/AddEditExtraValue', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Test: DeleteExtraValue
Description:
Arg: orgid
Arg: field
Script:
	coll = NameValueCollection()
	coll.Add('orgid', orgid)
	coll.Add('field', field)
	resp = webclient.UploadValues('APIOrg/DeleteExtraValue', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Section: Meeting
------------
Test: ExtraValues
Description:
    <ul>
    <li>leave fields blank to get all extravalues</li>
    <li>or put in a list of comma separated field names</li>
    </ul>
Arg: meetingid
Arg: fields
Script:
	xml = webclient.DownloadString('APIMeeting/ExtraValues/' + meetingid + '?fields=' + fields)
	return xml
------------
Test: AddEditExtraValue
Description:
    <ul>
    <li>this will add an extra value or update an existing one</li>
    </ul>
Arg: meetingid
Arg: field
Arg: value
Script:
	coll = NameValueCollection()
	coll.Add('meetingid', meetingid)
	coll.Add('field', field)
	coll.Add('value', value)
	resp = webclient.UploadValues('APIMeeting/AddEditExtraValue', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Test: DeleteExtraValue
Description:
Arg: meetingid
Arg: field
Script:
	coll = NameValueCollection()
	coll.Add('meetingid', meetingid)
	coll.Add('field', field)
	resp = webclient.UploadValues('APIMeeting/DeleteExtraValue', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Test: MarkRegistered
Description:
    <ul>
    <li>registered is true or false</li>
    <li>registered indicates an intent to attend a future meeting</li>
    </ul>
Arg: meetingid
Arg: peopleid
Arg: registered
Script:
	coll = NameValueCollection()
	coll.Add('meetingid', meetingid)
	coll.Add('peopleid', peopleid)
	coll.Add('registered', registered)
	resp = webclient.UploadValues('APIMeeting/MarkRegistered', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Section: Checkin2
------------
Test: Match
Description:
    <ul>
    <li>this method returns all family members and their possible checkin classes or multiple families</li>
    <li>phone is a 7 or 10 digit phone number, matches cell or home</li>
    <li>campus is an integer</li>
    <li>thisday is an integer where sunday = 0, monday = 1 etc.</li>
    <li>page is an optional integer where first page = 1 (default)</li>
    <li>kiosk is an optional description of the originating kiosk</li>
    </ul>
Arg: phone
Arg: campus
Arg: thisday
Arg: page
Arg: kiosk
Script:
    args = phone + '?campus=' + campus + '&thisday=' + thisday
    if len(page) > 0:
        args += '&page=' + page
    if len(kiosk) > 0:
        args += '&kiosk=' + kiosk
	xml = webclient.DownloadString('APICheckin2/Match/' + args)
	return xml
------------
Test: NameSearch
Description:
    <ul>
    <li>space separates first from last name</li>
    <li>you can use partial names to avoid spelling mistakes, a few of the beginning letters of each</li>
    <li>you can use just the last name without a space</li>
    </ul>
Arg: name
Arg: page
Script:
    args = name
    if len(page) > 0:
        args += '?page=' + page
	xml = webclient.DownloadString('APICheckin2/NameSearch/' + args)
	return xml
------------
Test: RecordAttend2
Description:
    <ul>
    <li>PeopleId and OrgId are integers</li>
    <li>Present is true or false</li>
    <li>hour is a date and time value formatted like: 11/20/2011 9:30 AM</li>
    <li>kiosk is the optional name of the kiosk</li>
    </ul>
Arg: PeopleId
Arg: OrgId
Arg: Present
Arg: hour
Arg: kiosk
Script:
	coll = NameValueCollection()
	coll.Add('PeopleId', PeopleId)
	coll.Add('OrgId', OrgId)
	coll.Add('Present', Present)
	coll.Add('hour', hour)
	coll.Add('kiosk', kiosk)
	resp = webclient.UploadValues('APICheckin2/RecordAttend2', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Test: AddPerson
Description:
    <ul>
    <li>familyid, 0 if adding a new family or # for adding to an existing family</li>
    <li>home, home phone</li>
    <li>grade, (integer) grade -1 = preschool, 0 = kindergarten, 1 = first etc. 99 = special</li>
    <li>parent, name of parent</li>
    <li>emfriend, name of person bringing</li>
    <li>emphone, cell phone number of person bringing</li>
    <li>marital, (integer) marital status (see lookup table for codes)</li>
    <li>gender, (integer) see lookup table for codes</li>
    <li>campusid, (integer) see lookup table for codes</li>
    <li>activeother, (true/false) active in another church</li>
    <li>the following Ask* parameters indicate whether they were asked for that information</li>
    <li>AskChurch, (true/false)</li>
    <li>AskChurchName, (true/false) or this</li>
    <li>AskGrade, (true/false) or this</li>
    <li>AskEmFriend, (true/false) or this</li>
    <li>returns familyid.peopleid as in 454.602 where 454 is the new familyid and 602 is the new peopleid</li>
    </ul>
Arg: familyid
Arg: first
Arg: last
Arg: goesby
Arg: dob
Arg: marital
Arg: gender
Arg: email
Arg: cell
Arg: home
Arg: addr
Arg: zip
Arg: campusid
Arg: allergies
Arg: AskEmFriend
Arg: parent
Arg: emfriend
Arg: emphone
Arg: AskChurch
Arg: activeother
Arg: AskChurchName
Arg: churchname
Arg: AskGrade
Arg: grade
Script:
    coll = NameValueCollection()
    coll.Add('first', first)
    coll.Add('last', last)
    coll.Add('goesby', goesby)
    coll.Add('dob', dob)
    coll.Add('marital', marital)
    coll.Add('gender', gender)
    coll.Add('email', email)
    coll.Add('cell', cell)
    coll.Add('home', home)
    coll.Add('addr', addr)
    coll.Add('zip', zip)
    coll.Add('campusid', campusid)
    coll.Add('allergies', allergies)
    if AskEmFriend == 'true':
        coll.Add('parent', parent)
        coll.Add('emfriend', emfriend)
        coll.Add('emphone', emphone)
        coll.Add('AskEmFriend', AskEmFriend)
    if AskChurch == 'true':
        coll.Add('activeother', activeother)
        coll.Add('AskChurch', AskChurch)
    if AskChurchName == 'true':
        coll.Add('churchname', churchname)
        coll.Add('AskChurchName', AskChurchName)
    if AskGrade == 'true':
        coll.Add('AskGrade', AskGrade)
        coll.Add('grade', grade)
    resp = webclient.UploadValues('APICheckin2/AddPerson/' + familyid, 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Test: EditPerson
Description:
    <ul>
    <li>see AddPerson for information about fields</li>
    <li>peopleid is required, the others are not required and will only be updated if present in the collection</li>
    <li>the fields below the Ask* Args will not be updated unless the particular Ask Arg is present too</li>
    <li>returns familyid</li>
    </ul>
Arg: peopleid
Arg: first
Arg: last
Arg: goesby
Arg: dob
Arg: marital
Arg: gender
Arg: email
Arg: cell
Arg: home
Arg: addr
Arg: zip
Arg: campusid
Arg: allergies
Arg: AskEmFriend
Arg: parent
Arg: emfriend
Arg: emphone
Arg: AskChurch
Arg: activeother
Arg: AskChurchName
Arg: churchname
Arg: AskGrade
Arg: grade
Script:
    coll = NameValueCollection()
    coll.Add('first', first)
    coll.Add('last', last)
    coll.Add('goesby', goesby)
    coll.Add('dob', dob)
    coll.Add('marital', marital)
    coll.Add('gender', gender)
    coll.Add('email', email)
    coll.Add('cell', cell)
    coll.Add('home', home)
    coll.Add('addr', addr)
    coll.Add('zip', zip)
    coll.Add('campusid', campusid)
    coll.Add('allergies', allergies)
    if AskEmFriend == 'true':
        coll.Add('parent', parent)
        coll.Add('emfriend', emfriend)
        coll.Add('emphone', emphone)
        coll.Add('AskEmFriend', AskEmFriend)
    if AskChurch == 'true':
        coll.Add('activeother', activeother)
        coll.Add('AskChurch', AskChurch)
    if AskChurchName == 'true':
        coll.Add('churchname', churchname)
        coll.Add('AskChurchName', AskChurchName)
    if AskGrade == 'true':
        coll.Add('AskGrade', AskGrade)
        coll.Add('grade', grade)
    resp = webclient.UploadValues('APICheckin2/EditPerson/' + peopleid, 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Test: EditPerson (single field)
Description:
    <ul>
    <li>This demonstrates that it is possible update a single field (or as few as you want)</li>
    <li>peopleid is required, the others are not required and will only be updated if present in the collection</li>
    <li>returns familyid</li>
    </ul>
Arg: peopleid
Arg: goesby
Script:
    coll = NameValueCollection()
    coll.Add('goesby', goesby)
    resp = webclient.UploadValues('APICheckin2/EditPerson/' + peopleid, 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Test: Campuses
Description:
    <ul>
    <li>returns a list of campuses suitable for a dropdown menu</li>
    <li>the checkin program uses this simple function to authenticate the user</li>
    </ul>
Script:
	xml = webclient.DownloadString('APICheckin2/Campuses')
	return xml
------------
Test: UploadImage
Description:
Script:
------------
Test: FetchImage
Description:
Script:
------------
Section: iPhone
------------
Test: Search
Description:
    <ul>
    <li>name: space separates first from last name<br>
        you can use partial names to avoid spelling mistakes, a few of the beginning letters of each<br>
        you can use just the last name without a space</li>
    <li>comm: searches cell, home, work, and email</li>
    <li>addr: searches address, city and zip</li>
    </ul>
Arg: name
Arg: comm
Arg: addr
Script:
    coll = NameValueCollection()
    coll.Add('name', name)
    coll.Add('comm', comm)
    coll.Add('addr', addr)
    resp = webclient.UploadValues('APIiPhone/Search', 'POST', coll)
	ret = Encoding.ASCII.GetString(resp)
	return ret
------------
Test: Organizations
Description:
    <ul>
    <li>no arguments</li>
    </ul>
Script:
	xml = webclient.DownloadString('APIiPhone/Organizations')
	return xml
------------
Test: RollList
Description:
    <ul>
    <li>orgid</li>
    <li>datetime (should be urlencoded string as in '5/20/11%208:00AM')</li>
    </ul>
Arg: orgid
Arg: datetime
Script:
    coll = NameValueCollection()
    coll.Add('datetime', datetime)
    resp = webclient.UploadValues('APIiPhone/RollList/' + orgid, 'POST', coll)
	xml = Encoding.ASCII.GetString(resp)
	return xml
------------
";
    }
}
