using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using System.Web;

namespace CmsData
{
    public class ProtectMyMinistryHelper
    {
        public const String PMM_URL = "https://services.priorityresearch.com/webservice/default.cfm";

        public const int PACKAGE_BASIC = 1;
        public const int PACKAGE_PLUS = 2;

        public const int SERVICE_CRIMINAL_COUNTY = 3;
        public const int SERVICE_CRIMINAL_STATE = 4;
        public const int SERVICE_CRIMINAL_MULTISTATE = 5;
        public const int SERVICE_SSN_TRACE = 6;
        public const int SERVICE_SEXOFFENDER_MULTISTATE = 7;
        public const int SERVICE_MOTOR_VEHICLE_RECORD = 8;
        public const int SERVICE_EDUCATION = 9;
        public const int SERVICE_EMPLOYMENT = 10;
        public const int SERVICE_CREDIT = 11;
        public const int SERVICE_WORKMANS_COMP = 12;
        public const int SERVICE_NATIONAL_COMBO = 13;

        public static string[] SERVICE_CODES = { "Invalid", "BASIC", "PLUS", "CountyCrim", "StateCriminal", "MultistateCrim", "SSNTrace",
                                                   "NationalSOR", "MVR", "EduVerify", "EmploymentVer", "Credit", "WorkersComp", "Combo" };

        public bool bTestMode = true;
        public bool bInitialized = false;

        public int[] sServiceCodes;

        public string sMethod = "SEND ORDER";

        public string sUser;
        public string sPassword;

        public string sReturnURL = "http://steven.bvcms.com:8888/ExternalServices/PMMResults";
        public string sBillingReference;
        public string sOrderID = "1234567";

        public string sFirstName;
        public string sMiddleName;
        public string sLastName;
        public string sGeneration;
        public string sDOB;
        public string sSSN = "123-45-6789";
        public string sGender;
        public string sEthnicity;

        public string sAddress;
        public string sCity;
        public string sState;
        public string sZip;

        public string sDLNumber;

        public string sJurisdictionCode;
        public string sJurisdictionState;

        private MemoryStream msRequest;
        private XmlWriter xwWriter;

        public ProtectMyMinistryHelper()
        {
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.NewLineOnAttributes = false;
            //xws.NewLineChars = "";

            msRequest = new MemoryStream();
            xwWriter = XmlWriter.Create(msRequest, xws);
        }

        ~ProtectMyMinistryHelper()
        {
            if( msRequest != null ) msRequest.Close();
        }

        public bool init()
        {
            sUser = DbUtil.Db.Setting("PMMUser", null);
            sPassword = DbUtil.Db.Setting("PMMPassword", null);

            if (sUser == null || sPassword == null) return false;

            return bInitialized = true;
        }

        public void populate(int PersonID)
        {
            var person = (from e in DbUtil.Db.People
                          where e.PeopleId == PersonID
                          select e).FirstOrDefault();

            populate(person);
        }

        public void populate(Person pPerson)
        {
            int iBirthMonth = pPerson.BirthMonth ?? 0;
            int iBirthDay = pPerson.BirthDay ?? 0;
            int iBirthYear = pPerson.BirthYear ?? 0;

            sBillingReference = pPerson.PeopleId.ToString();
            sFirstName = pPerson.FirstName;
            sMiddleName = pPerson.MiddleName;
            sLastName = pPerson.LastName;
            sGeneration = pPerson.SuffixCode;
            sDOB = iBirthMonth.ToString("D2") + "/" + iBirthDay.ToString("D2") + "/" + iBirthYear.ToString("D4");
            sGender = pPerson.Gender.Description;

            sAddress = pPerson.AddressLineOne;
            sCity = pPerson.CityName;
            sState = pPerson.StateCode;
            sZip = pPerson.ZipCode;
        }

        public void setServiceCodes(int iNewCode)
        {
            sServiceCodes = new int[] { iNewCode };
        }

