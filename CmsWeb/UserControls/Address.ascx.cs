using System;
using UtilityExtensions;
using CMSPresenter;
using System.Text.RegularExpressions;
using CmsData;
using System.Linq;
using System.Collections.Generic;

namespace CMSWeb
{
    public partial class Address : System.Web.UI.UserControl
    {
        ///////////////////////////////////////////////////
        //Constants
        ///////////////////////////////////////////////////

        //Binding Sources
        private const string cFamily = "person.family";
        private const string cPerson = "person";

        //Binding Members
        private const string cAddressLineOne = "AddressLineOne";
        private const string cAddressLineTwo = "AddressLineTwo";
        private const string cCityName = "CityName";
        private const string cStateCode = "StateCode";
        private const string cZipCode = "ZipCode";
        private const string cAltAddressLineOne = "AltAddressLineOne";
        private const string cAltAddressLineTwo = "AltAddressLineTwo";
        private const string cAltCityName = "AltCityName";
        private const string cAltStateCode = "AltStateCode";
        private const string cAltZipCode = "AltZipCode";
        private const string cToDate = "AddressToDate";
        private const string cFromDate = "AddressFromDate";
        private const string cAltToDate = "AltAddressToDate";
        private const string cAltFromDate = "AltAddressFromDate";
        private const string cBadAddrFlag = "BadAddressFlag";
        private const string cAltBadAddrFlag = "AltBadAddressFlag";
        private const string cResCodeId = "ResCodeId";
        private const string cAltResCodeId = "AltResCodeId";
        ///////////////////////////////////////////////////


        public bool showPreferredAddress = true;

        public CmsData.Person person { get; set; }
        public enum AddressTypes { Personal = 30, PersonalAlternate = 40, Family = 10, FamilyAlternate = 20 };
        public AddressTypes AddressType { get; set; }
        public int AddressIndex;
        public string Checked;
        public CustomControls.DisplayOrEditDropDown PreferredAddressControl { get; set; }

