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
	[Table(Name="ConsecutiveAbsents")]
	public partial class ConsecutiveAbsent
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _OrganizationId;
		
		private int _PeopleId;
		
		private int? _Consecutive;
		
		private DateTime? _Lastattend;
		
		
		public ConsecutiveAbsent()
		{
		}

		
		
		[Column(Name="OrganizationId", Storage="_OrganizationId", DbType="int NOT NULL")]
		public int OrganizationId
		{
			get
			{
				return this._OrganizationId;
			}

			set
			{
				if (this._OrganizationId != value)
					this._OrganizationId = value;
			}

		}

		
		[Column(Name="PeopleId", Storage="_PeopleId", DbType="int NOT NULL")]
		public int PeopleId
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

		
		[Column(Name="consecutive", Storage="_Consecutive", DbType="int")]
		public int? Consecutive
		{
			get
			{
				return this._Consecutive;
			}

			set
			{
				if (this._Consecutive != value)
					this._Consecutive = value;
			}

		}

		
		[Column(Name="lastattend", Storage="_Lastattend", DbType="datetime")]
		public DateTime? Lastattend
		{
			get
			{
				return this._Lastattend;
			}

			set
			{
				if (this._Lastattend != value)
					this._Lastattend = value;
			}

		}

		
    }

}
