using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using UtilityExtensions;
using CmsData;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace CmsData
{
	public class RegSettings
	{
		public enum RegKeywords
		{
			Shell,
			Fee,
			AskSuggestedFee,
			Deposit,
			ShirtFee,
			ExtraFee,
			MaximumFee,
			AllowOnlyOne,
			AllowReRegister,
			OrgMemberFees,
			AskTickets,
			AskRequest,
			GradeOptions,
			AgeGroups,
			OrgFees,
			AskShirtSize,
			AllowLastYearShirt,
			ShirtSizes,
			YesNoQuestions,
			ExtraQuestions,
			MenuItems,
			ValidateOrgs,
			MemberOnly,
			LinkGroupsFromOrgs,
			AskParents,
			AskDoctor,
			AskInsurance,
			AskEmContact,
			AskAllergies,
			AskMedical,
			AskChurch,
			AskTylenolEtc,
			AskCoaching,
			AskGrade,
			AskDonation,
			DonationLabel,
			DonationFundId,
			GroupToJoin,
			GiveOrgMembAccess,
			NotReqDOB,
			NotReqAddr,
			NotReqZip,
			NotReqPhone,
			NotReqGender,
			NotReqMarital,
			AskOptions,
			ExtraOptions,
			Dropdown1,
			Dropdown2,
			Dropdown3,
			Limit,
			Confirmation,
			Terms,
			Label,
			Time,
			DayOfWeek,
			TimeSlots,
			Options,
			Code,
			SmallGroup,
			Checkboxes,
			Checkboxes2,
			Minimum,
			Maximum,
			Items,
			Subject,
			Body,
			VoteTags,
			Confirm,
			Message,
			Display,
			Instructions,
			Select,
			Find,
			Login,
			Submit,
			Sorry,
			Html,
			Title,
			None,
		}
		int lineno;
		string[] lines;
		readonly Regex skipline = new Regex(@"^(?:\s*)$|^(?:\s*\#.*)$");
		readonly Regex linedata = new Regex(@"^(?<sp>\s*)(?:(?<keyword>[a-zA-Z]\w*):)?\s*(?<val>.*)$");
		readonly Regex agerange = new Regex(@"(?<start>\d+)-(?<end>\d+)");
		class LineData
		{
			public int indent { get; set; }
			public string line { get; set; }
			public int lineno { get; set; }
			public RegKeywords kw { get; set; }
			public string value { get; set; }
			public bool html { get; set; }
			public override string ToString()
			{
				return line;
			}
		}

		#region Properties
		public string Title { get; set; }
		public string Shell { get; set; }
		public decimal? Fee { get; set; }
		public bool SuggestedFee { get; set; }
		public decimal? Deposit { get; set; }
		public decimal? ShirtFee { get; set; }
		public decimal? ExtraFee { get; set; }
		public decimal? MaximumFee { get; set; }
		public bool AllowOnlyOne { get; set; }
		public bool AllowReRegister { get; set; }
		public bool AskTickets { get; set; }
		public string NumItemsLabel { get; set; }
		public bool AskRequest { get; set; }
		public string RequestLabel { get; set; }
		public bool AskShirtSize { get; set; }
		public bool AllowLastYearShirt { get; set; }
		public int? CheckboxMin { get; set; }
		public int? CheckboxMax { get; set; }
		public int? Checkbox2Min { get; set; }
		public int? Checkbox2Max { get; set; }
		public string CheckBoxLabel { get; set; }
		public string CheckBox2Label { get; set; }
		public string GradeLabel { get; set; }
		public string GradeOptionsLabel { get; set; }
		public string SuggestedFeeLabel { get; set; }
		public string MenuItemsLabel { get; set; }
		public string Dropdown1Label { get; set; }
		public string Dropdown2Label { get; set; }
		public string Dropdown3Label { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public string Terms { get; set; }

		public bool MemberOnly { get; set; }
		public bool AskParents { get; set; }
		public bool AskDoctor { get; set; }
		public bool AskInsurance { get; set; }
		public bool AskEmContact { get; set; }
		public bool AskAllergies { get; set; }
		public bool AskMedical { get; set; }
		public bool AskChurch { get; set; }
		public bool AskTylenolEtc { get; set; }
		public bool AskCoaching { get; set; }
		//public bool AskGrade { get; set; }
		public bool AskDonation { get; set; }
		public string DonationLabel { get; set; }
		public bool NotReqDOB { get; set; }
		public bool NotReqAddr { get; set; }
		public bool NotReqPhone { get; set; }
		public bool NotReqGender { get; set; }
		public bool NotReqMarital { get; set; }
		public bool NotReqZip { get; set; }
		public int? DonationFundId { get; set; }
		public string GroupToJoin { get; set; }
		public bool GiveOrgMembAccess { get; set; }

		public string DonationFund()
		{
			return (from f in DbUtil.Db.ContributionFunds
					where f.FundId == DonationFundId
					select f.FundName).SingleOrDefault();
		}

		public string InstructionSelect { get; set; }
		public string InstructionFind { get; set; }
		public string InstructionOptions { get; set; }
		public string InstructionLogin { get; set; }
		public string InstructionSubmit { get; set; }
		public string InstructionSorry { get; set; }
		public string InstructionAll { get; set; }

		private List<OrgFee> _OrgFees;
		public List<OrgFee> OrgFees
		{
			get { return _OrgFees; }
		}
		private List<GradeOption> _GradeOptions;
		public List<GradeOption> GradeOptions
		{
			get { return _GradeOptions; }
		}
		private List<AgeGroup> _AgeGroups;
		public List<AgeGroup> AgeGroups
		{
			get { return _AgeGroups; }
		}
		private List<ShirtSize> _ShirtSizes;
		public List<ShirtSize> ShirtSizes
		{
			get { return _ShirtSizes; }
		}
		private List<YesNoQuestion> _YesNoQuestions;
		public List<YesNoQuestion> YesNoQuestions
		{
			get { return _YesNoQuestions; }
		}
		private List<MenuItem> _Checkboxes;
		public List<MenuItem> Checkboxes
		{
			get { return _Checkboxes; }
		}
		private List<TimeSlot> _TimeSlots;
		public List<TimeSlot> TimeSlots
		{
			get { return _TimeSlots; }
		}
		private List<MenuItem> _Checkboxes2;
		public List<MenuItem> Checkboxes2
		{
			get { return _Checkboxes2; }
		}
		private List<ExtraQuestion> _ExtraQuestions;
		public List<ExtraQuestion> ExtraQuestions
		{
			get { return _ExtraQuestions; }
		}
		private List<MenuItem> _MenuItems;
		public List<MenuItem> MenuItems
		{
			get { return _MenuItems; }
		}
		private List<MenuItem> _Dropdown1;
		public List<MenuItem> Dropdown1
		{
			get { return _Dropdown1; }
		}
		private List<MenuItem> _Dropdown2;
		public List<MenuItem> Dropdown2
		{
			get { return _Dropdown2; }
		}
		private List<MenuItem> _Dropdown3;
		public List<MenuItem> Dropdown3
		{
			get { return _Dropdown3; }
		}
		private List<int> _LinkGroupsFromOrgs;
		public List<int> LinkGroupsFromOrgs
		{
			get { return _LinkGroupsFromOrgs; }
		}
		private List<int> _ValidateOrgs;
		public List<int> ValidateOrgIds
		{
			get { return _ValidateOrgs; }
		}
		public string ValidateOrgs
		{
			get { return string.Join(",", _ValidateOrgs); }
			set
			{
				if (value.HasValue())
					_ValidateOrgs = (from i in value.Split(',')
									 where i.ToInt() > 0
									 select i.ToInt()).ToList();
				else
					_ValidateOrgs = new List<int>();
			}
		}
		private List<VoteTag> _VoteTags;
		public List<VoteTag> VoteTags
		{
			get { return _VoteTags; }
		}

		#endregion

		public class YesNoQuestion
		{
			public int Id { get; set; }
			public string Question { get; set; }
			public string SmallGroup { get; set; }
		}
		public class OrgFee
		{
			public int OrgId { get; set; }
			public decimal? Fee { get; set; }
		}
		public class GradeOption
		{
			public int Id { get; set; }
			public string Description { get; set; }
			public int Code { get; set; }
		}
		public class HtmlText2
		{
			public string Text { get; set; }
			public string Html { get; set; }
			public override string ToString()
			{
				if (Html.HasValue())
					return Html;
				return Text;
			}
		}
		public class AgeGroup
		{
			public int Id { get; set; }
			public string SmallGroup { get; set; }
			public int StartAge { get; set; }
			public int EndAge { get; set; }
			public decimal? Fee { get; set; }
		}
		public class ShirtSize
		{
			public int Id { get; set; }
			public string Description { get; set; }
			public string SmallGroup { get; set; }
		}
		public class ExtraQuestion
		{
			public int Id { get; set; }
			public string Question { get; set; }
		}
		public class MenuItem
		{
			public int Id { get; set; }
			public string Description { get; set; }
			public string SmallGroup { get; set; }
			public decimal? Fee { get; set; }
			public int? Limit { get; set; }
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yy h:mm tt}")]
			public DateTime? MeetingTime { get; set; }
		}
		public class TimeSlot
		{
			public int Id { get; set; }
			public string Description { get; set; }
			public int DayOfWeek { get; set; }
			public int? Limit { get; set; }
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:h:mm tt}")]
			public DateTime? Time { get; set; }
			public DateTime Datetime()
			{
				var dt = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
				return Time != null ?
					dt.AddDays(DayOfWeek).Add(Time.Value.TimeOfDay)
					: new DateTime();
			}

			public DateTime Datetime(DateTime dt)
			{
				return Time != null ?
					dt.AddDays(DayOfWeek).Add(Time.Value.TimeOfDay)
					: new DateTime();
			}
		}
		public class VoteTag
		{
			public int Id { get; set; }
			public string Display { get; set; }
			public bool Confirm { get; set; }
			public string Message { get; set; }
			public string SmallGroup { get; set; }
		}

		List<LineData> data = new List<LineData>();

		Exception GetException(string msg)
		{
			var stackTrace = new StackTrace();
			var name = stackTrace.GetFrame(1).GetMethod().Name;
			return new Exception("{0}: {1}".Fmt(name, msg));
		}

		public RegSettings()
		{
			var keysections = Enum.GetNames(typeof(RegKeywords)).ToList();

			_OrgFees = new List<OrgFee>();
			_GradeOptions = new List<GradeOption>();
			_AgeGroups = new List<AgeGroup>();
			_ShirtSizes = new List<ShirtSize>();
			_YesNoQuestions = new List<YesNoQuestion>();
			_TimeSlots = new List<TimeSlot>();
			_Checkboxes = new List<MenuItem>();
			_Checkboxes2 = new List<MenuItem>();
			_ExtraQuestions = new List<ExtraQuestion>();
			_MenuItems = new List<MenuItem>();
			_Dropdown1 = new List<MenuItem>();
			_Dropdown2 = new List<MenuItem>();
			_Dropdown3 = new List<MenuItem>();
			_LinkGroupsFromOrgs = new List<int>();
			_ValidateOrgs = new List<int>();
			_VoteTags = new List<VoteTag>();
		}
		public static string[] SplitLines(string s)
		{
			var splitlines = new Regex(@"\r?\n");
			if (!s.HasValue())
				return new string[] { };
			var lines = splitlines.Split(s);
			return lines;
		}
		public Organization org { get; set; }
		public int OrgId { get; set; }
		public CMSDataContext Db { get; set; }
		public RegSettings(string s, CMSDataContext Db, int OrgId, bool check = false)
			: this()
		{
			this.Db = Db;
			this.OrgId = OrgId;
			lines = SplitLines(s);
			for (lineno = 0; lineno < lines.Length; )
			{
				var d = GetLineData();
				if (d == null)
					break;
				data.Add(d);
			}
			lines = null;
			lineno = 0;
			while (lineno < data.Count)
			{
				if (curr.kw == RegKeywords.None)
					throw GetException("unknown keyword");
				if (curr.indent > 0)
					throw GetException("expected no indent");
				ParseSection();
			}
			data = null;
			if ((AskShirtSize || ShirtFee > 0) && ShirtSizes.Count == 0)
				throw GetException("Need Shirt Sizes");

			if (check)
			{
				var q = ShirtSizes.GroupBy(mi => mi.SmallGroup).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
				if (q.Any())
					throw GetException("Duplicate SmallGroup in ShirtSizes: " + string.Join(",", q));

				CheckDupSmallGroup(Checkboxes, "Checkboxes");
				CheckDupSmallGroup(Checkboxes2, "Checkboxes2");
				CheckDupSmallGroup(MenuItems, "MenuItems");
				CheckDupSmallGroup(Dropdown1, "Dropdown1");
				CheckDupSmallGroup(Dropdown2, "Dropdown2");
				CheckDupSmallGroup(Dropdown3, "Dropdown3");
			}
		}
		private void CheckDupSmallGroup(IEnumerable<MenuItem> list, string name)
		{
			var q = list.GroupBy(mi => mi.SmallGroup).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
			if (q.Any())
				throw GetException("Duplicate SmallGroup in {0}: {1}".Fmt(name, string.Join(",", q)));
		}
		LineData GetLineData()
		{
			while (lineno < lines.Length && skipline.IsMatch(curline))
			{
				lineno++;
				continue;
			}
			if (lineno >= lines.Length)
				return null;
			var m = linedata.Match(curline);
			if (m.Success)
			{
				var d = new LineData();
				d.line = curline.Trim();
				lineno++;
				d.lineno = lineno;
				d.indent = m.Groups["sp"].Value.Length;
				d.value = m.Groups["val"].Value.TrimEnd();
				var keyword = m.Groups["keyword"].Value;

				RegKeywords kw;
				if (!Enum.TryParse<RegKeywords>(keyword, true, out kw))
					kw = RegKeywords.None;
				d.kw = kw;
				if (d.value == "<<")
				{
					var lookfor = curline.Trim();
					lineno++;
					d.html = true;
					var sb = new StringBuilder();
					while (lineno < lines.Length && curline.Trim() != lookfor)
					{
						sb.Append(curline.Trim());
						lineno++;
					}
					d.value = sb.ToString();
					lineno++;
				}
				return d;
			}
			return null;
		}

		string curline
		{
			get
			{
				if (lineno < lines.Length)
					return lines[lineno];
				return null;
			}
		}
		LineData curr
		{
			get
			{
				if (lineno < data.Count)
					return data[lineno];
				return new LineData();
			}
		}
		LineData prev { get { return data[lineno - 1]; } }

		void ParseSection()
		{
			switch (curr.kw)
			{
				case RegKeywords.MemberOnly:
					MemberOnly = GetBool();
					break;
				case RegKeywords.AskParents:
					AskParents = GetBool();
					break;
				case RegKeywords.AskDoctor:
					AskDoctor = GetBool();
					break;
				case RegKeywords.AskInsurance:
					AskInsurance = GetBool();
					break;
				case RegKeywords.AskEmContact:
					AskEmContact = GetBool();
					break;
				case RegKeywords.AskAllergies:
					AskAllergies = GetBool();
					break;
				case RegKeywords.AskMedical:
					AskMedical = GetBool();
					break;
				case RegKeywords.AskChurch:
					AskChurch = GetBool();
					break;
				case RegKeywords.AskTylenolEtc:
					AskTylenolEtc = GetBool();
					break;
				case RegKeywords.AskCoaching:
					AskCoaching = GetBool();
					break;
				case RegKeywords.AskDonation:
					AskDonation = GetBool();
					break;
				case RegKeywords.NotReqDOB:
					NotReqDOB = GetBool();
					break;
				case RegKeywords.NotReqAddr:
					NotReqAddr = GetBool();
					break;
				case RegKeywords.NotReqPhone:
					NotReqPhone = GetBool();
					break;
				case RegKeywords.NotReqGender:
					NotReqGender = GetBool();
					break;
				case RegKeywords.NotReqMarital:
					NotReqMarital = GetBool();
					break;
				case RegKeywords.NotReqZip:
					NotReqZip = GetBool();
					break;
				case RegKeywords.DonationFundId:
					DonationFundId = GetNullInt();
					break;
				case RegKeywords.DonationLabel:
					DonationLabel = ParseMessage();
					break;
				case RegKeywords.GroupToJoin:
					GroupToJoin = GetString();
					break;
				case RegKeywords.GiveOrgMembAccess:
					GiveOrgMembAccess = GetBool();
					break;
				case RegKeywords.LinkGroupsFromOrgs:
					_LinkGroupsFromOrgs = (from i in curr.value.Split(',')
										   where i.ToInt() > 0
										   select i.ToInt()).ToList();
					lineno++;
					break;
				case RegKeywords.ValidateOrgs:
					_ValidateOrgs = (from i in curr.value.Split(',')
									 where i.ToInt() > 0
									 select i.ToInt()).ToList();
					lineno++;
					break;

				case RegKeywords.Terms:
					ParseTerms();
					break;
				case RegKeywords.Instructions:
					ParseInstructions();
					break;
				case RegKeywords.Confirmation:
					ParseConfirmation();
					break;
				case RegKeywords.AskOptions:
				case RegKeywords.Dropdown1:
					Dropdown1Label = GetString("Options");
					_Dropdown1 = ParseMenuItems("Dropdown1");
					break;
				case RegKeywords.ExtraOptions:
				case RegKeywords.Dropdown2:
					Dropdown2Label = GetString("Options 2");
					_Dropdown2 = ParseMenuItems("Dropdown2");
					break;
				case RegKeywords.Dropdown3:
					Dropdown3Label = GetString("Options 3");
					_Dropdown3 = ParseMenuItems("Dropdown3");
					break;
				case RegKeywords.MenuItems:
					MenuItemsLabel = GetString("Menu");
					_MenuItems = ParseMenuItems("MenuItems");
					break;
				case RegKeywords.ExtraQuestions:
					ParseExtraQuestions();
					break;
				case RegKeywords.TimeSlots:
					var ts = GetString("TimeSlots");
					_TimeSlots = ParseTimeSlots();
					break;
				case RegKeywords.Checkboxes:
					CheckBoxLabel = GetString("CheckBoxes");
					CheckboxMin = GetInt(RegKeywords.Minimum);
					CheckboxMax = GetInt(RegKeywords.Maximum);
					_Checkboxes = ParseMenuItems("Checkboxes");
					break;
				case RegKeywords.Checkboxes2:
					CheckBox2Label = GetString("More CheckBoxes");
					Checkbox2Min = GetInt(RegKeywords.Minimum);
					Checkbox2Max = GetInt(RegKeywords.Maximum);
					_Checkboxes2 = ParseMenuItems("Checkboxes2");
					break;
				case RegKeywords.YesNoQuestions:
					ParseYesNoQuestions();
					break;
				case RegKeywords.AskShirtSize:
					AskShirtSize = GetBool();
					if (AskShirtSize && ShirtSizes == null)
						AddStandardSizes();
					break;
				case RegKeywords.ShirtSizes:
					ParseShirtSizes();
					break;
				case RegKeywords.AgeGroups:
					ParseAgeGroups();
					break;
				case RegKeywords.OrgFees:
					ParseOrgMemberFees();
					break;
				case RegKeywords.GradeOptions:
					ParseGradeOptions();
					break;
				case RegKeywords.VoteTags:
					ParseVoteTags();
					break;

				case RegKeywords.AskRequest:
					AskRequest = GetBool();
					RequestLabel = GetLabel("Request");
					break;
				case RegKeywords.AskGrade:
					GetBool();
					GetLabel("Grade");
					break;
				case RegKeywords.Title:
					Title = GetString();
					break;
				case RegKeywords.Shell:
					Shell = GetString();
					break;
				case RegKeywords.Fee:
					Fee = GetDecimal();
					break;
				case RegKeywords.AskSuggestedFee:
					SuggestedFee = GetBool();
					SuggestedFeeLabel = GetLabel("Suggested Amount");
					break;
				case RegKeywords.AllowLastYearShirt:
					AllowLastYearShirt = GetBool();
					break;
				case RegKeywords.Deposit:
					Deposit = GetDecimal();
					break;
				case RegKeywords.ShirtFee:
					ShirtFee = GetDecimal();
					break;
				case RegKeywords.ExtraFee:
					ExtraFee = GetDecimal();
					break;
				case RegKeywords.MaximumFee:
					MaximumFee = GetDecimal();
					break;
				case RegKeywords.AllowOnlyOne:
					AllowOnlyOne = GetBool();
					break;
				case RegKeywords.AllowReRegister:
					AllowReRegister = GetBool();
					break;
				case RegKeywords.OrgMemberFees:
					ParseOrgMemberFees();
					break;
				case RegKeywords.AskTickets:
					AskTickets = GetBool();
					NumItemsLabel = GetLabel("No. of Items");
					break;
			}
		}
		private string GetString()
		{
			return GetString(null);
		}
		private string GetString(string def)
		{
			var s = curr.value;
			if (def.HasValue() && !s.HasValue())
				s = def;
			lineno++;
			return s;
		}
		private string GetLine()
		{
			var s = curr.line;
			lineno++;
			return s;
		}
		private decimal? GetDecimal()
		{
			decimal d;
			if (!decimal.TryParse(curr.value, out d))
				throw GetException("expected numeric value");
			lineno++;
			return d;
		}
		private int GetInt()
		{
			if (!curr.value.HasValue())
				throw GetException("expected integer value");
			int i;
			if (!int.TryParse(curr.value, out i))
				throw GetException("expected integer value");
			lineno++;
			return i;
		}
		private DateTime GetTime()
		{
			if (!curr.value.HasValue())
				throw GetException("expected time value");
			DateTime i;
			if (!DateTime.TryParse(curr.value, out i))
				throw GetException("expected time value");
			lineno++;
			return i;
		}
		private DateTime GetDateTime()
		{
			if (!curr.value.HasValue())
				throw GetException("expected datetime value");
			DateTime i;
			if (!DateTime.TryParse(curr.value, out i))
				throw GetException("expected datetime value");
			lineno++;
			return i;
		}
		private int? GetNullInt()
		{
			if (!curr.value.HasValue())
			{
				lineno++;
				return null;
			}
			int i;
			if (!int.TryParse(curr.value, out i))
				throw GetException("expected integer value");
			lineno++;
			return i;
		}
		private bool GetBool()
		{
			if (!curr.value.HasValue())
			{
				lineno++;
				return false;
			}
			bool b;
			if (!bool.TryParse(curr.value, out b))
				throw GetException("expected true/false");
			lineno++;
			return b;
		}
		private string GetLabel(string def)
		{
			if (curr.indent > 0)
			{
				if (curr.kw != RegKeywords.Label)
					throw GetException("expected Label");
				var label = curr.value;
				lineno++;
				return label;
			}
			return def;
		}
		private int? GetInt(RegKeywords kw)
		{
			if (curr.indent > 0)
			{
				if (curr.kw != kw)
					return null;
				return GetNullInt();
			}
			return null;
		}

		private void ParseTerms()
		{
			Terms = ParseMessage();
			if (Terms.HasValue() || curr.indent == 0)
				return;
			var startindent = curr.indent;
			while (curr.indent == startindent)
			{
				switch (curr.kw)
				{
					case RegKeywords.Html:
						lineno++;
						break;
					case RegKeywords.Body:
						Terms = ParseMessage();
						break;
					default:
						throw GetException("unexpected line");
				}
			}
		}
		private void ParseConfirmation()
		{
			lineno++;
			var startindent = curr.indent;
			while (curr.indent == startindent)
			{
				switch (curr.kw)
				{
					case RegKeywords.Html:
						lineno++;
						break;
					case RegKeywords.Subject:
						Subject = GetString();
						break;
					case RegKeywords.Body:
						Body = ParseMessage();
						break;
					default:
						throw GetException("unexpected line in Confirmation");
				}
			}
		}
		private string ParseMessage()
		{
			return GetString();
		}
		private void ParseInstructions()
		{
			lineno++;
			if (curr.indent == 0)
				return;
			var startindent = curr.indent;
			while (curr.indent == startindent)
			{
				switch (curr.kw)
				{
					case RegKeywords.Html:
						lineno++;
						break;
					case RegKeywords.Select:
						InstructionSelect = ParseMessage();
						break;
					case RegKeywords.Find:
						InstructionFind = ParseMessage();
						break;
					case RegKeywords.Options:
						InstructionOptions = ParseMessage();
						break;
					case RegKeywords.Login:
						InstructionLogin = ParseMessage();
						break;
					case RegKeywords.Submit:
						InstructionSubmit = ParseMessage();
						break;
					case RegKeywords.Sorry:
						InstructionSorry = ParseMessage();
						break;
					case RegKeywords.Body:
						InstructionAll = ParseMessage();
						break;
					default:
						throw GetException("unexpected line");
				}
			}
		}
		private void ParseVoteTags()
		{
			lineno++;
			if (curr.indent == 0)
				return;
			var startindent = curr.indent;
			while (curr.indent == startindent)
			{
				var votetag = new VoteTag();
				if (curr.kw != RegKeywords.Display)
					throw GetException("unexpected line");
				VoteTags.Add(votetag);
				votetag.Display = ParseMessage();
				if (curr.indent <= startindent)
					throw GetException("expected indented Message,Confirm, or SmallGroup");
				var ind = curr.indent;
				while (curr.indent == ind)
				{
					switch (curr.kw)
					{
						case RegKeywords.Message:
							votetag.Message = GetString();
							break;
						case RegKeywords.Confirm:
							votetag.Confirm = GetBool();
							break;
						case RegKeywords.SmallGroup:
							votetag.SmallGroup = GetString();
							break;
						default:
							throw GetException("unexpected line");
					}
				}
				if (!votetag.Message.HasValue())
					throw GetException("expected Message");
				if (!votetag.SmallGroup.HasValue())
					throw GetException("expected SmallGroup");
			}
		}
		private List<MenuItem> ParseMenuItems(string section)
		{
			var list = new List<MenuItem>();
			if (curr.indent == 0)
				return list;
			var startindent = curr.indent;
			while (curr.indent == startindent)
			{
				var menuitem = new MenuItem();
				if (curr.kw != RegKeywords.None)
					throw GetException("unexpected line in " + section);
				list.Add(menuitem);
				menuitem.Description = GetLine();
				menuitem.SmallGroup = menuitem.Description;
				if (curr.indent <= startindent)
					continue;
				var ind = curr.indent;
				while (curr.indent == ind)
				{
					switch (curr.kw)
					{
						case RegKeywords.SmallGroup:
							menuitem.SmallGroup = GetString(menuitem.Description);
							break;
						case RegKeywords.Fee:
							menuitem.Fee = GetDecimal();
							break;
						case RegKeywords.Limit:
							menuitem.Limit = GetNullInt();
							break;
						case RegKeywords.Time:
							menuitem.MeetingTime = GetDateTime();
							break;
						default:
							throw GetException("unexpected line in " + section);
					}
				}
			}
			return list;
		}
		private List<TimeSlot> ParseTimeSlots()
		{
			var list = new List<TimeSlot>();
			if (curr.indent == 0)
				return list;
			var startindent = curr.indent;
			while (curr.indent == startindent)
			{
				var timeslot = new TimeSlot();
				if (curr.kw != RegKeywords.None)
					throw GetException("unexpected line in TimeSlots");
				list.Add(timeslot);
				timeslot.Description = GetLine();
				if (curr.indent <= startindent)
					continue;
				var ind = curr.indent;
				while (curr.indent == ind)
				{
					switch (curr.kw)
					{
						case RegKeywords.Time:
							timeslot.Time = GetTime();
							break;
						case RegKeywords.DayOfWeek:
							timeslot.DayOfWeek = GetInt();
							break;
						case RegKeywords.Limit:
							timeslot.Limit = GetInt();
							break;
						default:
							throw GetException("unexpected line in TimeSlot");
					}
				}
			}
			return list;
		}
		private void ParseExtraQuestions()
		{
			lineno++;
			if (curr.indent == 0)
				return;
			var startindent = curr.indent;
			while (curr.indent == startindent)
			{
				if (curr.kw != RegKeywords.None)
					throw GetException("unexpected line");
				ExtraQuestions.Add(new ExtraQuestion { Question = GetLine() });
			}
		}
		private void ParseYesNoQuestions()
		{
			lineno++;
			if (curr.indent == 0)
				return;
			var startindent = curr.indent;
			int n = 0;
			while (curr.indent == startindent)
			{
				var yesnoquestion = new YesNoQuestion();
				if (curr.kw != RegKeywords.None)
					throw GetException("unexpected line");
				yesnoquestion.Question = GetLine();
				if (curr.indent <= startindent)
					throw GetException("Expected SmallGroup indented");
				if (curr.kw != RegKeywords.SmallGroup)
					throw GetException("Expected SmallGroup keyword");
				if (!curr.value.HasValue())
					throw GetException("Expected SmallGroup value");
				yesnoquestion.SmallGroup = GetString();
				YesNoQuestions.Add(yesnoquestion);
			}
		}
		private void ParseShirtSizes()
		{
			lineno++;
			if (curr.indent == 0)
				return;
			var startindent = curr.indent;
			while (curr.indent == startindent)
			{
				var shirtsize = new ShirtSize();
				if (curr.kw != RegKeywords.None)
					throw GetException("unexpected line");
				shirtsize.Description = GetLine();
				shirtsize.SmallGroup = shirtsize.Description;
				if (curr.indent > startindent)
				{
					if (curr.kw != RegKeywords.SmallGroup)
						throw GetException("expected SmallGroup keyword");
					shirtsize.SmallGroup = GetString(shirtsize.SmallGroup);
				}
				ShirtSizes.Add(shirtsize);
			}
		}
		private void AddStandardSizes()
		{
			var q = from ss in Db.ShirtSizes
					orderby ss.Id
					select new ShirtSize
					{
						SmallGroup = ss.Code,
						Description = ss.Description
					};
			_ShirtSizes = q.ToList();
		}
		private void ParseAgeGroups()
		{
			lineno++;
			if (curr.indent == 0)
				return;
			var startindent = curr.indent;
			while (curr.indent == startindent)
			{
				var agegroup = new AgeGroup();
				if (curr.kw != RegKeywords.None)
					throw GetException("unexpected line");
				agegroup.SmallGroup = GetLine();
				var m = agerange.Match(agegroup.SmallGroup);
				if (!m.Success)
				{
					lineno--;
					throw GetException("expected age range like 0-12");
				}
				agegroup.StartAge = m.Groups["start"].Value.ToInt();
				agegroup.EndAge = m.Groups["end"].Value.ToInt();
				if (curr.indent <= startindent)
					throw GetException("expected either indented SmallGroup or Fee");
				var ind = curr.indent;
				while (curr.indent == ind)
				{
					switch (curr.kw)
					{
						case RegKeywords.SmallGroup:
							agegroup.SmallGroup = GetString(agegroup.SmallGroup);
							break;
						case RegKeywords.Fee:
							agegroup.Fee = GetDecimal();
							break;
						default:
							throw GetException("unexpected line");
					}
				}
				AgeGroups.Add(agegroup);
			}
		}
		private void ParseGradeOptions()
		{
			GradeOptionsLabel = GetString("GradeOptions");
			if (curr.indent == 0)
				throw GetException("expected indented Options");
			var startindent = curr.indent;
			while (curr.indent == startindent)
			{
				if (curr.kw != RegKeywords.None)
					throw GetException("expected description only");
				var option = new GradeOption();
				option.Description = GetLine();
				if (curr.indent <= startindent)
					throw GetException("expected greater indent");
				if (curr.kw != RegKeywords.Code)
					throw GetException("expected Code");
				var code = GetNullInt();
				if (!code.HasValue)
				{
					lineno--;
					throw GetException("expected integer code");
				}
				option.Code = code.Value;
				GradeOptions.Add(option);
			}
		}
		void ParseOrgMemberFees()
		{
			lineno++;
			var startindent = curr.indent;
			while (curr.indent == startindent)
			{
				if (curr.kw != RegKeywords.None)
					throw GetException("expected orgid only");
				var oid = GetInt();
				if (oid == 0)
				{
					lineno--;
					throw GetException("invalid orgid");
				}
				var orgfee = new OrgFee { OrgId = oid };
				if (curr.indent > startindent)
				{
					if (curr.kw != RegKeywords.Fee)
						throw GetException("expected fee");
					orgfee.Fee = GetDecimal();
				}
				OrgFees.Add(orgfee);
			}
		}
		public override string ToString()
		{
			var sb = new StringBuilder();

			AddConfirmation(sb);
			AddVoteTags(sb);
			AddFees(sb);
			AddDonation(sb);
			AddValueCk(0, sb, "ValidateOrgs", ValidateOrgs);

			sb.AppendLine();
			AddAgeGroups(sb);
			AddOrgFees(sb);
			AddRequest(sb);
			AddTickets(sb);
			AddGradeOptions(sb);
			AddYesNoQuestions(sb);
			AddExtraQuestions(sb);
			AddTimeSlots(sb);
			AddMenuItems(sb);
			AddDropdown1(sb);
			AddDropdown2(sb);
			AddDropdown3(sb);
			AddCheckBoxes(sb);
			AddCheckBoxes2(sb);

			AddValueCk(0, sb, "AskShirtSize", AskShirtSize);
			AddValueCk(0, sb, "ShirtFee", ShirtFee);
			AddValueCk(0, sb, "AllowLastYearShirt", AllowLastYearShirt);
			AddShirtSizes(sb);
			AddValueCk(0, sb, "Shell", Shell);
			AddValueCk(0, sb, "AllowOnlyOne", AllowOnlyOne);
			AddValueCk(0, sb, "AllowReRegister", AllowReRegister);
			AddValueCk(0, sb, "MemberOnly", MemberOnly);
			AddValueCk(0, sb, "AskParents", AskParents);
			AddValueCk(0, sb, "AskDoctor", AskDoctor);
			AddValueCk(0, sb, "AskInsurance", AskInsurance);
			AddValueCk(0, sb, "AskEmContact", AskEmContact);
			AddValueCk(0, sb, "AskAllergies", AskAllergies);
			AddValueCk(0, sb, "AskMedical", AskMedical);
			AddValueCk(0, sb, "AskChurch", AskChurch);
			AddValueCk(0, sb, "AskTylenolEtc", AskTylenolEtc);
			AddValueCk(0, sb, "AskCoaching", AskCoaching);
			//AddValueCk(0, sb, "AskGrade", AskGrade);
			AddValueCk(0, sb, "GroupToJoin", GroupToJoin);
			AddValueCk(0, sb, "GiveOrgMembAccess", GiveOrgMembAccess);
			sb.AppendLine();

			AddValueCk(0, sb, "NotReqDOB", NotReqDOB);
			AddValueCk(0, sb, "NotReqAddr", NotReqAddr);
			AddValueCk(0, sb, "NotReqZip", NotReqZip);
			AddValueCk(0, sb, "NotReqPhone", NotReqPhone);
			AddValueCk(0, sb, "NotReqGender", NotReqGender);
			AddValueCk(0, sb, "NotReqMarital", NotReqMarital);
			sb.AppendLine();

			AddInstructions(sb);
			AddTerms(sb);
			return sb.ToString();
		}
		private void AddFees(StringBuilder sb)
		{
			AddValueCk(0, sb, "Fee", Fee);
			AddValueCk(0, sb, "Deposit", Deposit);
			AddValueCk(0, sb, "AskSuggestedFee", SuggestedFee);
			AddValueCk(0, sb, "ExtraFee", ExtraFee);
			AddValueCk(0, sb, "MaximumFee", MaximumFee);
			sb.AppendLine();
		}
		private void AddDonation(StringBuilder sb)
		{
			AddValueCk(0, sb, "AskDonation", AskDonation);
			AddSingleOrMultiLine(0, sb, "DonationLabel", DonationLabel);
			AddValueCk(0, sb, "DonationFundId", DonationFundId);
			sb.AppendLine();
		}
		private void AddVoteTags(StringBuilder sb)
		{
			if (VoteTags.Count == 0)
				return;
			var sb2 = new StringBuilder();
			AddValueNoCk(0, sb, "VoteTags", "");
			foreach (var i in VoteTags)
			{
				AddSingleOrMultiLine(1, sb, "Display", i.Display);
				AddValueNoCk(2, sb, "SmallGroup", i.SmallGroup);
				AddValueNoCk(2, sb, "Message", i.Message);
				AddValueCk(2, sb, "Confirm", i.Confirm);
				sb2.AppendFormat(@"# <votetag id={0} confirm={1} smallgroup=""{2}"" message=""{3}"">{4}</votetag>\n",
					OrgId, i.Confirm, i.SmallGroup, i.Message, i.Display);
			}
			sb.AppendLine();
		}
		public string VoteTagsLinks()
		{
			if (VoteTags.Count == 0)
				return null;
			var sb2 = new StringBuilder();
			foreach (var i in VoteTags)
			{
				sb2.AppendFormat("<votetag id={0} confirm={1} smallgroup=\"{2}\" message=\"{3}\">{4}</votetag>\n",
						OrgId, i.Confirm, i.SmallGroup, i.Message, i.Display);
			}
			return sb2.ToString();
		}
		private void AddAgeGroups(StringBuilder sb)
		{
			if (AgeGroups.Count == 0)
				return;
			AddValueNoCk(0, sb, "AgeGroups", "");
			foreach (var i in AgeGroups)
			{
				AddValueCk(1, sb, "{0}-{1}".Fmt(i.StartAge, i.EndAge));
				AddValueCk(2, sb, "SmallGroup", i.SmallGroup);
				AddValueCk(2, sb, "Fee", i.Fee);
			}
			sb.AppendLine();
		}
		private void AddOrgFees(StringBuilder sb)
		{
			if (OrgFees.Count == 0)
				return;
			AddValueNoCk(0, sb, "OrgFees", "");
			foreach (var i in OrgFees)
			{
				AddValueCk(1, sb, "{0}".Fmt(i.OrgId));
				AddValueCk(2, sb, "Fee", i.Fee);
			}
			sb.AppendLine();
		}
		private void AddRequest(StringBuilder sb)
		{
			if (AskRequest == false)
				return;
			AddValueCk(0, sb, "AskRequest", AskRequest);
			AddValueCk(1, sb, "Label", RequestLabel);
			sb.AppendLine();
		}
		private void AddTickets(StringBuilder sb)
		{
			if (AskTickets == false)
				return;
			AddValueCk(0, sb, "AskTickets", AskTickets);
			AddValueCk(1, sb, "Label", NumItemsLabel);
		}
		private void AddGradeOptions(StringBuilder sb)
		{
			if (GradeOptions.Count == 0)
				return;
			AddValueNoCk(0, sb, "GradeOptions", GradeLabel);
			foreach (var g in GradeOptions)
			{
				AddValueCk(1, sb, g.Description);
				AddValueCk(2, sb, "Code", g.Code);
			}
			sb.AppendLine();
		}
		private void AddYesNoQuestions(StringBuilder sb)
		{
			if (YesNoQuestions.Count == 0)
				return;
			AddValueNoCk(0, sb, "YesNoQuestions", "");
			foreach (var ss in YesNoQuestions)
			{
				AddValueCk(1, sb, ss.Question);
				AddValueNoCk(2, sb, "SmallGroup", ss.SmallGroup);
			}
			sb.AppendLine();
		}
		private void AddExtraQuestions(StringBuilder sb)
		{
			if (ExtraQuestions.Count == 0)
				return;
			AddValueNoCk(0, sb, "ExtraQuestions", "");
			foreach (var q in ExtraQuestions)
				AddValueCk(1, sb, q.Question);
			sb.AppendLine();
		}
		private void AddMenuItems(StringBuilder sb)
		{
			if (MenuItems.Count == 0)
				return;
			AddValueNoCk(0, sb, "MenuItems", "");
			foreach (var i in MenuItems)
			{
				AddValueCk(1, sb, i.Description);
				AddValueCk(2, sb, "SmallGroup", i.SmallGroup);
				AddValueCk(2, sb, "Fee", i.Fee);
			}
			sb.AppendLine();
		}
		private void AddMenuItemsList(StringBuilder sb, IEnumerable<MenuItem> list)
		{
			foreach (var s in list)
			{
				AddValueCk(1, sb, s.Description);
				AddValueCk(2, sb, "SmallGroup", s.SmallGroup);
				AddValueCk(2, sb, "Fee", s.Fee);
				AddValueCk(2, sb, "Limit", s.Limit);
				AddValueCk(2, sb, "Time", s.MeetingTime.ToString2("M/d/yy h:mm tt"));
			}
		}
		private void AddDropdown1(StringBuilder sb)
		{
			if (Dropdown1.Count == 0)
				return;
			AddValueNoCk(0, sb, "Dropdown1", Dropdown1Label);
			AddMenuItemsList(sb, Dropdown1);
			sb.AppendLine();
		}
		private void AddDropdown2(StringBuilder sb)
		{
			if (Dropdown2.Count == 0)
				return;
			AddValueNoCk(0, sb, "Dropdown2", Dropdown2Label);
			AddMenuItemsList(sb, Dropdown2);
			sb.AppendLine();
		}
		private void AddDropdown3(StringBuilder sb)
		{
			if (Dropdown3.Count == 0)
				return;
			AddValueNoCk(0, sb, "Dropdown3", Dropdown3Label);
			AddMenuItemsList(sb, Dropdown3);
			sb.AppendLine();
		}
		private void AddCheckBoxes(StringBuilder sb)
		{
			if (Checkboxes.Count == 0)
				return;
			AddValueNoCk(0, sb, "Checkboxes", CheckBoxLabel);
			AddValueCk(1, sb, "Minimum", CheckboxMin);
			AddValueCk(1, sb, "Maximum", CheckboxMax);
			AddMenuItemsList(sb, Checkboxes);
			sb.AppendLine();
		}
		private void AddCheckBoxes2(StringBuilder sb)
		{
			if (Checkboxes2.Count == 0)
				return;
			AddValueNoCk(0, sb, "Checkboxes2", CheckBox2Label);
			AddValueCk(1, sb, "Minimum", Checkbox2Min);
			AddValueCk(1, sb, "Maximum", Checkbox2Max);
			AddMenuItemsList(sb, Checkboxes2);
			sb.AppendLine();
		}
		private void AddTimeSlots(StringBuilder sb)
		{
			if (TimeSlots.Count == 0)
				return;
			AddValueNoCk(0, sb, "TimeSlots", "");
			foreach (var c in TimeSlots)
			{
				AddValueCk(1, sb, c.Description);
				AddValueCk(2, sb, "Time", c.Time.ToString2("h:mm tt"));
				AddValueCk(2, sb, "DayOfWeek", c.DayOfWeek);
				AddValueCk(2, sb, "Limit", c.Limit);
			}
			sb.AppendLine();
		}
		private void AddShirtSizes(StringBuilder sb)
		{
			if (AskShirtSize != true || ShirtSizes.Count == 0)
				return;
			AddValueNoCk(0, sb, "ShirtSizes", "");
			foreach (var ss in ShirtSizes)
			{
				AddValueCk(1, sb, ss.Description);
				AddValueCk(2, sb, "SmallGroup", ss.SmallGroup);
			}
			sb.AppendLine();
		}
		private void AddConfirmation(StringBuilder sb)
		{
			AddValueNoCk(0, sb, "Confirmation", "");
			AddValueNoCk(1, sb, "Subject", Subject);
			AddSingleOrMultiLine(1, sb, "Body", Body);
		}
		private void AddSingleOrMultiLine(int n, StringBuilder sb, string Section, string ht)
		{
			if (ht == null)
				return;
			if (ht.Contains("\n"))
			{
				AddValueCk(n, sb, Section, "<<");
				sb.AppendLine("----------");
				sb.AppendLine(ht.Trim());
				sb.AppendLine("----------");
			}
			else
				AddValueNoCk(n, sb, Section, ht);
		}
		private void AddInstructions(StringBuilder sb)
		{
			AddValueNoCk(0, sb, "Instructions", "");
			AddSingleOrMultiLine(1, sb, "Login", InstructionLogin);
			AddSingleOrMultiLine(1, sb, "Select", InstructionSelect);
			AddSingleOrMultiLine(1, sb, "Find", InstructionFind);
			AddSingleOrMultiLine(1, sb, "Options", InstructionOptions);
			AddSingleOrMultiLine(1, sb, "Submit", InstructionSubmit);
			AddSingleOrMultiLine(1, sb, "Sorry", InstructionSorry);
		}
		private void AddTerms(StringBuilder sb)
		{
			AddSingleOrMultiLine(0, sb, "Terms", Terms);
		}
		//
		private void AddValueCk(int n, StringBuilder sb, string label, int? value)
		{
			if (value.HasValue)
				sb.AppendFormat("{0}{1}: {2}\n", new string('\t', n), label, value);
		}
		private void AddValueCk(int n, StringBuilder sb, string label, decimal? value)
		{
			if (value.HasValue)
				sb.AppendFormat("{0}{1}: {2}\n", new string('\t', n), label, value.ToString2("n2"));
		}
		private void AddValueCk(int n, StringBuilder sb, string label, bool? value)
		{
			if (value == true)
				sb.AppendFormat("{0}{1}: {2}\n", new string('\t', n), label, value.ToString());
		}
		private void AddValueCk(int n, StringBuilder sb, string label, string value)
		{
			if (value.HasValue())
				sb.AppendFormat("{0}{1}: {2}\n", new string('\t', n), label, value.Trim());
			else
				sb.AppendFormat("{0}#{1}:\n", new string('\t', n), label);
		}
		private void AddValueNoCk(int n, StringBuilder sb, string label, string value)
		{
			sb.AppendFormat("{0}{1}: {2}\n", new string('\t', n), label, value);
		}
		private void AddValueCk(int n, StringBuilder sb, string value)
		{
			if (value.HasValue())
				sb.AppendFormat("{0}{1}\n", new string('\t', n), value.Trim());
			else
				sb.AppendFormat("{0}#description\n", new string('\t', n));
		}
		public class OrgPickInfo
		{
			public int OrganizationId { get; set; }
			public string OrganizationName { get; set; }
		}
		public static List<OrgPickInfo> OrganizationsFromIdString(Organization org)
		{
			var a = org.OrgPickList.SplitStr(",").Select(ss => ss.ToInt()).ToArray();
			var d = new Dictionary<int, int>();
			var n = 0;
			foreach (var i in a)
				d.Add(n++, i);
			var q = (from o in DbUtil.Db.Organizations
					 where a.Contains(o.OrganizationId)
					 select new OrgPickInfo
					 {
						 OrganizationId = o.OrganizationId,
						 OrganizationName = o.OrganizationName
					 }).ToList();
			var list = (from op in q
						join i in d on op.OrganizationId equals i.Value into j
						from i in j
						orderby i.Key
						select op).ToList();
			return list;
		}
	}
}