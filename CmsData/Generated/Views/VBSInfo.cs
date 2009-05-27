using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace CmsData.View
{
	[Table(Name="VBSInfo")]
	public partial class VBSInfo
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _Id;
		
		private string _Name;
		
		private string _Name2;
		
		private int? _PeopleId;
		
		private string _UserInfo;
		
		private bool? _PubPhoto;
		
		private int? _MemberStatusId;
		
		private bool? _ActiveInAnotherChurch;
		
		private string _GradeCompleted;
		
		private int? _GenderId;
		
		private string _Request;
		
		private DateTime? _Uploaded;
		
		private string _OrgName;
		
		private int? _OrgId;
		
		private int? _DivId;
		
		
		public VBSInfo()
		{
		}

		
		
		[Column(Name="Id", Storage="_Id", DbType="int NOT NULL")]
		public int Id
		{
			get
			{
				return this._Id;
			}

			set
			{
				if (this._Id != value)
					this._Id = value;
			}

		}

		
		[Column(Name="Name", Storage="_Name", DbType="varchar(36)")]
		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (this._Name != value)
					this._Name = value;
			}

		}

		
		[Column(Name="Name2", Storage="_Name2", DbType="varchar(37)")]
		public string Name2
		{
			get
			{
				return this._Name2;
			}

			set
			{
				if (this._Name2 != value)
					this._Name2 = value;
			}

		}

		
		[Column(Name="PeopleId", Storage="_PeopleId", DbType="int")]
		public int? PeopleId
		{
			get
			{
				return this._PeopleId;
			}

			set
			{
				if (this._PeopleId != value)
					this._PeopleId = value;
			}

		}

		
		[Column(Name="UserInfo", Storage="_UserInfo", DbType="varchar(15)")]
		public string UserInfo
		{
			get
			{
				return this._UserInfo;
			}

			set
			{
				if (this._UserInfo != value)
					this._UserInfo = value;
			}

		}

		
		[Column(Name="PubPhoto", Storage="_PubPhoto", DbType="bit")]
		public bool? PubPhoto
		{
			get
			{
				return this._PubPhoto;
			}

			set
			{
				if (this._PubPhoto != value)
					this._PubPhoto = value;
			}

		}

		
		[Column(Name="MemberStatusId", Storage="_MemberStatusId", DbType="int")]
		public int? MemberStatusId
		{
			get
			{
				return this._MemberStatusId;
			}

			set
			{
				if (this._MemberStatusId != value)
					this._MemberStatusId = value;
			}

		}

		
		[Column(Name="ActiveInAnotherChurch", Storage="_ActiveInAnotherChurch", DbType="bit")]
		public bool? ActiveInAnotherChurch
		{
			get
			{
				return this._ActiveInAnotherChurch;
			}

			set
			{
				if (this._ActiveInAnotherChurch != value)
					this._ActiveInAnotherChurch = value;
			}

		}

		
		[Column(Name="GradeCompleted", Storage="_GradeCompleted", DbType="varchar(15)")]
		public string GradeCompleted
		{
			get
			{
				return this._GradeCompleted;
			}

			set
			{
				if (this._GradeCompleted != value)
					this._GradeCompleted = value;
			}

		}

		
		[Column(Name="GenderId", Storage="_GenderId", DbType="int")]
		public int? GenderId
		{
			get
			{
				return this._GenderId;
			}

			set
			{
				if (this._GenderId != value)
					this._GenderId = value;
			}

		}

		
		[Column(Name="Request", Storage="_Request", DbType="varchar(140)")]
		public string Request
		{
			get
			{
				return this._Request;
			}

			set
			{
				if (this._Request != value)
					this._Request = value;
			}

		}

		
		[Column(Name="Uploaded", Storage="_Uploaded", DbType="datetime")]
		public DateTime? Uploaded
		{
			get
			{
				return this._Uploaded;
			}

			set
			{
				if (this._Uploaded != value)
					this._Uploaded = value;
			}

		}

		
		[Column(Name="OrgName", Storage="_OrgName", DbType="varchar(60)")]
		public string OrgName
		{
			get
			{
				return this._OrgName;
			}

			set
			{
				if (this._OrgName != value)
					this._OrgName = value;
			}

		}

		
		[Column(Name="OrgId", Storage="_OrgId", DbType="int")]
		public int? OrgId
		{
			get
			{
				return this._OrgId;
			}

			set
			{
				if (this._OrgId != value)
					this._OrgId = value;
			}

		}

		
		[Column(Name="DivId", Storage="_DivId", DbType="int")]
		public int? DivId
		{
			get
			{
				return this._DivId;
			}

			set
			{
				if (this._DivId != value)
					this._DivId = value;
			}

		}

		
    }

}