        protected void setControls()
        {
            State.DataSource = CodeValueController.GetStateList();
            ResidentCode.DataSource = CodeValueController.ResidentCodesWithZero();
            switch (AddressType)
            {
                case AddressTypes.Family:
                    Line1.BindingSource = cFamily;
                    Line1.BindingMember = cAddressLineOne;

                    Line2.BindingSource = cFamily;
                    Line2.BindingMember = cAddressLineTwo;

                    City.BindingSource = cFamily;
                    City.BindingMember = cCityName;

                    State.BindingSource = cFamily;
                    State.BindingMember = cStateCode;

                    Zip.BindingSource = cFamily;
                    Zip.BindingMember = cZipCode;

                    FromDate.BindingSource = cFamily;
                    FromDate.BindingMember = cFromDate;

                    ToDate.BindingSource = cFamily;
                    ToDate.BindingMember = cToDate;

                    BadAddressFlag.BindingSource = cFamily;
                    BadAddressFlag.BindingMember = cBadAddrFlag;

                    ResidentCode.BindingSource = cFamily;
                    ResidentCode.BindingMember = cResCodeId;

                    break;

                case AddressTypes.FamilyAlternate:
                    Line1.BindingSource = cFamily;
                    Line1.BindingMember = cAltAddressLineOne;

                    Line2.BindingSource = cFamily;
                    Line2.BindingMember = cAltAddressLineTwo;

                    City.BindingSource = cFamily;
                    City.BindingMember = cAltCityName;

                    State.BindingSource = cFamily;
                    State.BindingMember = cAltStateCode;

                    Zip.BindingSource = cFamily;
                    Zip.BindingMember = cAltZipCode;

                    FromDate.BindingSource = cFamily;
                    FromDate.BindingMember = cAltFromDate;

                    ToDate.BindingSource = cFamily;
                    ToDate.BindingMember = cAltToDate;

                    BadAddressFlag.BindingSource = cFamily;
                    BadAddressFlag.BindingMember = cAltBadAddrFlag;

                    ResidentCode.BindingSource = cFamily;
                    ResidentCode.BindingMember = cAltResCodeId;

                    break;

                case AddressTypes.Personal:
                    Line1.BindingSource = cPerson;
                    Line1.BindingMember = cAddressLineOne;

                    Line2.BindingSource = cPerson;
                    Line2.BindingMember = cAddressLineTwo;

                    City.BindingSource = cPerson;
                    City.BindingMember = cCityName;

                    State.BindingSource = cPerson;
                    State.BindingMember = cStateCode;

                    Zip.BindingSource = cPerson;
                    Zip.BindingMember = cZipCode;

                    FromDate.BindingSource = cPerson;
                    FromDate.BindingMember = cFromDate;

                    ToDate.BindingSource = cPerson;
                    ToDate.BindingMember = cToDate;

                    BadAddressFlag.BindingSource = cPerson;
                    BadAddressFlag.BindingMember = cBadAddrFlag;

                    ResidentCode.BindingSource = cPerson;
                    ResidentCode.BindingMember = cResCodeId;

                    break;

                case AddressTypes.PersonalAlternate:
                    Line1.BindingSource = cPerson;
                    Line1.BindingMember = cAltAddressLineOne;

                    Line2.BindingSource = cPerson;
                    Line2.BindingMember = cAltAddressLineTwo;

                    City.BindingSource = cPerson;
                    City.BindingMember = cAltCityName;

                    State.BindingSource = cPerson;
                    State.BindingMember = cAltStateCode;

                    Zip.BindingSource = cPerson;
                    Zip.BindingMember = cAltZipCode;

                    FromDate.BindingSource = cPerson;
                    FromDate.BindingMember = cAltFromDate;

                    ToDate.BindingSource = cPerson;
                    ToDate.BindingMember = cAltToDate;

                    BadAddressFlag.BindingSource = cPerson;
                    BadAddressFlag.BindingMember = cAltBadAddrFlag;

                    ResidentCode.BindingSource = cFamily;
                    ResidentCode.BindingMember = cAltResCodeId;

                    break;
                default:
                    Line1.BindingSource = cFamily;
                    Line1.BindingMember = cAddressLineOne;

                    Line2.BindingSource = cFamily;
                    Line2.BindingMember = cAddressLineTwo;

                    City.BindingSource = cFamily;
                    City.BindingMember = cCityName;

                    State.BindingSource = cFamily;
                    State.BindingMember = cStateCode;

                    Zip.BindingSource = cFamily;
                    Zip.BindingMember = cZipCode;

                    FromDate.BindingSource = cFamily;
                    FromDate.BindingMember = cFromDate;

                    ToDate.BindingSource = cFamily;
                    ToDate.BindingMember = cToDate;

                    BadAddressFlag.BindingSource = cFamily;
                    BadAddressFlag.BindingMember = cBadAddrFlag;

                    ResidentCode.BindingSource = cFamily;
                    ResidentCode.BindingMember = cResCodeId;

                    break;
            }
            
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            setControls();
        }
        public string Disabled;
        public string OnChange;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PreferredAddressControl != null)
            {
                var s = AddressType.ToInt().ToString();
                var i = PreferredAddressControl.Items.FindByValue(s);
                AddressIndex = PreferredAddressControl.Items.IndexOf(i);
                Checked = PreferredAddressControl.SelectedValue == s ? "checked" : "";
                Disabled = PreferredAddressControl.Editing ? "" : "disabled";
                OnChange = PreferredAddressControl.Editing ? "onchange=\"prefchange('{0}', '{1}')\""
                    .Fmt(PreferredAddressControl.ClientID, AddressIndex) : "";
            }
            else
                trPreferredAddress.Visible = false;

            if (Line1.Editing)
                VerifyAddress2.Text = "<input type=\"button\" value=\"Verfiy Address\" onclick=\"javascript:PageMethods.VerifyAddress('{0}', {0}.value, {1}.value, {2}.value, {3}.value, {4}.value, VerifyCallback)\" />"
                    .Fmt(Line1.ClientID, Line2.ClientID, City.ClientID, State.ClientID, Zip.ClientID);
            else
                VerifyAddress2.Text = "";
        }

        protected void Zip_TextChanged(object sender, EventArgs e)
        {
            if (Zip.Text.Length >= 5)
            {
                var z5 = Zip.Text.Substring(0,5);
                var z = DbUtil.Db.Zips.SingleOrDefault(zip => z5 == zip.ZipCode);
                if (z == null)
                    ResidentCode.SelectedValue = "30";
                else
                    ResidentCode.SelectedValue = z.MetroMarginalCode.ToString();
                ResidentCode.ChangedStatus = true;
                ResidentCode.DataBind();
            }
        }
    }
}