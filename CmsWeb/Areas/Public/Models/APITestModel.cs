using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Text;
using UtilityExtensions;
using System.Web.Mvc;

namespace CmsWeb.Models
{
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
                var ss = script.Replace("\r\n", "\n");
                return ss.Split('\n').Length + 1;
            }
        }

        public static List<ApiTestInfo> testplan()
        {
            return new List<ApiTestInfo>
            {
                new ApiTestInfo 
                { 
                    api = "Lookups",
                    args = new Dictionary<string, string>
                    { { "table", "MemberType" } },
                    script = @"
xml = webclient.DownloadString('API/lookups/' + table)
return xml
",
                },
                new ApiTestInfo 
                { 
                    api = "LoginInfo",
                    args = new Dictionary<string, string>
                    { { "peopleid", Util.UserPeopleId.ToString() } },
                    script = @"
xml = webclient.DownloadString('API/LoginInfo/' + peopleid)
return xml
",
                },
                new ApiTestInfo 
                { 
                    api = "OrganizationsForDiv",
                    args = new Dictionary<string, string>
                    { { "divid", "6097" } },
                    script = @"
xml = webclient.DownloadString('API/OrganizationsForDiv/' + divid)
return xml
",
                },
                new ApiTestInfo 
                { 
                    api = "OrgMembers",
                    args = new Dictionary<string, string>
                    { { "orgid", "87900" } },
                    script = @"
xml = webclient.DownloadString('API/OrgMembers/' + orgid)
return xml
",
                },
                new ApiTestInfo 
                { 
                    api = "Login",
                    args = new Dictionary<string, string>
                    {
                        { "user", "" },
                        { "password", "" }
                    },
                    script = @"
coll = NameValueCollection()
coll.Add('user', user)
coll.Add('password', password)
resp = webclient.UploadValues('API/Login', 'POST', coll)
xml = Encoding.ASCII.GetString(resp)
return xml
",
                },
                new ApiTestInfo 
                { 
                    api = "OrgMembers2",
                    args = new Dictionary<string, string>
                    { { "orgid", "87900" } },
                    script = @"
xml = webclient.DownloadString('API/OrgMembers2/' + orgid)
return xml
",
                },
                new ApiTestInfo 
                { 
                    api = "ExtraValues",
                    args = new Dictionary<string, string>
                    {
                        { "peopleid", "828612" },
                        { "fields", "" },
                    },
                    script = @"
xml = webclient.DownloadString('API/ExtraValues/' + peopleid + '?fields=' + fields)
return xml
",
                },
                new ApiTestInfo 
                { 
                    api = "AddEditExtraValue",
                    args = new Dictionary<string, string>
                    {
                        { "peopleid", "828612" },
                        { "field", "" },
                        { "value", "" },
                    },
                    script = @"
coll = NameValueCollection()
coll.Add('peopleid', peopleid)
coll.Add('field', field)
coll.Add('value', value)
resp = webclient.UploadValues('API/AddEditExtraValue', 'POST', coll)
ret = Encoding.ASCII.GetString(resp)
return ret
",
                },
                new ApiTestInfo 
                { 
                    api = "DeleteExtraValue",
                    args = new Dictionary<string, string>
                    {
                        { "peopleid", "828612" },
                        { "field", "" },
                    },
                    script = @"
coll = NameValueCollection()
coll.Add('peopleid', peopleid)
coll.Add('field', field)
resp = webclient.UploadValues('API/DeleteExtraValue', 'POST', coll)
ret = Encoding.ASCII.GetString(resp)
return ret
",
                },
                new ApiTestInfo 
                { 
                    api = "MarkRegistered",
                    args = new Dictionary<string, string>
                    {
                        { "meetingid", "4288226" },
                        { "peopleid", "828612" },
                        { "present", "true" },
                    },
                    script = @"
coll = NameValueCollection()
coll.Add('meetingid', meetingid)
coll.Add('peopleid', peopleid)
coll.Add('present', present)
resp = webclient.UploadValues('API/MarkRegistered', 'POST', coll)
ret = Encoding.ASCII.GetString(resp)
return ret
",
                },
                new ApiTestInfo 
                {
                    api = "GetOneTimeLoginLink",
                    args = new Dictionary<string, string>
                    {
                        { "url", "" },
                        { "user", "David" },
                    },
                    script = @"
coll = NameValueCollection()
coll.Add('url', url)
coll.Add('user', user)
resp = webclient.UploadValues('API/GetOneTimeLoginLink', 'POST', coll)
ret = Encoding.ASCII.GetString(resp)
return ret
",
                },
                new ApiTestInfo 
                {
                    api = "GetOneTimeRegisterLink",
                    args = new Dictionary<string, string>
                    {
                        { "PeopleId", "828612" },
                        { "OrgId", "88216" },
                    },
                    script = @"
coll = NameValueCollection()
coll.Add('PeopleId', PeopleId)
coll.Add('OrgId', OrgId)
resp = webclient.UploadValues('API/GetOneTimeRegisterLink', 'POST', coll)
ret = Encoding.ASCII.GetString(resp)
return ret
",
                },
                new ApiTestInfo 
                { 
                    api = "UpdateOrgMember",
                    description = @"<ul>
<li>type is an integer code for MemberType</li>
<li>enrolled is a DateTime value</li>
<li>inactive is a DateTime value</li>
<li>You can pass the word 'null' for the InactiveDate to reset it to a null value.</li>
<li>If you leave any of the fields blank, that field will not be updated.</li>
</ul>
",
                    args = new Dictionary<string, string>
                    {
                        { "PeopleId", "828612" },
                        { "OrgId", "88216" },
                        { "type", "155" },
                        { "enrolled", "" },
                        { "inactive", "3/20/12" },
                    },
                    script = @"
coll = NameValueCollection()
coll.Add('PeopleId', PeopleId)
coll.Add('OrgId', OrgId)
coll.Add('type', type)
coll.Add('enrolled', enrolled)
coll.Add('inactive', inactive)
resp = webclient.UploadValues('API/UpdateOrgMember', 'POST', coll)
ret = Encoding.ASCII.GetString(resp)
return ret
",
                },
                new ApiTestInfo 
                { 
                    api = "",
                    args = new Dictionary<string, string>
                    {
                        { "", "" },
                    },
                    script = @"
",
                },
                new ApiTestInfo 
                { 
                    api = "",
                    args = new Dictionary<string, string>
                    {
                        { "", "" },
                    },
                    script = @"
",
                },
                new ApiTestInfo 
                { 
                    api = "",
                    args = new Dictionary<string, string>
                    {
                        { "", "" },
                    },
                    script = @"
",
                },
                new ApiTestInfo 
                { 
                    api = "",
                    args = new Dictionary<string, string>
                    {
                        { "", "" },
                    },
                    script = @"
",
                },
                new ApiTestInfo 
                { 
                    api = "",
                    args = new Dictionary<string, string>
                    {
                        { "", "" },
                    },
                    script = @"
",
                },
                new ApiTestInfo 
                { 
                    api = "",
                    args = new Dictionary<string, string>
                    {
                        { "", "" },
                    },
                    script = @"
",
                },
                new ApiTestInfo 
                { 
                    api = "",
                    args = new Dictionary<string, string>
                    {
                        { "", "" },
                    },
                    script = @"
",
                },
                new ApiTestInfo 
                { 
                    api = "",
                    args = new Dictionary<string, string>
                    {
                        { "", "" },
                    },
                    script = @"
",
                },
            };
        }
    }
}