        public void setServiceCodes(int[] iNewCodes)
        {
            sServiceCodes = iNewCodes;
        }

        public bool xmlCreate()
        {
            // Open Document
            xwWriter.WriteStartDocument();

            // Open OrderXML
            xwWriter.WriteStartElement("OrderXML");

            // Method (Inside OrderXML)
            xwWriter.WriteElementString("Method", sMethod);

            // Authentication Section (Inside OrderXML)
            xwWriter.WriteStartElement("Authentication");
            xwWriter.WriteElementString("Username", sUser);
            xwWriter.WriteElementString("Password", sPassword);
            xwWriter.WriteFullEndElement();

            if( bTestMode ) xwWriter.WriteElementString("TestMode", "YES");

            // Return URL (Inside OrderXML)
            xwWriter.WriteElementString("ReturnResultURL", sReturnURL);

            // Order Section (Inside OrderXML)
            xwWriter.WriteStartElement("Order");

            // Our Billing Reference Code (Inside Order Section)
            xwWriter.WriteElementString("BillingReferenceCode", sBillingReference);

            // Subject Section (Inside Order Section)
            xwWriter.WriteStartElement("Subject");
            xwWriter.WriteElementString("Firstname", sFirstName);
            if( sMiddleName != null ) xwWriter.WriteElementString("Middlename", sMiddleName);
            xwWriter.WriteElementString("Lastname", sLastName);
            if (sGeneration != null) xwWriter.WriteElementString("Generation", sGeneration);
            xwWriter.WriteElementString("DOB", sDOB);
            xwWriter.WriteElementString("SSN", sSSN);
            xwWriter.WriteElementString("Gender", sGender);
            xwWriter.WriteElementString("Ethnicity", "Caucasian");
            xwWriter.WriteElementString("ApplicantPosition", "Volunteer");

            // CurrentAddress Section (Inside Subject Section)
            xwWriter.WriteStartElement("CurrentAddress");
            xwWriter.WriteElementString("StreetAddress", sAddress);
            xwWriter.WriteElementString("City", sCity);
            xwWriter.WriteElementString("State", sState);
            xwWriter.WriteElementString("Zipcode", sZip);
            xwWriter.WriteFullEndElement();

            // Close Subject Section
            xwWriter.WriteFullEndElement();

            // Package Service Code - Only if a package (BASIC,PLUS) (Inside Order Section)
            xwWriter.WriteStartElement("PackageServiceCode");
            xwWriter.WriteAttributeString("OrderId", sOrderID);
            xwWriter.WriteString("Basic");
            xwWriter.WriteFullEndElement();

            // Order Detail (Inside Order Section)
            xwWriter.WriteStartElement("OrderDetail");
            xwWriter.WriteAttributeString("serviceCode", "combo");
            xwWriter.WriteAttributeString("OrderId", sOrderID);
            xwWriter.WriteEndElement();

            // Close Order Section
            xwWriter.WriteFullEndElement();

            // Close OrderXML Section
            xwWriter.WriteFullEndElement();

            // Close Document
            xwWriter.WriteEndDocument();

            xwWriter.Flush();
            return true;
        }

        public bool submitRequest()
        {
            if (!bInitialized) return false;

            xmlCreate();

            //string sXML = HttpUtility.UrlEncode( Encoding.UTF8.GetString( msRequest.GetBuffer() ) );\
            string sXML = Encoding.UTF8.GetString( msRequest.GetBuffer() );

            File.WriteAllText(@"C:\RequestXML.txt", sXML);

            var fields = new NameValueCollection();
            fields.Add("REQUEST", sXML);

            WebClient wc = new WebClient();
            var response = Encoding.UTF8.GetString( wc.UploadValues( "http://dev.studio-lfp.com/api/pmm.php", "POST", fields) );

            return true;
        }

        public void processResponse( string sResponse )
        {
        }
    }
}
